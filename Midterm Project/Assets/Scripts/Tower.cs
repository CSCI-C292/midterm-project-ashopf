using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Enemies targetedEnemy;
    public Enemies TargetedEnemy{
        get{
            return targetedEnemy;
        }
    }
    private Queue<Enemies> enemies = new Queue<Enemies>();
    private bool ableToAttack = true;
    private float attackTimer;
    [SerializeField] private float fireRate;
    [SerializeField] private string bulletType;
    [SerializeField] private float bulletSpeed;
    public float BulletSpeed{
        get{
            return bulletSpeed;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
     this.spriteRenderer = GetComponent<SpriteRenderer>();   
     ableToAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        attack();
        Debug.Log(targetedEnemy);
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
            targetedEnemy = enemies.Dequeue();
        }
        if(targetedEnemy != null){
            if(ableToAttack){
                fire();
                ableToAttack = false;
            }
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
            enemies.Enqueue(other.GetComponent<Enemies>());
        }
    }

    //Whenever something leaves our range collider we want to check if was an enemy and then set our targeted enemy back to null
    public void OnTriggerExit2D(Collider2D other){
        if(other.tag == "Enemy"){
            targetedEnemy = null;
        }
    }
}
