using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet_Bouncy : Bullet_Base {

    public int maxBounce;

    int bounceCnt;
    float speed;

    private void OnEnable()
    {
        bounceCnt = 0;
    }

    protected override void OnCollisionWithWall(Collision2D collision)
    {
        if (bounceCnt >= maxBounce)
        {
            Kill();
            return;
        }

        rb2d.velocity = Vector2.Reflect(rb2d.velocity, collision.contacts[0].normal);
        bounceCnt++;
    }
}
