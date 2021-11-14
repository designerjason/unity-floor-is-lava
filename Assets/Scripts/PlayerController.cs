using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int moveIncrement = 1;
    private float vertBounds = 2.5f;
    private float horizBounds = 4.5f;
    private string playerFacing;
    private GameManager gameManager;
    private GameObject bootsActive;


    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        bootsActive = gameManager.lavaBoots;
        //playerFacing = "down";
    }


    void Update()
    {
        MovePlayer();
    }


    // input keys move player
    void MovePlayer()
    {
        if (!gameManager.gameOver)
        {
            float posX = transform.position.x;
            float posY = transform.position.y;

            if (Input.GetKeyDown(KeyCode.DownArrow) && posY != -vertBounds)
            {
                transform.position = new Vector2(posX, posY - moveIncrement);
                //playerFacing = "down";

            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && posY != vertBounds)
            {
                transform.position = new Vector2(posX, posY + moveIncrement);
                //playerFacing = "up";

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && posX != -horizBounds)
            {
                transform.position = new Vector2(posX - moveIncrement, posY);
                //playerFacing = "left";

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && posX != horizBounds)
            {
                transform.position = new Vector2(posX + moveIncrement, posY);
                //playerFacing = "right";
            }
        }
    }


    // if player collides with deadly tile, gameover!
    void OnTriggerEnter2D(Collider2D other)
    {
        // stepped on pickup
        if (other.gameObject.GetComponent<TileManager>().isPickup)
        {
            gameManager.lavaBootshealth = 100;
            gameManager.healthText.text = gameManager.lavaBootshealth.ToString() + "%";
            bootsActive.SetActive(true);
            other.gameObject.GetComponent<TileManager>().isPickup = false;
        }

        // stepped on a deadly tile?
        if (other.gameObject.GetComponent<TileManager>().isDeadly)
        {
            gameManager.onDeadlyTile = true;
            if (bootsActive.activeSelf)
            {
                // start coroutine to reduce lavaboot strength
                StartCoroutine(gameManager.lavaDamage);
            }
            else
            {
                gameManager.GameOver();
            }
        }
        else
        {
            gameManager.onDeadlyTile = false;
            if (bootsActive.activeSelf)
            {
                // start coroutine to reduce lavaboot strength
                StopCoroutine(gameManager.lavaDamage);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<TileManager>().isDeadly)
        {
            if (!bootsActive.activeSelf)
            {
                gameManager.GameOver();
            }
        }
    }
}
