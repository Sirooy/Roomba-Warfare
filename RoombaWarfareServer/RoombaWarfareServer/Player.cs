using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

public enum PlayerTeam : byte { Red, Blue, Spectator }
public enum PlayerType : byte { Assault, Commander, Rusher, Tank }
public enum PlayerState : byte { Position, Angle }

public class Player : Entity
{
    public Action<int> OnPlayerDisconnectEvent;

    public static readonly byte RADIUS = 24;
    public static readonly byte WIDTH = 64;
    public static readonly byte HEIGHT = 64;

    public bool IsConnected { get { return client.Connected; } }
    public bool DataAvailable { get { return stream.DataAvailable; } }

    public int ID { get; set; }
    public float Angle { get; set; }
    public bool IsAlive { get; set; }
    public PlayerTeam Team { get; set; }
    public PlayerType Type { get; set; }

    //Saves the last movement state of the player(Angle and position)
    private string[] movementStatus;
    //Saves the commands that only need to be send to one player
    private string ownStatus;
    private string lastProcessedCommand;

    private float lastPosX;
    private float lastPosY;
    private short maxHealth;
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
    public string GetLastProcessedCommand()
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
        Team = PlayerTeam.Spectator;
        IsAlive = false;
        ownStatus = "";
        movementStatus = new string[2];
        movementStatus[(byte)PlayerState.Position] = "";
        movementStatus[(byte)PlayerState.Angle] = "";
    }

    public void Create(PlayerType type)
    {
        Type = type;
        switch (Type)
        {
            case PlayerType.Assault: maxHealth = 125; break;
            case PlayerType.Commander: maxHealth = 150; break;
            case PlayerType.Rusher: maxHealth = 75; break;
            case PlayerType.Tank: maxHealth = 175; break;
        }
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


    public bool CheckDead()
    {
        if (currentHealth <= 0)
        {
            IsAlive = false;
        }

        return IsAlive;
    }

    //Updates the player position
    public void Update(float posXIncrement, float posYIncrement
        ,Hitbox[] hitboxes)
    {
        lastPosX = PosX;
        lastPosY = PosY;
        PosX += posXIncrement;
        PosY += posYIncrement;

        if (CheckCollisions(hitboxes))
        {
            PosX = lastPosX;
            PosY = lastPosY;
        }
    }

    public void TakeDamage(ushort amount)
    {
        currentHealth -= (short)amount;
    }

    //Checks if the players collides with any block
    public bool CheckCollisions(Hitbox[] hitboxes)
    {
        int centerX = (int)PosX + WIDTH / 2;
        int centerY = (int)PosY + HEIGHT / 2;

        foreach (Hitbox hitbox in hitboxes)
        {
            if (CollidesWith(hitbox, centerX, centerY))
            {
                return true;
            }
        }

        return false;
    }

    //Check if the player collides with a block
    public bool CollidesWith(Hitbox hitbox,int centerX, int centerY)
    {
        int closestX = Math.Max((int)hitbox.x,
            Math.Min((int)hitbox.x + Hitbox.WIDTH, centerX));
        int closestY = Math.Max((int)hitbox.y,
            Math.Min((int)hitbox.y + Hitbox.HEIGHT, centerY));

        int distanceX = (centerX - closestX) * (centerX - closestX);
        int distanceY = (centerY - closestY) * (centerY - closestY);

        return (distanceX + distanceY) < (RADIUS * RADIUS);
    }

    //Respawns the player in the given position
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

    public void Disconnect()
    {
        client.Close();
        stream.Close();
    }
}
