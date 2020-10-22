using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public static float health;

    [SerializeField] private float healthRate;
    void Start(){
        health = 100;
    }
    void Update()
    {
       healthDecreaser();
    }

    private void healthDecreaser(){
       if(health <= 100){
            health -= healthRate * Time.deltaTime;
        }
        if(health <= 0){
            health = 0;
            //call game over
        }
    }
}
