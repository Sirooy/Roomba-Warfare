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

    //Saves the last movement status of the player(Angle and position)
    private string[] movementStatus;
    //Saves the commands that only need to be send to one player
    private string ownStatus;
    private string lastProcessedCommand;

    private readonly short maxHealth;
    private short currentHealth;

    private TcpClient client;
    private NetworkStream stream;
    private BinaryFormatter sendSerializer;
    private BinaryFormatter receiveSerializer;

    //Sets the last processed movement command of the player
    public void SetLastProcessedCommand(uint newLastProcessedCommand)
    {
        lastProcessedCommand = (int)(ServerMessage.LastCommandProcessed)
            + " " + newLastProcessedCommand + ":";
    }

    //Gets the last processed movement command of the player and clears it
    public string getLastProcessedCommand()
    {
        string lastCommand = lastProcessedCommand;
        lastProcessedCommand = "";
        return lastCommand;
    }

    public void SetMovementStatus(string command, PlayerState type)
    {
        movementStatus[(byte)type] = command;
    }

    //Get the last movement status of the player and clears it
    public string GetMovementStatus()
    {
        string status = "";

        for (int i = 0; i < movementStatus.Length; i++)
        {
            if (movementStatus[i] != "")
            {
                status += movementStatus[i];
                movementStatus[i] = "";
            }

        }

        return status;
    }

    //Adds a command to its personal status
    public void AddOwnStatus(string addition)
    {
        ownStatus += addition;
    }

    //Returns the player status and clears it
    public string GetOwnStatus()
    {
        string status = ownStatus;
        ownStatus = "";
        return status;
    }

    public Player(TcpClient client)
    {
        ID = 0;
        this.client = client;
        stream = client.GetStream();
        client.NoDelay = true;
        sendSerializer = new BinaryFormatter();
        receiveSerializer = new BinaryFormatter();
        IsAlive = false;
        ownStatus = "";
        movementStatus = new string[2];
        movementStatus[(byte)PlayerState.Position] = "";
        movementStatus[(byte)PlayerState.Angle] = "";
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
