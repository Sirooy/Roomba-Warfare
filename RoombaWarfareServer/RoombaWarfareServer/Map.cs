using System;
using System.IO;
using System.Collections.Generic;

public struct Hitbox
{
    public static readonly byte WIDTH = 64;
    public static readonly byte HEIGHT = 64;

    public uint x;
    public uint y;

    public Hitbox(uint x, uint y)
    {
        this.x = x;
        this.y = y;
    }
}

//This class contains the map of the game and all its hitboxes.
public class Map
{
    public static uint Width;
    public static uint Height;

    private List<Hitbox> hitboxes;
    private List<SpawnPoint> blueSpawnPoints;
    private List<SpawnPoint> redSpawnPoints;

    public Map()
    {
        Width = 0;
        Height = 0;
        hitboxes = new List<Hitbox>();
        blueSpawnPoints = new List<SpawnPoint>();
        redSpawnPoints = new List<SpawnPoint>();
    }

    //Creates the map or returns false if it fails
    public bool Create(string path)
    {
        hitboxes = new List<Hitbox>();

        if (File.Exists(path))
        {
            try
            {
                using(StreamReader readFile = new StreamReader(path))
                {
                    //Save the width and height
                    string size = readFile.ReadLine();
                    string[] sizeParts = size.Split();
                    Width = ushort.Parse(sizeParts[0]);
                    Height = ushort.Parse(sizeParts[1]);

                    int posY = 0;
                    string mapLine = readFile.ReadLine();
                    while(mapLine != null)
                    {
                        string[] mapParts = mapLine.Split();

                        for(int posX = 0;posX < mapParts.Length; posX++)
                        {
                            switch (mapParts[posX])
                            {
                                //Any block that has an H is a hitbox
                                case "H1":
                                case "H2":
                                case "H3":
                                case "H4":
                                    hitboxes.Add
                                        (new Hitbox((uint)posX * Hitbox.WIDTH
                                        , (uint)posY * Hitbox.HEIGHT));
                                    break;
                                //Blue spawnpoint
                                case "BS":
                                    blueSpawnPoints.Add
                                        (new SpawnPoint(
                                        (uint)posX * Hitbox.WIDTH,
                                        (uint)posY * Hitbox.HEIGHT));
                                    break;
                                //Red spawnpoint
                                case "RS":
                                    redSpawnPoints.Add
                                        (new SpawnPoint(
                                        (uint)posX * Hitbox.WIDTH,
                                        (uint)posY * Hitbox.HEIGHT));
                                    break;
                            }
                        }

                        mapLine = readFile.ReadLine();
                        posY++;
                    }
                }

                return true;
            }
            catch (Exception) { return false; }
        }
        else
        {
            return false;
        }
    }

    //Returns true if the map loaded is valid
    public static bool ValidateMap(string path)
    {
        int blueSpawnPointsCount = 0, redSpawnPointsCount = 0;

        if (File.Exists(path))
        {
            try
            {
                using(StreamReader readFile = new StreamReader(path))
                {
                    string[] sizeParts = readFile.ReadLine().Split();

                    //If the first line is not two integers then return false
                    if (!uint.TryParse(sizeParts[0], out uint width))
                        return false;
                    if (!uint.TryParse(sizeParts[1], out uint height))
                        return false;

                    //If the map doesnt have spawnpoints return false
                    string line = readFile.ReadLine();
                    while(line != null)
                    {
                        string[] lineParts = line.Split();

                        foreach(string str in lineParts)
                        {
                            switch (str)
                            {
                                case "BS": blueSpawnPointsCount++; break;
                                case "RS": redSpawnPointsCount++; break;
                            }
                        }

                        line = readFile.ReadLine();
                    }
                }

                if (blueSpawnPointsCount == 0 || redSpawnPointsCount == 0)
                    return false;
            }
            catch (Exception) { return false; }
        }

        return true;
    }
}
