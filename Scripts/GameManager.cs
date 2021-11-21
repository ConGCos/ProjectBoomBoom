//Star Seeker - GameManager.cs
//Created by Connor Costigan
//Copyright 2021 No Sky Interactive. All rights reserved
//
//Description - This manages game state, calls functions such as save, imports data, loads the map, etc.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Game State Enums
    
    public enum gameStates
    {
        normal,
        combat, //We might not need this one...combat can be handled as normal is my guess but hey, why not?
        dialogue,
        menu,

    }


    public gameStates gameState = gameStates.normal; //use gameState to get the state from other components

    //This function will return the gameState
    public gameStates getState()
    {
        return gameState;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
