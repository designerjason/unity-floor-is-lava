using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Tile Settings")]
    private float posLeft = -4.5f;
    private float posTop = 2.5f;
    private int tileCount = 60; // best as multiple of 10
    public GameObject tile;
    private int tileIndex = 0;
    [SerializeField] private List<string> activeTiles = new List<string>();


    [Header("Game Settings")]
    private int timeLimit = 60;
    public GameObject player;
    public bool gameOver = false;
    // create this so we can start and stop co-routine wherever
    public IEnumerator lavaDamage;
    public bool onDeadlyTile;
    string currentScene;


    [Header("HUD")]
    [SerializeField] private GameObject gameOverScreen;
    public TextMeshProUGUI timer;
    public GameObject lavaBoots;
    public int lavaBootshealth = 100;
    public TextMeshProUGUI healthText;

    void Awake() {
        currentScene = SceneManager.GetActiveScene().name;

        if( currentScene == "Game" ) {
            StartGame();
        }
    }

    void Update()
    {
        if(lavaBootshealth == 0) {
            lavaBoots.SetActive(false);
        }

        if(lavaBootshealth == 0 && onDeadlyTile) {
            GameOver();
        }

        // and check if not on lava
        if(activeTiles.Count == 1 && !gameOver) {
           GameWin();
        }
    }


    // place a pickup on the screen
    void SetPickup(int pickupCount)
    {
        while(pickupCount > 0) {
            int randomTile = Random.Range(0, timeLimit);
            GameObject activeTile = GameObject.Find(activeTiles[randomTile].ToString());
            GameObject activeTileSprite = GameObject.Find(activeTiles[randomTile].ToString() + "/Square");

            activeTile.GetComponent<TileManager>().isPickup = true;
            pickupCount--;
        }

    }


    // initialise the tiles on the screen
    void GenerateTiles()
    {
        while (tileCount > 0)
        {
            // place and name tile
            GameObject thisTile = Instantiate(tile, new Vector3(posLeft, posTop, 0), tile.transform.rotation);
            thisTile.name = "tile_" + tileIndex;

            // add tile to List 
            activeTiles.Add(thisTile.name);

            // move tile placement across x by one tile
            posLeft++;

            // go onto the next row underneath (y)
            if (posLeft == 5.5f)
            {
                posLeft = -4.5f;
                posTop--;
            }

            // reduce the total tiles left available for the loop
            tileCount -= 1;

            // increment index for current tile
            tileIndex++;
        }
    }

    // restart level
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    // restart level
    public void GameQuit()
    {
        SceneManager.LoadScene("StartMenu");
    }


    //run game
    void StartGame()
    {
        // set coroutine as variable so we can start stop wherever
        lavaDamage = LavaBootDamage();

        GenerateTiles();
        SpawnObject(player);

        if(DifficultySelect.difficulty == "Easy") {
            SetPickup(3);
        }
        if(DifficultySelect.difficulty == "Normal") {
            SetPickup(1);
        }

        timer.text = timeLimit.ToString(); 
        gameOver = false;
        StartCoroutine("CountDown");
    }


    // countdown, making tiles deadly
    IEnumerator CountDown()
    {
        while (activeTiles.Count > 1 && !gameOver)
        {
            yield return new WaitForSeconds(1);
            int randomTile = Random.Range(0, timeLimit);
            GameObject thisTile = GameObject.Find(activeTiles[randomTile].ToString());
            thisTile.GetComponent<TileManager>().isDeadly = true;
            thisTile.GetComponent<TileManager>().isPickup = false;

            //update hud
            timeLimit--;
            timer.text = timeLimit.ToString();

            //remove random tile
            activeTiles.RemoveAt(randomTile);
        }
    }


    //coroutine for damage
    public IEnumerator LavaBootDamage()
    {
        while (lavaBootshealth > 0 && !gameOver)
        {
            lavaBootshealth -= 25;
            healthText.text = lavaBootshealth.ToString() + "%";
            yield return new WaitForSeconds(1);
        }
    }


    // spawn an object on the screen
    void SpawnObject(GameObject spawnObject)
    {
        int randomTile = Random.Range(0, activeTiles.Count);
        string startTile = activeTiles[randomTile];
        Vector3 startPos = GameObject.Find(startTile).transform.position;

        Instantiate(spawnObject, startPos, player.transform.rotation);
    }


    //win the game!
    public void GameWin()
    {
        timer.text = "YOU WIN!!!";
    }


    //what happens when game over
    public void GameOver()
    {
        gameOver = true;
        StopCoroutine("CountDown");
        gameOverScreen.SetActive(true);
    }
}
