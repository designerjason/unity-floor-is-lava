using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int moveIncrement = 1;
    private float vertBounds = 2.5f;
    private float horizBounds = 4.5f;
    private GameManager gameManager;
    private GameObject bootsActive;
    public Animator animator;
    public GameObject smoke;
    private IEnumerator animatePlayer;
    private bool runOnce = false;
    private AudioSource audioSource;
    private int dieOnce;

    // Start is called before the first frame update
    void Awake()
    {
        animatePlayer = AnimatePlayer("Down");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        bootsActive = gameManager.lavaBoots;
        audioSource = gameObject.GetComponent<AudioSource>();
        dieOnce = 0;
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
                PlayerMoveActions("Down", new Vector2 (posX, posY - moveIncrement));
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) && posY != vertBounds)
            {
                PlayerMoveActions("Up", new Vector2 (posX, posY + moveIncrement));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && posX != -horizBounds)
            {
                PlayerMoveActions("Left", new Vector2 (posX - moveIncrement, posY));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && posX != horizBounds)
            {
                PlayerMoveActions("Right", new Vector2 (posX + moveIncrement, posY));
            }
        }
    }

    // do the things when a player moves, like play a sound etc.
    void PlayerMoveActions(string direction, Vector2 position)
    {
        animatePlayer = AnimatePlayer(direction);
        StartCoroutine(animatePlayer);
        transform.position = position;
        audioSource.PlayOneShot(gameManager.audioWalking);
    }


    // play animation for player once, depending on direction
    IEnumerator AnimatePlayer(string direction) 
    {
        animator.SetBool(direction, true);
        yield return new WaitForSeconds(0.1f); 
        animator.SetBool(direction, false);
    }


    // if player collides with deadly tile, gameover!
    void OnTriggerEnter2D(Collider2D other)
    {
        // stepped on pickup
        if (other.gameObject.GetComponent<TileManager>().isPickup)
        {
            audioSource.PlayOneShot(gameManager.audioPickup);
            gameManager.lavaBootsHealth = 100;

            if(DifficultySelect.difficulty == "Easy") {
                gameManager.lavaBootsHealth = 200;
            } else {
                gameManager.lavaBootsHealth = 100;
            }

            gameManager.healthText.text = gameManager.lavaBootsHealth.ToString() + "%";
            bootsActive.SetActive(true);
            other.gameObject.GetComponent<TileManager>().isPickup = false;
        }

        // stepped on a deadly tile
        if (other.gameObject.GetComponent<TileManager>().isDeadly)
        {
            gameManager.onDeadlyTile = true;
            if (!bootsActive.activeSelf)
            {
                smoke.SetActive(true);
                audioSource.PlayOneShot(gameManager.audioDeath);
                gameManager.GameOver();
            }
        }

        // tile is not deadly
        else
        {
            gameManager.onDeadlyTile = false;
            runOnce = false;
            if (bootsActive.activeSelf)
            {
                smoke.SetActive(false);
                // stop coroutine to reduce lavaboot strength
                StopCoroutine(gameManager.lavaDamage);
            }
        }
    }


    // whilst inside a trigger, e.g lava
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<TileManager>().isDeadly)
        {
            gameManager.onDeadlyTile = true;
            if(bootsActive.activeSelf && !runOnce) {
                smoke.SetActive(true);
                StartCoroutine(gameManager.lavaDamage);
                
                // I need to stop the coroutine triggering over and over, so use a bool as a switch
                runOnce = true;
            } else if(!bootsActive.activeSelf) {
                
                // stop repeating, as above
                while(dieOnce == 0) {
                    audioSource.PlayOneShot(gameManager.audioDeath);
                    dieOnce++;
                } 

                gameManager.GameOver();
            }  
        } else {
            gameManager.onDeadlyTile = false;
        }
    }
}
