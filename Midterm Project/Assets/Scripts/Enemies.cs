﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{   
    [SerializeField] private float enemySpeed;
    [SerializeField] private int enemyHealth;
    [SerializeField] private int playerReward;
    [SerializeField] private int damage;
    public bool IsActive{ get; set; }
    private Stack<Node> path;
    public Point GridPosition { get; set; }
    private Vector3 destination;
    private int pointIndex = 0;
    private GameObject targetTile;
    public bool IsAlive{
        get{
            return enemyHealth > 0;
        }
    }

    public int EnemyHealth{
        get{
            return enemyHealth;
        }
    }
    
    //Whenever our enemy is hit by a projectile it needs to takeDamage from the bullet and takeDamage decreases our enemy health by the given amount
    // if the enemyHealth is ever <= 0 we can call the die() method. 

    public void takeDamage(int damage){
        enemyHealth -= damage;
        if(enemyHealth <= 0){
            die();
        }
    }

    //die() sounds morbid but whenever our enemyHealth has reached 0 we must remove it from our EnemyList and destroy the gameObject
    private void die(){
        GameManager gM = GameObject.FindObjectOfType<GameManager>();
        IsActive = false;
        gM.Currency += playerReward;
        gM.enemiesThisRound--;
        returnEnemy();
    }

    private void Update(){
        Move();
    }  

    //whenever we spawn an enemy we must set its transform to the startBase, and we must tell the enemy the path it needs to take along the map.
    public void SpawnEnemy(){
        MapGenerator mG = GameObject.FindObjectOfType<MapGenerator>();
        GameManager gM = GameObject.FindObjectOfType<GameManager>();
        this.transform.position = mG.BaseStart.transform.position;
        IsActive = true;
        SetPath(mG.MapPath);
    }

    //Move() handles the enemy movement along the path by checking the enemy position is at the destination.
    //Then if our path isn't null and we still have tiles in our path left we must peek at our next position and pop our new destination. 
    private void Move(){
        if(IsActive){
            transform.position = Vector2.MoveTowards(transform.position, destination, enemySpeed * Time.deltaTime);
            if(transform.position == destination){
                if(path != null && path.Count > 0){
                   GridPosition = path.Peek().GridPos;
                   destination = path.Pop().WorldPos;
                }
            }
        }
    }

    //SetPath() takes a stack of Nodes (or tiles references) given from our pathfinding algorithm and sets the path the enemies need to take
    private void SetPath(Stack<Node> newPath){
        this.path = newPath;
        GridPosition = path.Peek().GridPos;
        destination = path.Pop().WorldPos;
    }

    //Whenever our enemy collides with our EndBase we must returEnemy() because it has reached the end of the path,
    // and we should decrease our player health.
    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "EndBase"){
            returnEnemy();
            GameManager gM = GameObject.FindObjectOfType<GameManager>();
            gM.Health -= damage;
        }
    }

    //returnEnemy() puts the given enemy into storage by calling ReturnToStorage() in our ObjectStorage and then tells the gameManager that its a deadEnemy()
    private void returnEnemy(){
        GameManager gM = GameObject.FindObjectOfType<GameManager>();
        gM.deadEnemy(this);
        gM.objStorage.ReturnToStorage(this.gameObject);
    }
}
