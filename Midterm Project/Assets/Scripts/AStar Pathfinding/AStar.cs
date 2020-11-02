using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//A* is the pathfinding algorithm I used for my enemies
public static class AStar
{
    private static Dictionary<Point, Node> nodeDictionary;

    //BuildNodes() creates a dictionary of Nodes(tile references) and copies each tile and tile position from our tileDictionary into our nodeDictionary.
    private static void BuildNodes(){
        nodeDictionary = new Dictionary<Point, Node>();
        MapGenerator mG = GameObject.FindObjectOfType<MapGenerator>();
        foreach(TileManager tile in mG.TileDictionary.Values){
            nodeDictionary.Add(tile.GridPosition, new Node(tile));
        }
    }

    //GetPath is where we determine the best path for the enemies to get to their destination.
    public static Stack<Node> GetPath(Point start, Point goal){

        if(nodeDictionary == null){
            BuildNodes();
        }

        HashSet<Node> openList = new HashSet<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        Stack<Node> finalPath = new Stack<Node>();

        Node currentNode = nodeDictionary[start];

        openList.Add(currentNode);

        MapGenerator mG = GameObject.FindObjectOfType<MapGenerator>();

        //while we still have a currentNode in our openList
        while(openList.Count > 0){
            //we check the x positions to the left and right of our currentNode
            for(int x=-1; x<=1; x++){
                //we check the y positions above and below our current Node
                for(int y=-1; y<=1; y++){

                    //The positions of our nodes above, below, left, and right depending on where we are in the for loop
                    Point neighborPos = new Point(currentNode.GridPos.X - x, currentNode.GridPos.Y - y);

                    //check if our nodeDictionary has a keyValue for our neighbor(is the neighbor tile !null), is our neighbor ableToWalk?, and is our neighborPos not our currentNode Pos.
                    if(nodeDictionary.ContainsKey(neighborPos) && mG.TileDictionary[neighborPos].ableToWalk && neighborPos != currentNode.GridPos){
                        int gCost = 0;
                        //our gCost to go DIRECTLY up, down, left, or right, only costs our enemy 10
                        if(Mathf.Abs(x-y) == 1){
                            gCost = 10;

                        //else our gCost is more expensive (14) if we travel Diagonally in any direction
                        }else{
                            gCost = 14;
                        }

                        Node neighborNode = nodeDictionary[neighborPos];

                        //if our openList has a neighborNode and our currentNodes gScore + the gCost to go the neighborNode is less than our neighborNodes gScore
                        //we want to set our neighborNodes values where the currentNode is our neighborNode's parent, and give it our desitination, and update our gCost
                        if(openList.Contains(neighborNode)){
                            if(currentNode.gScore + gCost < neighborNode.gScore){
                                neighborNode.setValues(currentNode, nodeDictionary[goal], gCost);
                            }
                            //else if our closedList doesn't contain the neighbordNode we want to add it to our openList and set the values as mentioned above.
                        }else if(!closedList.Contains(neighborNode)){
                            openList.Add(neighborNode);
                            neighborNode.setValues(currentNode, nodeDictionary[goal], gCost); 
                        }
                    }
                }
            }
            //we are done checking the neighbors of our currentNode now it is time to take it out of our openList and into our closedList
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //If we still have nodes in our openList we want to set our currentNode to the node in the openList with the best fScore
            if(openList.Count > 0){
                currentNode = openList.OrderBy(n => n.fScore).First();
            }

            //if our currentNode is our destination/goal we want to go through our finalPath backwards and set each currentNode
            // as the next nodes parent until we reach the start position. 
            if(currentNode == nodeDictionary[goal]){
                while(currentNode.GridPos != start){
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                break;
            }
        }
        return finalPath;
    }
}
