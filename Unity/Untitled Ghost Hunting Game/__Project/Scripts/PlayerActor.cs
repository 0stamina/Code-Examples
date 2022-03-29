using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor
{
    public bool isPlayerControlled;
    public PlayerInput player;

    public void Move(Vector2 direction)
    {
        rb.velocity = direction * 10f;
    }

    public void Dodge()
    {
        Debug.Log("Dodge!!");
    }

}
