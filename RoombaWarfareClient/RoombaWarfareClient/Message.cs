﻿
//Type of messages that the server sends to the clients
public enum ServerMessage : byte
{
    Disconnect,
    LastCommandProcessed,
    SetID,
    NewPlayer,
    SetPlayerPosition,
    SetPlayerAngle,
    SetPlayerTeam,
    RemovePlayer,
    KillPlayer,
    NewBullet,
    RemoveBullet,
    Respawn,
}

//Type of messages that the client sends to the server
public enum ClientMessage : byte
{

}