//MapManager.cs
//Created by Connor Costigan
//Copyright 2021 No Sky Interactive
//All rights reserved
//
//Description:  The main class that handles both tilemap generation and population. 
//              Uses Binary Space Partition (BSP) to generate a map. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    public Tile defaultTile;
    public Tile aTile;
    public Tile bTile;
    List<leaf> mapLeaves = new List<leaf>();
    List<MapBlob> mapBlobs = new List<MapBlob>();
    public bool debug = true;
    public int mapSizeX;
    public int mapSizeY;
    public Tilemap tilemapBase; //This will hold on to the tilemap from Unity
    
    // Start is called before the first frame update
    void Start()
    {
        if (debug == true)
            print("Map Manager initializing...");
        Generate(4, 2, 2, 30, 30);

        print("and now...let's see if we did everything correctly...printing all map leaves!");
        


        foreach (var l in mapLeaves)
        {
            print(l.getName());
            print("   " + l.getStartingPos());
            print("   " + l.getEndingPos());
            print("   " + l.getArea());

        }
    }
    // Update is called once per frame
    public void Update()
    {
        
        tilemapBase.RefreshAllTiles();
    }


    //MAP GENERATOR FUNCTIONS


    //I mean, this one is kinda obvious, but yes, it generates a map.  Note: This function will divide the map up as well, so make sure that each leaf is properrly generated!
    public void Generate(int numOfDivides, int minSizeX, int minSizeY, int maxSizeX, int maxSizeY)
    {
        
        //Ok, so we know that we will need our two starting leaves
        int midPointX = mapSizeX / 2;
        int midPointY = mapSizeY / 2;

        //leaf a and b will always be complementary to the entire map size.
        //The divide function is not used for the two root cases because of this rule
        leaf rootA = new leaf(0, 0, Random.Range(0 + minSizeX, mapSizeX - minSizeX), mapSizeY, "a",0);
        leaf rootB = new leaf((int)rootA.getEndingPos().x, 0, mapSizeX, mapSizeY, "b", 0); 

        mapLeaves.Add(rootA);
        mapLeaves.Add(rootB);

        if (debug)
        {
            print("Generating main root leaves...");
            print("root "+ rootA.getName() +" is " + rootA.getStartingPos() + ", " + rootA.getEndingPos());
            print("root "+ rootB.getName() +" is " + rootB.getStartingPos() + ", " + rootB.getEndingPos());

            
        }

        //Let's divide up the leaves, starting with a and moving on to b
        for (int i = 0; i < numOfDivides; i ++)
        {
            Divide(mapLeaves[i], minSizeX, minSizeY, mapLeaves);
        }
        if (debug)
            Debug.Log("Generation complete! A total of " + mapLeaves[0].getNumOfLeaves() + " leaves have been generated");

        //We are going to fill the map with the default tile
        for (int i = 0; i <= mapSizeX; i++)
        {
            for (int j = 0; j <= mapSizeY; j++)
            {
                //tilemapBase.SetTile(new Vector3Int(i,j,0), defaultTile);
            }
        }

        //Now that we have done that, we are going to set each tile to the leaves
        //set index
        int index = 1;
        
        foreach (var L in mapLeaves)
        {
            Debug.Log("Going through leaf " + L.getName());

            for (int i = L.getStartingPos().x; i <= L.getEndingPos().x; i++)
            {
                for (int j = L.getStartingPos().y; j <= L.getEndingPos().y; j++)
                {
                    if (tilemapBase.GetTile(new Vector3Int(i,j,0)) != defaultTile)
                        tilemapBase.SetTile(new Vector3Int(i,j,0), (index % 2 == 0 ? aTile : bTile));

                }
            }
            tilemapBase.SetTile(new Vector3Int(L.getStartingPos().x, L.getStartingPos().y,0),defaultTile);
            tilemapBase.SetTile(new Vector3Int(L.getEndingPos().x, L.getStartingPos().y,0),defaultTile);
            tilemapBase.SetTile(new Vector3Int(L.getStartingPos().x, L.getEndingPos().y,0),defaultTile);
            tilemapBase.SetTile(new Vector3Int(L.getEndingPos().x, L.getEndingPos().y,0),defaultTile);
            index++;
        }
        
    }


    //Divide is what is called when a leaf will split into two leaves 
    public bool Divide(leaf parent, int minSizeX, int minSizeY, List<leaf> leafList)
    {
        //These two variables are important because they hold the old values of the parent
        Vector2 parentStart = parent.getStartingPos();
        Vector2 parentEnd = parent.getEndingPos();

        //let's make sure we are able to divide
        if (parent.getArea()/2 < minSizeX * minSizeY)
        {
            if (debug)
            {
                print("Error dividing " + parent.getName() + ": area is not large enough to divide :(");
            }
            return false;
        }
        else //we are ok
        {
            //let's roll the dice to see if we will divide horizontally or vertically
            int randDirection = Random.Range(1,6);
            

            //Once we know that, we will calculate the divide and create a new leaf to fill the leftover area.

            if (randDirection > 3) //vertical slice
            {
                //Now we will randomly slice somewhere along the x axis
                int slice = Random.Range((int)parent.getStartingPos().x + minSizeX, (int)parent.getEndingPos().x - minSizeX);
                //Now we will modify the parent area
                parent.setArea((int)parent.getStartingPos().x, (int)parent.getStartingPos().y, slice, (int)parent.getEndingPos().y) ;

                //now let's create the new leaf, also assigning its parent
                leaf newLeaf = new leaf(slice, (int)parent.getStartingPos().y, (int)parentEnd.x, (int)parentEnd.y, parent);


                if (debug)
                {
                    print("divided into a new leaf (vertically)" + newLeaf.getName() + " from " + parent.getName());
                }

                mapLeaves.Add(newLeaf); //Add the new leaf to the mapLeaves list
            
            }
            else //horizontal slice. Practically the same as above, except we are slicing horizontally 
            {
                //Let's find our slice.
                int slice = Random.Range((int)parent.getStartingPos().y + minSizeY, (int)parent.getEndingPos().y - minSizeY);
                //Now set the area of the original parent
                parent.setArea((int)parent.getStartingPos().x, (int)parent.getStartingPos().y, (int)parent.getEndingPos().x, slice);
                //create new leaf
                leaf newLeaf = new leaf((int)parent.getStartingPos().x, slice, (int)parentEnd.x, (int)parentEnd.y, parent);

                if (debug)
                {
                    print("divided into a new leaf (horizontally)" + newLeaf.getName() + " from " + parent.getName());
                }

                mapLeaves.Add(newLeaf);
            }
        }

        return true; //If we made it here, we made it through all that! Phew!

    }
    
    
    
}




