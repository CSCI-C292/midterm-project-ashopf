using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemies targetedEnemy;
    private Tower parentTower;
    void Update()
    {
        SeekAndDestroy();
        ReturnAllBullets();
    }

    //Initialize tells the bullet who is their parent Tower and tells the bullet who the targetedEnemy is for the parent tower. 
    public void Initialize(Tower parent){
        this.parentTower = parent;
        this.targetedEnemy = parent.TargetedEnemy;
    }

    //SeekAndDestroy() checks if we currently have a targeted enemy and if we do we want to move towards that enemy,
    //but if we do not have a targetedEnemy we simply put our bullet back into storage
    private void SeekAndDestroy(){
        GameManager gM = GameObject.FindObjectOfType<GameManager>();
        if(targetedEnemy != null){
            transform.position = Vector3.MoveTowards(transform.position, targetedEnemy.transform.position,Time.deltaTime * parentTower.BulletSpeed);
            Vector2 direction = targetedEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
        }
    }

    //Whenever the bullet collides with an enemy we damage the enemy and return our bullet to storage
    private void OnTriggerEnter2D(Collider2D other){
        GameManager gM = GameObject.FindObjectOfType<GameManager>();
        if(other.tag == "Enemy"){
            if(targetedEnemy.gameObject == other.gameObject){
                targetedEnemy.takeDamage(parentTower.Damage);
                gM.objStorage.ReturnToStorage(gameObject);
            }
        }
    }

    //ReturnAllBullets() is designed to check if there are any bullets that got left on the player screen.
    //If there are any stragglers it sends them to the storage.
    private void ReturnAllBullets(){
        GameManager gM = GameObject.FindObjectOfType<GameManager>();
        if(!gM.roundActive){
            gM.objStorage.ReturnToStorage(gameObject);
        }
        if(parentTower.TargetedEnemy != null){
            if(!parentTower.TargetedEnemy.IsActive || !parentTower.TargetedEnemy.IsAlive){
               gM.objStorage.ReturnToStorage(gameObject);
            }
        }
        
    }
}
