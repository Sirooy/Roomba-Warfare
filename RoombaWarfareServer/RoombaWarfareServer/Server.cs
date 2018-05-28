using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

public class Server
{
    public bool IsOpen { get; set; }
    public ushort Port { get; set; }
    public byte TickRate { get; set; }
    public ushort MaxPlayers { get; set; }
    public ushort Rounds { get; set; }
    public string MapPath { get; set; }

    private TcpListener listener;
    private PlayerCollection players;
    private BulletCollection bullets;
    private Map map;

    private int currentRound;
    private int redPlayersWonRounds;
    private int bluePlayersWonRounds;
    private ushort currentID;
    private string gameState;

    private Queue<string> messageQueue;
    private Task physics;
    private Task broadcast;

    private object lockGameState = new object();
    private object lockMessageQueue = new object();
    private object lockStreamWriter = new object();

    private const string logFileName = "log.txt";

    public Server()
    {
        messageQueue = new Queue<string>();
        map = new Map();
        players = new PlayerCollection();
        players.OnPlayerDisconnectEvent += DisconnectPlayer;
        bullets = new BulletCollection();
        physics = new Task(PhysicsLoop);
        broadcast = new Task(BroadcastGameStateLoop);
        gameState = "";

        //Creates the log if it doesnt exists
        if (!File.Exists(logFileName))
        {
            File.CreateText(logFileName);
        }
    }

    public void WriteLog(string logText)
    {
        lock (lockStreamWriter)
        {
            try
            {
                using (StreamWriter writeFile = File.AppendText(logFileName))
                {
                    writeFile.WriteLine(DateTime.Now + " - " + logText);
                }
            }
            catch (Exception) { }
        }
    }

