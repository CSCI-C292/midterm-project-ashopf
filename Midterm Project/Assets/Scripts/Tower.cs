using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Enemies targetedEnemy;
    private List<Enemies> enemies = new List<Enemies>();
    private bool ableToAttack = true;
    private float attackTimer;
    public int Cost{get; set;}
    [SerializeField] private float fireRate;
    [SerializeField] private string bulletType;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int damage;
    public Enemies TargetedEnemy{
        get{
            return targetedEnemy;
        }
    }
    public int Damage{
        get{
            return damage;
        }
    }
    public float BulletSpeed{
        get{
            return bulletSpeed;
        }
    }

    // Start is called before the first frame update
    void Start(){
    this.spriteRenderer = GetComponent<SpriteRenderer>();   
    ableToAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        attack();
    }

    //viewTowerRange() just sets the spriteRenderer for the tower's range active
    public void viewTowerRange(){
        spriteRenderer.enabled = !spriteRenderer.enabled;
    }

    //attack() basically determines whether or not we should attack an enemy. If we are able to attack then we check if we have a targetedEnemy.
    //If we do have a targeted enemy and are able to attack we call the fire() method and set our ableToAttack back to false. But if we do NOT have 
    //an targetedEnemy and we know we still have enemies remaining we want to take them out of our enemiesQueue
    private void attack(){
        if(!ableToAttack){
            attackTimer += Time.deltaTime;
            if(attackTimer >= fireRate){
                ableToAttack = true;
                attackTimer = 0;
            }
        }
        if(targetedEnemy == null && enemies.Count > 0){
            enemies.Remove(targetedEnemy);
            targetedEnemy = enemies[0];
        }
        if(targetedEnemy != null && targetedEnemy.IsActive){
            if(ableToAttack){
                fire();
                ableToAttack = false;
            }
        }
        else if(enemies.Count > 0){
            enemies.Remove(targetedEnemy);
            targetedEnemy = enemies[0];
        }
        if(targetedEnemy != null && !targetedEnemy.IsAlive || targetedEnemy != null && !targetedEnemy.IsActive){
            enemies.Remove(targetedEnemy);
        }
    }

    //fire() gets a bullet from storagte and sets the bullets position = to the position of the current tower and then Initializes that bullet.
    private void fire(){
        GameManager gM = GameObject.FindObjectOfType<GameManager>();
        Bullet bullet = gM.objStorage.GetObject(bulletType).GetComponent<Bullet>();
        bullet.transform.position = transform.position;
        bullet.Initialize(this);
    }

    //Whenever something collides with our range we check if it is an Enemy and then add it to our enemies Queue for all of our targeted enemies
    public void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Enemy"){
            enemies.Add(other.GetComponent<Enemies>());
            if(enemies[0] != null){
                targetedEnemy = enemies[0];
            }else if(enemies[0] == null){
                for(int i = 0; i < enemies.Count; i++){
                    if(enemies[i] != null){
                        targetedEnemy = enemies[i];
                    }
                }
            }
        }
    }

    //Whenever something leaves our range collider we want to check if was an enemy and then set our targeted enemy back to null
    public void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Enemy"){
            enemies.Remove(targetedEnemy);
        }
    }
}
