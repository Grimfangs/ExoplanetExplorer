using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOBehaviour : MonoBehaviour
{

    [SerializeField] float thrustValue = 1.0f;
    [SerializeField] float rotateValue = 5.0f;
    [SerializeField] GameObject body;
    [SerializeField] AudioClip boostSound;
    [SerializeField] ParticleSystem thrustParticles;
    [SerializeField] GameObject uFO;

    Rigidbody rb;
    AudioSource bodyAudio;
    AudioSource backgroundAudio;

    public bool gameOver = false;

    public GameObject checkpoint;

    CollisionBehaviour collisionBehaviour;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bodyAudio = body.GetComponent<AudioSource>();
        backgroundAudio = GetComponent<AudioSource>();
        collisionBehaviour = GetComponent<CollisionBehaviour>();

        bodyAudio.volume = PlayerPrefs.GetFloat("SFXVolume", 0.7f);
        backgroundAudio.volume = PlayerPrefs.GetFloat("BGAudioVolume", 0.5f);
    }

    void Update()
    {
        if (!gameOver)
        {
            manageInput();
            idleRotate();
        }
    }

    void manageInput ()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            thrust();
        }

        else
        {
            StopThrustFX();
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotate(1);
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rotate(-1);
        }
    }

    void StopThrustFX()
    {
        thrustParticles.Stop();
        bodyAudio.Stop();
    }

    void StartThrustFX()
    {
        if (!bodyAudio.isPlaying && !gameOver)
        {
            thrustParticles.Play();
            PlayAudio(boostSound);
        }
    }

    void thrust ()
    {
        rb.AddRelativeForce(Vector3.up * thrustValue * Time.deltaTime);

        StartThrustFX();

        boostRotate();

    }

    void rotate (int direction)
    {
        rb.freezeRotation = true;
        transform.Rotate(0.0f, 0.0f, rotateValue * Time.deltaTime * direction, Space.World);
        rb.freezeRotation = false;
    }

    void idleRotate ()
    {
        body.transform.Rotate(0.0f, rotateValue * Time.deltaTime, 0.0f);
    }

    void boostRotate ()
    {
        body.transform.Rotate(0.0f, rotateValue * Time.deltaTime * 2, 0.0f);
    }

    public void GameOver ()
    {
        if (!gameOver)
        {
            gameOver = true;
            backgroundAudio.Stop();
        }
    }

    public void PlayAudio (AudioClip sound)
    {
        bodyAudio.PlayOneShot(sound);
    }

    public void ResetToCheckpoint ()
    {
        StopAllAnimations();
        StopAllSoundFX();
        ResetOrientation();
        PlaceUFO();
    }

    void StopAllAnimations ()
    {
        // currently handled by CollisionBehaviour.cs
    }

    void StopAllSoundFX ()
    {
        bodyAudio.Stop();
    }

    void ResetOrientation ()
    {
        /*rb.freezeRotation = true;
        body.transform.rotation.Set(0.0f, 0.0f, 0.0f, 0.0f);
        uFO.transform.rotation.Set(0.0f, 0.0f, 0.0f, 0.0f);*/
    }

    void PlaceUFO ()
    {
        Vector3 newPosition = checkpoint.transform.position;
        newPosition.y += 1.0f;
        Quaternion zeroRotation = new Quaternion(0.0f,0.0f,0.0f,0.0f);
        uFO.transform.SetPositionAndRotation(newPosition, zeroRotation);
        body.transform.SetPositionAndRotation(newPosition, zeroRotation);
        rb.freezeRotation = false;
    }
}