    //Starts all the server loops
    public void Start()
    {
        try
        {
            IsOpen = true; 

            map.Create(MapPath);

            redPlayersWonRounds = 0;
            bluePlayersWonRounds = 0;

            IPEndPoint address = new IPEndPoint(IPAddress.Any, Port);
            listener = new TcpListener(address);

            listener.Start();
            physics.Start();
            broadcast.Start();
            AcceptClients();
        }
        catch (Exception)
        {
            MessageBox.Show("Could not start the server", "Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }
    }

    //Broadcast the game state to all players at fixed rate
    private void BroadcastGameStateLoop()
    {
        float maxFrameRate = (1f / TickRate * 1000f);
        string state = "";

        while (IsOpen)
        {
            int startTime = Environment.TickCount;

            lock (gameState)
            {
                if(gameState != "")
                {
                    state += gameState;
                    gameState = "";
                }
            }

            //Get all the players last positions and angles
            foreach(Player player in players)
            {
                state += player.GetMovementStatus();
            }

            //Remove the last : and send the data
            if(state.Length > 0)
            {
                state = state.Remove(state.Length - 1);
                players.Broadcast(state);
                state = "";
            }
                
            //Sleep the thread if the frame rate is higher than the max frame rate
            int frameTime = Environment.TickCount - startTime;
            if (frameTime < maxFrameRate)
                Thread.Sleep((int)(maxFrameRate - frameTime));
        }
    }

    //Resets the round and adds a point to the winner
    private void ResetRound()
    {
        System.Diagnostics.Debug.WriteLine("Reseting round...");

        if (players.RedAlivePlayersCount != 0)
        {
            Interlocked.Increment(ref currentRound);
            Interlocked.Increment(ref redPlayersWonRounds);
        }
        else if (players.BlueAlivePlayersCount != 0)
        {
            Interlocked.Increment(ref currentRound);
            Interlocked.Increment(ref bluePlayersWonRounds);
        }
            
        string state = players.GlobalRespawn(map);

        lock (lockGameState)
        {
            gameState += state;
        }
    }

    //Checks if a round has ended
    private void CheckRoundStatus()
    {
        if (players.RedAlivePlayersCount == 0 ||
            players.BlueAlivePlayersCount == 0)
        {
            System.Diagnostics.Debug.WriteLine("0 players alive");
            if (players.RedPlayersCount > 0 &&
                players.BluePlayersCount > 0)
                ResetRound();
        }
    }

    //Translate the client commands.
    private void TranslateCommands(string[] commands)
    {
        foreach(string command in commands)
        {
            string[] commandParts = command.Split();

            switch ((ClientMessage)int.Parse(commandParts[0]))
            {
                //Sets the angle of a player
                case ClientMessage.NewAngle:
                    {
                        players.SetAngle(commandParts);
                        break;
                    }

                //Sets the position of a player
                case ClientMessage.NewPos:
                    {
                        players.SetPosition(commandParts);
                        break;
                    }

                //Changes the team of a player
                case ClientMessage.ChangeTeam:
                    {
                        string state = players.ChangeTeam(commandParts);
                        lock (lockGameState)
                        {
                            gameState += state;
                        }
                        CheckRoundStatus();
                        break;
                    }
            }
        }
    }

    //Dequeues all the commands from the messageQueue to translate then
    private void DequeueCommands()
    {
        lock (messageQueue)
        {
            while (messageQueue.Count > 0)
            {
                string allCommands = messageQueue.Dequeue();
                string[] singleCommands = allCommands.Split(':');
                TranslateCommands(singleCommands);
            }
        }
         
    }

    //Updates the physics of the game at 60 frames per second
    private void PhysicsLoop()
    {
        float maxFrameRate = (1f / 60f * 1000f);

        while (IsOpen)
        {
            int startTime = Environment.TickCount;

            DequeueCommands();

            //Sleep the thread if the frame rate is higher than the max frame rate
            int frameTime = Environment.TickCount - startTime;
            if (frameTime < maxFrameRate)
                Thread.Sleep((int)(maxFrameRate - frameTime));
        }
    }

    //Accept clients asynchronously
    private async void AcceptClients()
    {
        while (IsOpen)
        {
            try
            {
                TcpClient newClient = await listener.AcceptTcpClientAsync();
                Player newPlayer = new Player(newClient);

                //If the server has reached the max number of players
                //Send a disconnection message.
                if(MaxPlayers != 0 && players.Count == MaxPlayers)
                {
                    newPlayer.Send((int)ServerMessage.Disconnect + " "
                        + "Server-is-full");
                }
                //Add the player to the server list
                else
                {
                    newPlayer.ID = currentID;
                    WriteLog("Client connected : " + newClient.Client.LocalEndPoint);
                    Task listen = Task.Run(() => 
                        ListenClient(newPlayer));
                    currentID++;
                }
                
            }
            catch (Exception) { }
        }
    }

    //Disconects a player and sends the info to the others
    public void DisconnectPlayer(int id)
    {
        string state = players.Remove(id);
        System.Diagnostics.Debug.WriteLine("Client disconnected " + state); //Remove Later

        lock (lockGameState)
        {
            gameState += state;
        }

        players.CalculatePlayers();
        CheckRoundStatus();
    }

    //Receives all the data send by a client
    private void ListenClient(Player player)
    {
        System.Diagnostics.Debug.WriteLine("Client connected"); //Remove Later
        string initialData = GetInitialData(player.ID);
        player.Send(initialData);
        player.Send(map.Seed);
        PlayerType type = (PlayerType)int.Parse(player.Receive());
        player.Type = type;

        //Once we send all the data and receive the player type
        //Add the player to the list and send the data to the others
        lock (lockGameState)
        {
            gameState += players.Add(player.ID, player);
        }

        while (IsOpen && player.IsConnected)
        {
            string data = player.Receive();
            
            if(data != "")
            {
                lock (lockMessageQueue)
                {
                    messageQueue.Enqueue(data);
                }
            }
        }
    }

    //Returns the data of all the entities
    private string GetInitialData(int id)
    {
        string data = (int)ServerMessage.SetID + " " + id + ":";

        //Get all the players state as if they were new
        foreach (Player player in players)
        {
            data += (int)ServerMessage.NewPlayer + " " +
                player.ToString() + ":";
        }

        foreach (Bullet bullet in bullets)
        {
            data += (int)ServerMessage.NewBullet + " " +
                bullet.ToString() + ":";
        }

        data = data.Remove(data.Length - 1);
        
        return data;
    }

    //Close the server
    public void Close()
    {
        currentID = 0;
        //TO DO
    }
}

