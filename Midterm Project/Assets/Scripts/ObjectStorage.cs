using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStorage : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPrefabs;
    private List<GameObject> objectList = new List<GameObject>();

    //GetObject(string type) returns a gameObject depending on the string given. For ex. we can ask for a "Bullet" or a "BaseEnemy" from our
    //and we will iterate through our objectList to see if we have the desired object. 
    public GameObject GetObject(string type){

        foreach(GameObject gameObj in objectList){
            if(gameObj.name == type && !gameObj.activeInHierarchy){
                gameObj.SetActive(true);
                return gameObj;
            }
        }

        for(int i=0; i<objectPrefabs.Length; i++){

            if(objectPrefabs[i].name == type){
                GameObject newObject = Instantiate(objectPrefabs[i]);
                objectList.Add(newObject);
                newObject.name = type;
                return newObject;
            }
        }
        return null;
    }

    //ReturnToStorage() just sets the gameObject inactive. 
    public void ReturnToStorage(GameObject gameObject){
        gameObject.SetActive(false);
    }
}
