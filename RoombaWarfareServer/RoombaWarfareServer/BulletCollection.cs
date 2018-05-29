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
