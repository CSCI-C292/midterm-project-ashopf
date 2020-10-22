using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField] private float enemyHealth;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float playerReward;
    [SerializeField] private int damage;
    private GameObject targetTile;

    private void Awake(){
        EnemyList.enemies.Add(gameObject);
    }

    private void Start(){
        initializeEnemy();
    }
    private void initializeEnemy(){
        targetTile = MapGenerator.startTile;

    }
    private void moveEnemy(){
        transform.position = Vector3.MoveTowards(transform.position, targetTile.transform.position, movementSpeed * Time.deltaTime);
    }

    public void takeDamage(float amount){
        enemyHealth -= amount;
        if(enemyHealth <= 0){
            die();
        }
    }

    private void die(){
        EnemyList.enemies.Remove(gameObject);
        Destroy(transform.gameObject);
    }

    private void checkPosition(){
        if(targetTile != null && targetTile != MapGenerator.endTile){
            float distance = (transform.position - targetTile.transform.position).magnitude;

            if(distance < 0.001f){
                int currentIndex = MapGenerator.pathTiles.IndexOf(targetTile);
                targetTile = MapGenerator.pathTiles[currentIndex + 1];
            }
        }
    }

    private void update(){
        checkPosition();
        moveEnemy();

    }
}
