﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public TowerButton towerSelected { get; private set; }
    [SerializeField] private GameObject roundButton;
    [SerializeField] public GameObject sellTowerButton;
    [SerializeField] private GameObject gameOverScreen;
    private int currency;
    public int round = 0;
    private int health;
    public int enemiesThisRound;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private TextMeshProUGUI currencyText;
    List<Enemies> aliveEnemies = new List<Enemies>();
    private bool gameOver = false;
    private bool doneSpawning;
    private Tower towerClicked;
    public bool roundActive{
        get {
            return aliveEnemies.Count > 0 && enemiesThisRound > 0 && doneSpawning;
        }
    }

    public ObjectStorage objStorage{get; set;}
    public int Currency{
        get{
            return currency;
        }
        set{
            this.currency = value;
            this.currencyText.text = "Money: $" + value.ToString();
        }
    }

    public int Health{
        get{
            return health;
        }
        set{
            this.health = value;
            healthText.text = "Health: " + value.ToString();
            if(health <= 0){
                this.health = 0;
                GameOver();
            }
        }
    } 
    private void Awake(){
        objStorage = GetComponent<ObjectStorage>();
    }

    // Start is called before the first frame update
    void Start(){
        Health = 100;
        Currency = 500;
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Whenever a TowerButton is clicked in the shop we check if the player has enough currency to buy the tower, and we call showHover() if we do. 
    //If we don't have enough money we simply cannot select the Tower. 
    public void TowerSelected(TowerButton towerBtn){
        if(currency >= towerBtn.Price){
            this.towerSelected = towerBtn;
            TowerHover tH = GameObject.FindObjectOfType<TowerHover>();
            tH.showHover(towerBtn.Sprite);
        }
    }

    //clickTower() is used whenever a tower has been placed on a tile and we want to view its range
    public void clickTower(Tower tower){
            towerClicked = tower;
            sellTowerButton.SetActive(true);
            towerClicked.viewTowerRange();
    }

    //BuyTower() is called whenever we try and SetTower() and it checks if we have enough money to buy the Tower. If we have enough,
    //BuyTower() subtracts our currency by the price of the Tower, hides the hover object, and sets our towerSelected to null.
    public void BuyTower(){
        if(currency >= towerSelected.Price){
            Currency -= towerSelected.Price;
            TowerHover tH = GameObject.FindObjectOfType<TowerHover>();
            tH.hideHover();
            towerSelected = null;
            
        }
    }

   

    //StartRound() is called whenever we click the StartRound Button, and it handles which Round we are currently on for the HUD text. 
    //It also calls our Coroutine SpawnWave() and then disables our roundButton until we are ready for the next wave. 
    public void StartRound(){
        round++;
        roundText.text = "Round: " + round.ToString();
        StartCoroutine(SpawnWave());
        Currency += Mathf.FloorToInt((round * 100) / 10);
        roundButton.SetActive(false);
    }

    //SpawnWave() uses a for loop to randomly select enemies to spawn for our player to attack, then adds them to our objectStorage, and puts them
    // into a list to determine if the enemy is alive. It also tells the enemies the generatedPath they need to follow.  
    private IEnumerator SpawnWave(){
        MapGenerator mG = GameObject.FindObjectOfType<MapGenerator>();
        enemiesThisRound = round * 5;

        for(int i = 0; i < enemiesThisRound; i++){
            doneSpawning = false;
            int enemyIndex = 0;
            if(round >= 5){
                enemyIndex = Random.Range(0,1);                
            }
            if(round >= 10){
                enemyIndex = Random.Range(0,2);
            }
            if(round >= 15){
                enemyIndex = Random.Range(0,3);
            }
            if(round >= 20){
                enemyIndex = Random.Range(0,4);
            }
            string enemyType = string.Empty;
            switch(enemyIndex){
                case 0:
                    enemyType = "BaseEnemy";
                    break;
                case 1:
                    enemyType = "SlowEnemy";
                    break;
                case 2:
                    enemyType = "FastEnemy";
                    break;
                case 3:
                    enemyType = "UltimateEnemy";
                    break;
            }
            Enemies enemy = objStorage.GetObject(enemyType).GetComponent<Enemies>();
            enemy.SpawnEnemy();
            aliveEnemies.Add(enemy);
            yield return new WaitForSeconds(.5f);
        }
        doneSpawning = true;
        mG.GeneratePath();
        
    }

    //deadEnemy() is called whenever we have returned an enemy to our ObjectStorage aka its dead. Therefore, we remove the enemy from our 
    //aliveEnemies list. If our round is over and our game is NOT over we want to make our RoundButton active so we can start another round.
    public void deadEnemy(Enemies enemy){
        aliveEnemies.Remove(enemy);
        if(!roundActive && !gameOver){
            roundButton.SetActive(true);
        }
    }

    //whenever we click on a tower we want the ability to sell the tower for half its cost and then destroy it.
    public void sellTower(){
        if(towerClicked != null){
            Currency += towerClicked.Cost / 2;
            towerClicked.GetComponentInParent<TileManager>().IsEmpty = true;
            Destroy(towerClicked.transform.parent.gameObject);
            sellTowerButton.SetActive(false);
            towerClicked = null;
        }
    }

    //GameOver() checks to see if our game is over (big surprise) and our gameOverScreen is SetActive to see. 
    public void GameOver(){
        if(!gameOver){
            gameOver = true;
            gameOverScreen.SetActive(true);
        }
    }

    //When our game is over and we click the NewGame Button from the gameOverScreen, NewGame() restarts our scene
    public void NewGame(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //QuitGame() quits the application from the gameOverScreen.
    public void QuitGame(){
        Application.Quit();
    }
}
