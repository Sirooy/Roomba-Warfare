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

    private TcpListener listener;
    private PlayerCollection players;
    private BulletCollection bullets;
    private ushort currentRound;
    private ushort currentID;
    private string gameState;

    private Queue<string> messageQueue;
    private Task physics;
    private Task broadcast;

    private object lockGameState = new object();
    
    public Server()
    {
        messageQueue = new Queue<string>();
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
    public void BroadcastGameStateLoop()
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
    public void PhysicsLoop()
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
    public async void AcceptClients()
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
                    //TO DO
                    Task listen = Task.Run(() => ListenClient(newPlayer));
                    currentID++;
                }
                
            }
            catch (Exception) { }
        }
    }

    public void ListenClient(Player player)
    {
        //TO DO (Send the initial data to the client)
        while (IsOpen)
        {
            while (player.IsConnected)
            {

            }
        }
    }

    //Close the server
    public void Close()
    {
        currentID = 0;
        //TO DO
    }
}

