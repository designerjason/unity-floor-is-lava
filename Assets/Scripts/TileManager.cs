using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public bool isDeadly = false;
    public bool isPickup = false;

    [Header("Sprites")]
    private SpriteRenderer tileSprite;
    private SpriteRenderer tileParent;
    public GameObject tile;
    public GameObject sprite;    
    public Sprite lavaBootSprite;    
    public Sprite tileSpriteLava;

    void Start() {
        tileSprite = sprite.GetComponent<SpriteRenderer>();
        tileParent = tile.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isPickup)
        {
            tileSprite.color = Color.white;
            tileSprite.sprite = lavaBootSprite;
        } else {
            tileSprite.sprite = null;
            tileSprite.color = Color.black;
        }

        if(isDeadly)
        {
            tileParent.sprite = tileSpriteLava;
        }
    }
}
