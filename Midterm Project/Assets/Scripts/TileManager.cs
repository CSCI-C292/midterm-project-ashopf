using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileManager : MonoBehaviour
{
    public Point GridPosition{ get; private set; }
    public bool IsEmpty{ get; set; }
    [SerializeField] private Color tileTakenColor;
    [SerializeField] private Color tileAvailableColor;
    public bool ableToWalk {get; set;}
    private SpriteRenderer spriteRenderer;
    private Tower towerOnTile;
    public Vector2 centerPosition{
        get{
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y /2));
        }
    }

    private void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //Setup() is used to determine whether the tile at the given gridPos is able for our enemies to walk on, if it is empty, and then sets the position
    public void Setup(Point gridPos, Vector3 camPos, Transform parent)
    {
        ableToWalk = true;
        this.IsEmpty = true;
        this.GridPosition = gridPos;
        transform.position = camPos;
        transform.SetParent(parent);
    }

    //Whenever our mose is over a gameObject(a tile) and we have a towerSelected from the shop, we want to change the tile color to red or green
    //depending on if the tile is empty or if it is a path tile. We can also clickTower() and see the current range of our tower.
    private void OnMouseOver(){
        GameManager gM = GameObject.FindObjectOfType<GameManager>();
        MapGenerator mG = GameObject.FindObjectOfType<MapGenerator>();
        if(!EventSystem.current.IsPointerOverGameObject() && gM.towerSelected != null){
            if(IsEmpty){
                changeTileColor(tileAvailableColor);
            }
            if(!IsEmpty && this.tag == "Path"){
                changeTileColor(tileTakenColor);
            }
            else if(Input.GetMouseButtonDown(0)){
            SetTower();
            }
        }
        else if(!EventSystem.current.IsPointerOverGameObject() && gM.towerSelected == null && Input.GetMouseButtonDown(0)){
            if(towerOnTile != null){
                gM.clickTower(towerOnTile);
            }
        }
    }

    //whenever our mouse is not over a gameObject we want to make sure the tile changes back to its original color
    private void OnMouseExit(){
        changeTileColor(Color.white);
    }

    //SetTower() is the function that places the Tower onto the desired tile on our map.
    private void SetTower(){
        GameManager gM = GameObject.FindObjectOfType<GameManager>();
        GameObject gO = (GameObject)Instantiate(gM.towerSelected.TowerPrefab, transform.position, Quaternion.identity);
        gO.transform.SetParent(transform);
        this.towerOnTile = gO.transform.GetChild(0).GetComponent<Tower>();
        IsEmpty = false;
        gM.BuyTower();
        ableToWalk = false;
        
    }

    //simple method to change the spriteRenderer's (tile) color to any given shade. 
    private void changeTileColor(Color color){
        spriteRenderer.color = color;
    }
}
