﻿using System.Collections;
using System.Collections.Generic;

public class BulletCollection : IEnumerable<Bullet>
{
    private List<Bullet> bullets;

    public BulletCollection()
    {
        bullets = new List<Bullet>();
    }

    public IEnumerator<Bullet> GetEnumerator()
    {
        foreach(Bullet bullet in bullets)
        {
            yield return bullet;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
