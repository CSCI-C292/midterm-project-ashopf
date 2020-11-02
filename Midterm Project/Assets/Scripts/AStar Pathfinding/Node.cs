using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Point GridPos{ get; set;}

    public Vector2 WorldPos{ get; set; }

    public TileManager TileReference { get; private set;}

    public Node Parent { get; private set;}

    public int gScore {get; set;}

    public int hScore {get; set;}

    public int fScore {get; set;}

    //Node is a tileReference to our tiles in our TileManger, it takes the same object, gridPosition, and centerPosition as the tile it references
    public Node(TileManager tileRef){
        this.TileReference = tileRef;
        this.GridPos = tileRef.GridPosition;
        this.WorldPos = tileRef.centerPosition;
    }

    //SetValues() tells our AStar algorithm what the nodes parents are, the destination, and the current gCost for the next Node
    public void setValues(Node parent, Node goal, int gCost){
        this.Parent = parent;
        this.gScore = parent.gScore + gCost;
        this.hScore = (Mathf.Abs(GridPos.X - goal.GridPos.X) + Mathf.Abs(goal.GridPos.Y - GridPos.Y)) * 10;
        this.fScore = gScore + hScore;
    }
}
