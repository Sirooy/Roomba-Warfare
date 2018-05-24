using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

public enum PlayerTeam : byte { Red, Blue, Spectator }
public enum PlayerType : byte { Assault, Commander, Rusher, Tank }
public enum PlayerState : byte { Position, Angle }

public class Player : Entity
{
    public Action<int> OnPlayerDisconnectEvent;

    public bool IsConnected { get { return client.Connected; } }
    public bool DataAvailable { get { return stream.DataAvailable; } }

    public int ID { get; set; }
    public float Angle { get; set; }
    public bool IsAlive { get; set; }
    public PlayerTeam Team { get; set; }
    public PlayerType Type { get; set; }
    //
    public string[] MovementStatus { get; set; }
    //Saves the commands that only need to be send to one player
    public string Status { get; set; }

    private readonly short maxHealth;
    private short currentHealth;

    private TcpClient client;
    private NetworkStream stream;
    private BinaryFormatter sendSerializer;
    private BinaryFormatter receiveSerializer;

    public Player(TcpClient client)
    {
        ID = 0;
        this.client = client;
        stream = client.GetStream();
        client.NoDelay = true;
        sendSerializer = new BinaryFormatter();
        receiveSerializer = new BinaryFormatter();
        Status = "";
        IsAlive = false;
        MovementStatus = new string[2];
        MovementStatus[(byte)PlayerState.Position] = "";
        MovementStatus[(byte)PlayerState.Angle] = "";
    }

    //Sends the given data to the player (Works Non-blocking)
    public void Send(string data)
    {
        try
        {
            sendSerializer.Serialize(stream, data);
        }
        catch (Exception) { OnPlayerDisconnectEvent(ID); }
    }

    //Receive the data sent by the player (Blocks)
    public string Receive()
    {
        string data = "";

        try
        {
            data = (string)receiveSerializer.Deserialize(stream);
        }
        catch (Exception) { OnPlayerDisconnectEvent(ID); }

        return data;
    }

    public void Respawn(float posX, float posY)
    {
        IsAlive = true;
        currentHealth = maxHealth;
        PosX = posX;
        PosY = posY;
    }

    public string GetMovementStatus()
    {
        string status = "";

        for(int i=0;i < MovementStatus.Length; i++)
        {
            if(MovementStatus[i] != "")
            {
                status += MovementStatus[i];
                MovementStatus[i] = "";
            }
            
        }

        return status;
    }

    public string GetStatus()
    {
        string status = Status;
        Status = "";
        return status;
    }

    public override string ToString()
    {
        return ID + " " + (int)Type + " " + (int)Team + " " + 
            Convert.ToInt32(IsAlive) + " " + base.ToString() + " " +
            Angle.ToString("0.#");
    }

    public void EndConnection()
    {
        client.Close();
        stream.Close();
    }
}
