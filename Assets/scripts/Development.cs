using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Development : MonoBehaviour
{
    GameObject rocket;
    GameObject fuel;
    GameObject obstacle;
    GameObject ground;
    GameObject launchPad;
    GameObject landingPad;

    bool collisionDisabled = false;


    void Start()
    {
        rocket = GameObject.Find("Rocket");
        fuel = GameObject.Find("Fuel");
        obstacle = GameObject.Find("Obstacle");
        ground = GameObject.Find("Ground");
        launchPad = GameObject.Find("LaunchPad");
        landingPad = GameObject.Find("LandingPad");
    }

    void Update()
    {
        CheckKey();
    }

    void CheckKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            switch (collisionDisabled)
            {
                case false:
                    collisionDisabled = true;
                    DisableCollisions();
                    break;
                default:
                    collisionDisabled = false;
                    EnableCollisions();
                    break;
            }
        }
    }

    private void LoadNextLevel()
    {
        rocket.GetComponent<CollisionHandler>().Next();
    }

    private void ReloadLevel()
    {
        rocket.GetComponent<CollisionHandler>().Reload();
    }

    private void DisableCollisions()
    {
        //disables collisions
        Debug.Log("Collisions disabled");
        rocket.GetComponent<CollisionHandler>().collisionEnabled = false;
    }

    private void EnableCollisions()
    {
        //enables collisions
        Debug.Log("Collisions enabled");
        rocket.GetComponent<CollisionHandler>().collisionEnabled = true;
    }
}
