using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public bool isDeadly = false;
    public bool isPickup = false;
    public GameObject sprite;

    void Update()
    {
        if (isPickup)
        {
            sprite.GetComponent<SpriteRenderer>().color = Color.green;
        } else {
            sprite.GetComponent<SpriteRenderer>().color = Color.black;
        }

        if(isDeadly)
        {
            sprite.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
}
