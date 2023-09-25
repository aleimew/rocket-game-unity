using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float waitTime = 1f;
    [SerializeField] float forceMultiplier;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip landingSound;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem landingParticles;

    // gameObject rocket = GetComponent<Rocket>();

    public bool collisionEnabled;

    AudioSource crashSoundSource;
    AudioSource landingSoundSource;
    GameObject fuel;
    GameObject rocket;
    Rigidbody rb;

    private bool soundIsPlaying = false;
    private bool hasLanded;
    private int numberOfChildren = 5;
    void Start()
    {
        rocket = GameObject.Find("Rocket");
        fuel = GameObject.Find("Fuel");
        rb = GetComponent<Rigidbody>();

        // Physics.IgnoreCollision(rocket.GetComponent<Collider>(), fuel.GetComponent<Collider>());
        collisionEnabled = true;
        hasLanded = false;

        crashSoundSource = gameObject.AddComponent<AudioSource>();
        landingSoundSource = gameObject.AddComponent<AudioSource>();
        crashSoundSource.clip = crashSound;
        landingSoundSource.clip = landingSound;
    }

    private void OnCollisionEnter(Collision other)
    {
        // Debug.Log("Collided with: " + other.gameObject.name);

        switch (other.gameObject.tag)
        {
            case "LaunchPad":
                // Debug.Log("Collided with LaunchPad");
                break;
            case "Finish":
                // Debug.Log("Collided with LandingPad");
                Landed();
                break;
            case "Fuel":
                // Debug.Log("Collided with Fuel");
                GetFuel();
                break;
            default:
                // Debug.Log("Collided with something else");
                Crash();
                break;
        }
    }

    void Crash()
    {
        if (collisionEnabled || !hasLanded)
        {
            PlaySound("crash");
            PlayParticles("crash");
            DestroyRocket();
            StopAllThrusters();
            FreezeRocket();
            // Debug.Log("Crashed");
            StopAndWait("crash");
        }
    }

    void Landed()
    {
        hasLanded = true;
        PlaySound("landing");
        PlayParticles("landing");
        StopAllThrusters();
        FreezeRocket();
        // Debug.Log("Landed");
        StopAndWait("landing");
    }

    void GetFuel()
    {
        // KeepMoving();
        // fuel.GetComponent<SphereCollider>().enabled = false;
        Destroy(fuel);
    }



    private void PlaySound(string sound)
    {
        if (!soundIsPlaying &&
        (!crashSoundSource.isPlaying || !landingSoundSource.isPlaying))
        {
            switch (sound)
            {
                case "crash":
                    crashSoundSource.Play();
                    break;
                case "landing":
                    landingSoundSource.Play();
                    break;
            }
            soundIsPlaying = true;
        }
    }

    private void PlayParticles(string particle)
    {
        switch (particle)
        {
            case "crash":
                crashParticles.Play();
                break;
            case "landing":
                landingParticles.Play();
                break;
        }
    }

    private void DestroyRocket()
    {
        rocket.GetComponent<MeshRenderer>().enabled = false;
        for (int i = 0; i < numberOfChildren; i++)
        {
            rocket.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void FreezeRocket()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void StopAllThrusters()
    {
        rocket.GetComponent<RocketMovement>().HardStopThrusters();
    }

    private void StopAndWait(string condition)
    {
        GetComponent<RocketMovement>().enabled = false;
        GetComponent<AudioSource>().Stop();

        switch (condition)
        {
            case "crash":
                Invoke("Reload", waitTime);
                break;
            case "landing":
                Invoke("Next", waitTime);
                break;
        }
    }

    public void Reload()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void Next()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }


    // private void KeepMoving()
    // {
    //     if (rb.velocity.x > 0 || rb.velocity.x < 0)
    //     {
    //         float xVelocity = rb.velocity.x;
    //         XMovement(xVelocity);
    //     }

    //     if (rb.velocity.y > 0 || rb.velocity.y < 0)
    //     {
    //         float yVelocity = rb.velocity.y;
    //         YMovement(yVelocity);
    //     }
    // }

    // private void XMovement(float xVelocity)
    // {
    //     if (xVelocity > 0)
    //     {
    //         rb.AddRelativeForce(Vector3.right * (xVelocity * forceMultiplier) * Time.deltaTime);
    //     }
    //     else if (xVelocity < 0)
    //     {
    //         rb.AddRelativeForce(Vector3.left * (xVelocity * forceMultiplier) * Time.deltaTime);
    //     }
    // }

    // private void YMovement(float yVelocity)
    // {
    //     if (yVelocity > 0)
    //     {
    //         rb.AddRelativeForce(Vector3.up * (yVelocity * forceMultiplier) * Time.deltaTime);
    //     }
    //     else if (yVelocity < 0)
    //     {
    //         rb.AddRelativeForce(Vector3.down * (yVelocity * forceMultiplier) * Time.deltaTime);
    //     }
    // }
}
