using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] tiles;

    private Point startPoint, endPoint;
    [SerializeField] private GameObject startBase; 
    [SerializeField] private GameObject endBase;
    
    [SerializeField] private Transform map; 
    public Dictionary<Point, TileManager> TileDictionary { get; set; }

    public BaseSpawn BaseStart {get; set;}
    private Point mapSize;

    private Stack<Node> mapPath;

    public Stack<Node> MapPath{
        get{
            if(mapPath == null){
                GeneratePath();
            }
            return new Stack<Node>(new Stack<Node>(mapPath));
        }
    }

    public bool walkable {get; set;}

    public float tileSize{
        get { 
            return tiles[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
            }
    }


    private void Start(){
        walkable = GetComponent<TileManager>();
        generateMap();
    }

    //generates the map by placing the tiles placing tiles row by row
    private void generateMap(){

        TileDictionary = new Dictionary<Point, TileManager>();

        string[] mapData = new string[]{
            "000100000000000", "000100000111110", "000111110100010", "000000010100010",
             "000000010100010", "011111110100010", "010000000100010", "010000000100010", 
             "011111111100010", "000000000000010", "011111111111110", "010000000000000",
             "010000000000000", "011111111111110", "000000000000010"
        };

        int mapWidth = mapData[0].ToCharArray().Length;
        int mapHeight = mapData.Length;
        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        Vector3 startingCamPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for(int y=0; y < mapWidth; y++){
            char[] currentRowTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapHeight; x++){
                placeTile(currentRowTiles[x].ToString(), x, y, startingCamPos);
            }   
        }
        StartEndPoint();
    }

    //takes a string (our tiles) and the x,y position it needs to be placed with the camera's position as reference to place 1 tile.
    //placeTile is used in generateMap()
    private void placeTile(string tileType, int x, int y, Vector3 camPos){
        int tileIndex = int.Parse(tileType);
        TileManager nextTile = Instantiate(tiles[tileIndex]).GetComponent<TileManager>();
        TileDictionary.Add(new Point(x, y), nextTile);
        nextTile.Setup(new Point(x, y), new Vector3(camPos.x + (tileSize * x), camPos.y - (tileSize * y), 0), map);
        if(tileType == "0"){
            nextTile.ableToWalk = false;
        }
        if(tileType == "1"){
            nextTile.IsEmpty = false;
        }
    }

    //StartEndPoint() is where we set the position of our startBase and endBase
    private void StartEndPoint(){
        startPoint = new Point(3, 0);
        GameObject tmpGameObject = (GameObject)Instantiate(startBase, TileDictionary[startPoint].GetComponent<TileManager>().centerPosition, Quaternion.identity);
        BaseStart = tmpGameObject.GetComponent<BaseSpawn>();
        BaseStart.name = "StartBase";

        endPoint = new Point(13, 14);
        Instantiate(endBase, TileDictionary[endPoint].GetComponent<TileManager>().centerPosition, Quaternion.identity);
    }

    //GeneratePath() is the pathwalking algorithm used to get our enemy players from our startBase to our endBase.
    public void GeneratePath(){
        mapPath = AStar.GetPath(startPoint, endPoint);
    }

}
