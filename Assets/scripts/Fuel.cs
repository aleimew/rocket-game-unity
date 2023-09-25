using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] float fuelAmount = 100f;

    GameObject rocket;

    // Start is called before the first frame update
    void Start()
    {
        rocket = GameObject.Find("Rocket");

    }

    // Update is called once per frame
    void Update()
    {
        IsRocketMoving();
        CheckFuel();
    }

    void IsRocketMoving()
    {
        if (Input.GetKey(KeyCode.Space) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.D))
        {
            fuelAmount -= Time.deltaTime;

        }
    }

    void CheckFuel()
    {
        if (fuelAmount <= 0)
        {
            rocket.GetComponent<RocketMovement>().hasFuel = false;
        }
    }


    public void addFuel(float f)
    {
        fuelAmount += f;
    }
}
