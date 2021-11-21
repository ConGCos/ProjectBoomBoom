//Star Seeker - PlayerController.cs
//Created by Connor Costigan
//Copyright 2021 No Sky Interactive. All rights reserved
//
//Description - Player Controller used to...control the player and handle collision, interact, etc.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    float mouseX, mouseY;

    private Vector3 moveDelta;

    //Collider
    BoxCollider2D boxCollider;

    
    //Code for the game manager
    GameManager gameManager;
    GameObject manager;
    
    
    //Boolean for facing direction
    bool facingRight = false;

    public float playerSpeed;


    //Collision
    private RaycastHit2D hit;
    private Transform collisionLocation;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        manager = GameObject.Find("GameManager");
        gameManager = manager.GetComponent<GameManager>();
        collisionLocation = GetComponentInChildren<Transform>();
        
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Let's get the mouse input and convert it to World coordinates
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        //Now let's get input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //float speed;


        //Consider removing this...
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;

        //Reset the moveDelta every frame
        moveDelta = new Vector3(x * playerSpeed, y * playerSpeed, 0);

        //Make sure we aren't paused or in dialogue to handle movement
        if (gameManager.gameState == GameManager.gameStates.normal)
        {



            //Let's determine which way to face
            if (worldPosition.x > transform.position.x)
            {
                facingRight = true;
            }
            else
                facingRight = false;
            if (!facingRight)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else
            {
                transform.localScale = new Vector2(1, 1);
            }

            //Handle movement

            if ((moveDelta.x > 0 || moveDelta.x < 0) && (moveDelta.y > 0 || moveDelta.y < 0))
                transform.Translate(moveDelta * (playerSpeed/2));
            else
                transform.Translate(moveDelta * (playerSpeed));

        }
    }
}
