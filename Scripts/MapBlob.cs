//MapBlob.cs
//Created by Connor Costigan
//Copyright 2021 No Sky Interactive
//All rights reserved
//
//Description:  The main class for blobs, which are the authored pieces of interesting map parts that are inserted into leaves

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps; //We gonna need this...

public class MapBlob
{
    const int MAX_SIZE = 800;

    int[] mapBlob = new int[MAX_SIZE]; //This will hold the blob data to transfer onto the leaf

    int x, y;
    int minLevel, maxLevel; //This is used to determine the minimum level for the map
    int weight; //Used for balancing purpose, EXPERIMENTAL

    public MapBlob(int rows, int col)
    {
        x = rows;
        y = col;
    }
    //Function that returns the area of a blob
    public int getArea()
    {
        return x * y;
    }
}
