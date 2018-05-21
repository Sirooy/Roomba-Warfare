using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;

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

    private ushort currentRound;
    private ushort currentID;
    private string gameState;

    private Queue<string> messageQueue;
    private Task physics;
    private Task broadcast;

    private object lockGameState = new object();
    private object lockMessageQueue = new object();
    private object lockBullets = new object();
    private object lockPlayers = new object();


    public Server()
    {
        messageQueue = new Queue<string>();
        map = new Map();
        players = new PlayerCollection();
        bullets = new BulletCollection();
        physics = new Task(PhysicsLoop);
        broadcast = new Task(BroadcastGameStateLoop);
    }

    //Starts all the server loops
    public void Start()
    {
        try
        {
            IsOpen = true; 

            map.Create(MapPath);

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
        float maxFrameRate = (1f / TickRate) * 1000f;

        while (IsOpen)
        {
            int startTime = Environment.TickCount;

            //TO DO

            //Sleep the thread if the frame rate is higher than the max frame rate
            int frameTime = Environment.TickCount - startTime;
            if (frameTime < maxFrameRate)
                Thread.Sleep((int)(maxFrameRate - frameTime));
        }
    }

    //Updates the physics of the game at 60 frames per second
    //and 
    private void PhysicsLoop()
    {
        float maxFrameRate = (1f / 60f) * 1000f;

        while (IsOpen)
        {
            int startTime = Environment.TickCount;

            //TO DO

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
                    newPlayer.Send((int)ServerMessage.Disconnect + "");
                }
                //Add the player to the server list
                else
                {
                    newPlayer.ID = currentID;
                    Task listen = Task.Run(() => 
                        ListenClient(newPlayer));
                    currentID++;
                }
                
            }
            catch (Exception) { }
        }
    }

    //Receives all the data send by a client
    private void ListenClient(Player player)
    {
        try
        {
            player.Send(map.Seed);
            player.Send(GetInitialData());
            PlayerType type = (PlayerType)int.Parse(player.Receive());

            //Once we send all the data and receive the player type
            //Add the player to the list and send the data to the others
            lock (lockGameState)
            {
                gameState += players.Add(player.ID, player);
            }

            while (IsOpen)
            {
                while (player.IsConnected)
                {
                    if (player.DataAvailable)
                    {
                        string data = player.Receive();

                        lock (lockMessageQueue)
                        {
                            messageQueue.Enqueue(data);
                        }
                    }
                }
            }
        }
        catch (Exception){ }
    }

    //Returns the data of all the entities
    private string GetInitialData()
    {
        string data = "";

        //Get all the players state as if they were new
        lock (lockPlayers)
        {
            foreach (Player player in players)
            {
                data += (int)ServerMessage.NewPlayer +
                    player.ToString() + ":";
            }
        }

        //TO DO bullets
        lock (lockBullets)
        {
            foreach (Bullet bullet in bullets)
            {
                data += (int)ServerMessage.NewBullet +
                    bullet.ToString() + ":";
            }
        }
        
        return data;
    }

    //Close the server
    public void Close()
    {
        currentID = 0;
        //TO DO
    }
}

