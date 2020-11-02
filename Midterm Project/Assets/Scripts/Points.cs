using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point 
{
    public int X {get; set;}
    public int Y {get; set;}

    //Points are simply positions on a grid and this is the struct class to build them. 
    public Point(int x, int y){
        this.X = x;
        this.Y = y;
    }

    public static bool operator ==(Point first, Point second){
        return first.X == second.X && first.Y == second.Y;
    }
    public static bool operator !=(Point first, Point second){
        return first.X != second.X || first.Y != second.Y;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
