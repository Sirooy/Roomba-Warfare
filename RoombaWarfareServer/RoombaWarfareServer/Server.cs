﻿using System;
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

    private int currentRound;
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
        gameState = "";
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

    private void ResetRound()
    {
        System.Diagnostics.Debug.WriteLine("Reseting round...");

        string state = players.GlobalRespawn(map);

        Interlocked.Increment(ref currentRound);

        lock (lockGameState)
        {
            gameState += state;
        }
    }

    private void CheckRoundStatus()
    {
        if (players.RedAlivePlayersCount == 0 ||
            players.BlueAlivePlayersCount == 0)
        {
            if (players.RedPlayersCount > 0 &&
                players.BluePlayersCount > 0)
                ResetRound();
        }
    }

    private void TranslateCommands(string[] commands)
    {
        foreach(string command in commands)
        {
            string[] commandParts = command.Split();

            switch ((ClientMessage)int.Parse(commandParts[0]))
            {
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

                case ClientMessage.NewPos:
                    {
                        break;
                    }
            }
        }
    }

    private void DequeueCommands()
    {
        Queue<string> commands = new Queue<string>();
        lock (messageQueue)
        {
            if(messageQueue.Count > 0)
            {
                commands = new Queue<string>(messageQueue);
                messageQueue.Clear();
            }
        }

        while(commands.Count > 0)
        {
            string allCommands = commands.Dequeue();
            string[] singleCommands = allCommands.Split(':');
            TranslateCommands(singleCommands);
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
                        + "Server is full!");
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
        System.Diagnostics.Debug.WriteLine("Client connected"); //Remove Later
        player.Send(map.Seed);
        string initialData = GetInitialData(player.ID);
        player.Send(initialData);
        PlayerType type = (PlayerType)int.Parse(player.Receive());
        player.Type = type;

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
                string data = player.Receive();

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
        lock (lockPlayers)
        {
            foreach (Player player in players)
            {
                data += (int)ServerMessage.NewPlayer + " " +
                    player.ToString() + ":";
            }
        }

        lock (lockBullets)
        {
            foreach (Bullet bullet in bullets)
            {
                data += (int)ServerMessage.NewBullet + " " +
                    bullet.ToString() + ":";
            }
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

