using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

public enum PlayerTeam { Red, Blue, Spectator }
public enum PlayerType { Assault, Commander, Rusher, Tank }

public class Player
{
    public Action<int> OnPlayerDisconnectEvent;

    public bool IsConnected { get { return client.Connected; } }
    public bool DataAvailable { get { return stream.DataAvailable; } }

    public int ID { get; set; }
    public float Angle { get; set; }
    public bool IsAlive { get; set; }
    public PlayerTeam Team { get; set; }
    public PlayerTeam Type { get; set; }

    private readonly short maxHealth;
    private ushort currentHealth;

    private TcpClient client;
    private NetworkStream stream;
    private BinaryFormatter serializer;

    private object lockSerializer = new object();

    public Player(TcpClient client)
    {
        ID = 0;
        this.client = client;
        stream = client.GetStream();
        serializer = new BinaryFormatter();
    }

    //Sends the given data to the player (Works Non-blocking)
    public void Send(string data)
    {
        try
        {
            lock (lockSerializer)
            {
                serializer.Serialize(stream, data);
            }
        }
        catch (Exception) { OnPlayerDisconnectEvent(ID); }
    }

    //Receive the data sent by the player (Blocks)
    public string Receive()
    {
        string data = "";

        try
        {
            lock (lockSerializer)
            {
                data = (string)serializer.Deserialize(stream);
            }
        }
        catch (Exception) { OnPlayerDisconnectEvent(ID); }

        return data;
    }

    public override string ToString()
    {
        return ID + " " + (int)Type + " " + (int)Team + " " + 
            Convert.ToInt32(IsAlive) + " " + base.ToString() + " " +
            Angle.ToString("0.#");
    }
}
