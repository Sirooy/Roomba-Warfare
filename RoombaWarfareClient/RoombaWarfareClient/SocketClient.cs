using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

public class SocketClient
{
    public event Action OnDisconnectionEvent;

    public bool DataAvailable { get { return stream.DataAvailable; } }

    private TcpClient client;
    private NetworkStream stream;
    private BinaryFormatter serializer;

    public SocketClient()
    {
        client = new TcpClient();
        serializer = new BinaryFormatter();
    }

    //Connects to the server or returns false if it fails
    public bool Connect()
    {
        try
        {
            string[] addressParts = Game.ServerAddress.Split('-');
            IPAddress ip;
            IPAddress.TryParse(addressParts[0], out ip);
            ushort port = ushort.Parse(addressParts[1]);

            client.Connect(ip, port);

            if (client.Connected)
            {
                stream = client.GetStream();
                client.Client.NoDelay = true;
                
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    //Send a message to the server
    public void Send(string data)
    {
        try
        {
            serializer.Serialize(stream, data);
        }
        catch (Exception) { OnDisconnectionEvent(); }
    }

    //Receives a message from the server
    public string Receive()
    {
        string ret = "";

        try
        {
            ret = (string)serializer.Deserialize(stream);
        }
        catch (Exception) { OnDisconnectionEvent(); }

        return ret;
    }

    //Disconnects the client
    public void Disconnect()
    {
        client.Client.Close();
        stream.Close();
    }
}
