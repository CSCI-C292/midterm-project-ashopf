﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject MapTile;
    public GameObject PathTile;

    [SerializeField] private int mapWidth;
    [SerializeField] private int mapHeight;
    
    public static List<GameObject> mapTiles = new List<GameObject>();
    public static List<GameObject> pathTiles = new List<GameObject>();

    public static GameObject startTile;
    public static GameObject endTile;
    private bool reachedX = false;
    private bool reachedY = false;
    private GameObject currentTile;
    private int currentIndex;
    private int nextIndex;


    private void Start(){
        generateMap();
    }
    private List<GameObject> getTopEdgeTiles(){
        List<GameObject> edgeTiles = new List<GameObject>();

        for(int i= mapWidth * (mapHeight - 1); i<mapWidth * mapHeight; i++){
            edgeTiles.Add(mapTiles[i]);
        }
        return edgeTiles;
    }
    private List<GameObject> getBottomEdgeTiles(){
        List<GameObject> edgeTiles = new List<GameObject>();
        for (int i = 0; i<mapWidth; i++){
            edgeTiles.Add(mapTiles[i]);
        }
        return edgeTiles;
    }

    private void moveDown(){
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex - mapWidth;
        currentTile = mapTiles[nextIndex];
    }
    private void moveLeft(){
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex-1;
        currentTile = mapTiles[nextIndex];
    }
    
    private void moveRight(){
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex+1;
        currentTile = mapTiles[nextIndex];
    }
    private void generateMap(){
        for(int y=0; y<mapHeight; y++){
            for(int x=0; x<mapWidth; x++){
                GameObject newTile = Instantiate(MapTile);
                mapTiles.Add(newTile);
                newTile.transform.position = new Vector2(x, y);
            }
        }

        List<GameObject> topEdgeTiles = getTopEdgeTiles();
        List<GameObject> bottomEdgeTiles = getBottomEdgeTiles();

        startTile = topEdgeTiles[3];
        endTile = bottomEdgeTiles[6];
        currentTile = startTile;
        moveDown();
        moveLeft();
        moveLeft();
        moveDown();
        moveDown();
        moveDown();
        moveRight();
        moveRight();
        moveRight();
        moveRight();
        moveRight();
        moveDown();
        moveDown();
        moveDown();
        pathTiles.Add(endTile);
        
        foreach(GameObject obj in pathTiles){
            GameObject path= PathTile;
            obj.GetComponent<SpriteRenderer>().sprite = path.GetComponent<SpriteRenderer>().sprite;
        }
    }
}