//Leaf is a class that is defined by an area within the map. It is a part of the BSP Algorithm used to generate a map
public class leaf
{
    private int startX, startY, endX, endY;
    private static int numOfLeaves = 0; //Used to keep track of how many leaaves are generated
    private string name;
    int index; //Will be used as part of name.  This is simply the integer part of the name.  For example, a1 comes from a0
    private leaf parent;
    private List<leaf> children = new List<leaf>();

    
    //This constuctor is used to create a named leaf, with a starting index (usually roots)
    public leaf(int x, int y, int endX, int endY, string n, int index)
    {
        startX = x;
        startY = y;
        this.endX = endX;
        this.endY = endY;
        numOfLeaves++; //Add to the number every time
        this.index = index;
        setName(n, index);
        
    }
    //This constructor creates a leaf from a parent (it will automatically name and set the correct parameters for the leaf)
    public leaf(int x, int y, int endX, int endY, leaf p)
    {
        startX = x;
        startY = y;
        this.endX = endX;
        this.endY = endY;
        numOfLeaves++; //Add to the number every time
        setParent(p);
        p.addChild(this);
        setName(p); //We will take our name from our parent (+1 on index)
    }

    public void setName(string n)
    {
        name = n;
    }
    //This overload of setName is used to take a name from the parent. Use this for children
    public void setName(leaf parent)
    {
        index = parent.getIndex() + 1;
        name = ("" + parent.getName()[0] + index); 
    }
    public void setName(string n, int i)
    {
        name = n + i;
        index = i;
    }
    public string getName()
    {
        return "" + name;
    }
    public int getIndex()
    {
        return index;
    }
    public void setParent(leaf p)
    {
        parent = p;
    }
    public leaf getParent()
    {
        return parent;
    }

    public List<leaf> getChildren()
    {
        return children;
    }

    public void setArea(int x, int y, int endX, int endY)
    {
        startX = x;
        startY = y;
        this.endX = endX;
        this.endY = endY;
    }

    public void addChild(leaf c)
    {
        children.Add(c);
    }

    public int getNumOfLeaves()
    {
        return numOfLeaves;
    }

    public Vector2Int getStartingPos()
    {
        return new Vector2Int(startX, startY);
    }
    
    public Vector2Int getEndingPos()
    {
        return new Vector2Int(endX, endY);
    }

    public int getArea()
    {
        return (endX - startX) * (endY - startY);
    }

    //Simple function that checks if map point x, y is found within the leaf NOTE: may need to make this non local
    public bool isInBounds(int x, int y)
    {
        
        if ((x >= startX && x <= endX) && (y >= startY && y <= endY))
        {
            return true;
        }
        else
            return false;
    }



    
}