using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    Rigidbody rb;
    GameObject launchPad;
    AudioSource audioSource;
    bool playSound;

    public bool hasFuel;

    [SerializeField] float thrust = 50f;
    [SerializeField] float rotationSpeed = 50f;
    [SerializeField] float audioVolume = 0.2f;
    [SerializeField] AudioClip rocketEngine;
    [SerializeField] ParticleSystem MainThrust;
    [SerializeField] ParticleSystem LeftThrust;
    [SerializeField] ParticleSystem RightThrust;


    private bool alive;
    private KeyCode currentKey;

    // Start is called before the first frame update
    void Start()
    {
        //should always start on launch pad
        rb = GetComponent<Rigidbody>();
        launchPad = GameObject.Find("LaunchPad");
        // transform.position = launchPad.transform.position;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = audioVolume;
        playSound = false;
        hasFuel = true;
        // Debug.Log("Audio Source: " + audioSource);
        // Debug.Log("Audio Volume: " + audioSource.volume);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFuel)
        {
            RocketThrust();
            RocketTurn();
            RocketSound();
        }
    }

    void RocketThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartMainThruster();
        }
        else
        {
            StopMainThruster();
        }
    }

    void RocketSound()
    {
        if (playSound && !audioSource.isPlaying)
        {
            PlaySound();
        }
        else if (!playSound && audioSource.isPlaying)
        {
            stopAudio();
        }
    }

    void RocketTurn()
    {
        if (Input.GetKey(KeyCode.A))
        {
            TurnLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            TurnRight();
        }
        StopThrusters();
    }



    private void StartMainThruster()
    {
        //activate rocket
        // GetCurrentKey(KeyCode.Space);
        rb.AddRelativeForce(Vector3.up * thrust * Time.deltaTime);
        playSound = true;
        ActivateThrusters(KeyCode.Space);
    }

    private void StopMainThruster()
    {
        playSound = false;
        StopThrusters();
    }

    private void TurnLeft()
    {
        //turn rocket
        // GetCurrentKey(KeyCode.A);
        Rotate(rotationSpeed);
        ActivateThrusters(KeyCode.A);
        // Debug.Log("A");
    }

    private void TurnRight()
    {
        //turn rocket
        // GetCurrentKey(KeyCode.D);
        Rotate(-rotationSpeed);
        ActivateThrusters(KeyCode.D);
        // Debug.Log("D");
    }

    private void PlaySound()
    {
        audioSource.PlayOneShot(rocketEngine);
    }

    private void stopAudio()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= 0.0001f;
            // Debug.Log("Volume: " + audioSource.volume);
        }
        if (audioSource.volume <= 0)
        {
            audioSource.Stop();
            // Debug.Log("audio stopped");
            audioSource.volume = audioVolume;
        }
    }

    private void Rotate(float rotation)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually Rotate
        transform.Rotate(Vector3.forward * Time.deltaTime * rotation);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }

    private void ActivateThrusters(KeyCode key)
    {
        if (key == KeyCode.Space && !MainThrust.isPlaying)
        {
            MainThrust.Play();
        }

        if (key == KeyCode.A && !LeftThrust.isPlaying)
        {
            LeftThrust.Play();
        }

        if (key == KeyCode.D && !RightThrust.isPlaying)
        {
            RightThrust.Play();
        }
    }

    private void StopThrusters()
    {
        if (MainThrust.isPlaying && !Input.GetKey(KeyCode.Space))
        {
            MainThrust.Stop();
        }

        if (LeftThrust.isPlaying && !Input.GetKey(KeyCode.A))
        {
            LeftThrust.Stop();
        }

        if (RightThrust.isPlaying && !Input.GetKey(KeyCode.D))
        {
            RightThrust.Stop();
        }
    }

    public void HardStopThrusters()
    {
        MainThrust.Stop();
        LeftThrust.Stop();
        RightThrust.Stop();
    }

    // private void GetCurrentKey(KeyCode key)
    // {
    //     if (key == KeyCode.Space)
    //     {
    //         currentKey = KeyCode.Space;
    //     }

    //     if (key == KeyCode.A)
    //     {
    //         currentKey = KeyCode.A;
    //     }

    //     if (key == KeyCode.D)
    //     {
    //         currentKey = KeyCode.D;
    //     }
    // }

    // void activateRocket()
    // {
    //     //activate rocket
    //     transform.Translate(Vector3.up * Time.deltaTime);
    // }

    // void turnRocket()
    // {
    //     //turn rocket
    //     transform.Rotate(Vector3.forward * Time.deltaTime);
    // }
}
