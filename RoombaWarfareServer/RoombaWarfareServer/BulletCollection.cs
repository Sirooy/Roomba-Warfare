using System;
using System.Collections;
using System.Collections.Generic;

public class BulletCollection : IEnumerable<Bullet>
{
    private List<Bullet> bullets;

    private object lockBullets = new object();

    public BulletCollection()
    {
        bullets = new List<Bullet>();
    }

    public string Add(string[] commadParts,PlayerCollection players)
    {
        int id = int.Parse(commadParts[1]);
        float dirX = float.Parse(commadParts[2]);
        float dirY = float.Parse(commadParts[3]);
        PlayerTeam bulletTeam = players[id].Team;
        PlayerType bulletType = players[id].Type;
        float playerPosX = players[id].PosX;
        float playerPosY = players[id].PosY;
        Bullet newBullet = new Bullet
            (dirX, dirY, bulletTeam, bulletType, playerPosX, playerPosY);

        lock (lockBullets)
        {
            bullets.Add(newBullet);
        }
        //-
        return (int)ServerMessage.NewBullet + " " + newBullet.ToString() + ":";
    }

    public string Update(float deltaTime,Hitbox[] hitboxes
        ,PlayerCollection players)
    {
        string ret = "";

        lock (lockBullets)
        {
            for (int index = 0; index < bullets.Count; index++)
            {
                bullets[index].Update(deltaTime);
                //Checks if the bullet collides with any wall and deletes it
                if (bullets[index].CollidesWith(hitboxes))
                {
                    ret += (int)ServerMessage.RemoveBullet + " "
                        + index + ":";
                    bullets.RemoveAt(index);
                    index--;
                }
                //Checks if the bullet collides with a player
                else
                {
                    int playerID = bullets[index].CollidesWith(players);

                    if(playerID != -1)
                    {
                        ret += (int)ServerMessage.RemoveBullet + " "
                            + index + ":";
                        players[playerID].AddOwnStatus(
                            (int)ServerMessage.DamagePlayer + " " 
                            + bullets[index].Damage + ":");
                        players[playerID].TakeDamage(bullets[index].Damage);

                        if (!players[playerID].CheckDead())
                        {
                            ret += (int)ServerMessage.KillPlayer + " " +
                                playerID + ":";
                            players.CalculatePlayers();
                        }

                        bullets.RemoveAt(index);
                        index--;
                    }
                }
            }
        }

        return ret;
    }

    public IEnumerator<Bullet> GetEnumerator()
    {
        lock (lockBullets)
        {
            foreach (Bullet bullet in bullets)
            {
                yield return bullet;
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
