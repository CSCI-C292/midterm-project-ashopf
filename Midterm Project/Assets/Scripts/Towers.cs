using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Towers : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float fireRate;
    private float timeToFire;

    private GameObject currentTarget;

    private void Start(){
        timeToFire = Time.time;
    }
    private void updateNearestEnemy(){
        GameObject currentEnemy = null;
        float distance = Mathf.Infinity;
        foreach(GameObject enemy in EnemyList.enemies){
            if(enemy != null){
                float updatedDistance = (transform.position - enemy.transform.position).magnitude;
                if(updatedDistance < distance){
                   distance = updatedDistance;
                   currentEnemy = enemy;
                }
            }
        }
        if(distance <= range){
            currentTarget = currentEnemy;
        }else{
            currentTarget = null;
        }
    }

    private void fire(){
        Enemies enemiesScript = currentTarget.GetComponent<Enemies>();
        enemiesScript.takeDamage(damage);
    }
    private void Update(){
        updateNearestEnemy();
        if(Time.time >= timeToFire){
            if(currentTarget != null){
                fire();
                timeToFire = Time.time + fireRate;
            }
        }
    }
}
