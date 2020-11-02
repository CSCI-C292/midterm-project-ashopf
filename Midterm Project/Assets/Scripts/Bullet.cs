using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Enemies targetedEnemy;
    private Tower parentTower;
    // Update is called once per frame
    void Update()
    {
        SeekAndDestroy();
    }

    //Initialize tells the bullet who is their parent Tower and tells the bullet who the targetedEnemy is for the parent tower. 
    public void Initialize(Tower parent){
        this.parentTower = parent;
        this.targetedEnemy = parent.TargetedEnemy;
    }

    //SeekAndDestroy() checks if we currently have a targeted enemy and if we do we want to move towards that enemy,
    //but if we do not have a targetedEnemy we simply put our bullet back into storage
    private void SeekAndDestroy(){
        if(targetedEnemy != null){
            transform.position = Vector3.MoveTowards(transform.position, targetedEnemy.transform.position,Time.deltaTime * parentTower.BulletSpeed);
        }
        else if(targetedEnemy == null){
            GameManager gM = GameObject.FindObjectOfType<GameManager>();
            gM.objStorage.ReturnToStorage(gameObject);
        }
    }
}
