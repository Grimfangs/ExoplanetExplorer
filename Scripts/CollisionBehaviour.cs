using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionBehaviour : MonoBehaviour
{
    [SerializeField] float delay = 4.0f;

    [SerializeField] public AudioClip explosionSound;
    [SerializeField] AudioClip victorySound;

    [SerializeField] public ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem victoryParticles;
    int currentLevel;

    bool noCrashCheat = false;

    bool cheatsEnabled = false;

    UFOBehaviour uFOBehaviour;
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        uFOBehaviour = GetComponent<UFOBehaviour>();

        cheatsEnabled = PlayerPrefs.GetInt("CheatsEnabled",0) == 1 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        ManageCheats();
    }

    void OnCollisionEnter (Collision other)
    {
        if (uFOBehaviour.gameOver)
            return;
        
        switch (other.gameObject.tag)
        {
            case "Respawn": 
                Debug.Log("Let's go!");
                break;

            case "Checkpoint":
                AddCheckpoint(other.gameObject);
                break;
            
            case "Finish":
                StartVictoryDance();
                break;

            default:
                if (noCrashCheat)
                {
                    Debug.Log ("Boink");
                    break;
                }
                else
                {
                StartCrashSequence();
                break;
                }
        }
    }

    void AddCheckpoint (GameObject cp)
    {
        Debug.Log("Checkpoint achieved!");
        uFOBehaviour.checkpoint = cp;
        ParticleSystem cpParticles = cp.GetComponentInChildren<ParticleSystem>(true);
        cpParticles.Play();
    }
    

    void StartCrashSequence ()
    {
        explosionParticles.Play();
        uFOBehaviour.PlayAudio(explosionSound);

        if (uFOBehaviour.checkpoint == null)
        {
            uFOBehaviour.GameOver();
            Invoke("ReloadLevel", delay);
        }
            else
        {
            Invoke("StartResetSequence", delay);
        }
    }

    void StartResetSequence ()
    {
        explosionParticles.Stop(true);
        uFOBehaviour.ResetToCheckpoint();
    }
    void StartVictoryDance ()
    {
        uFOBehaviour.GameOver();
        victoryParticles.Play();
        uFOBehaviour.PlayAudio(victorySound);
        Invoke("LoadNextLevel", delay);
    }
    void ReloadLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }

    void LoadNextLevel()
    {
        if (SceneManager.sceneCountInBuildSettings-1 > currentLevel)
        {
            SceneManager.LoadScene(currentLevel+1);
        }
        else
        {
            Debug.Log("This is the end of the road for you pal.");
            SceneManager.LoadScene(0);
        }
    }

    void ManageCheats()
    {
        if (!cheatsEnabled)
        {
            return;
        }
        else
        {
            if (Input.GetKeyUp(KeyCode.L))
            {
                LoadNextLevel();
            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                ToggleCollision();
            }

            if (Input.GetKeyUp(KeyCode.R))
            {
                if (uFOBehaviour.checkpoint == null)
                {
                    ReloadLevel();
                }
                else
                {
                    StartResetSequence();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    void ToggleCollision()
    {
        if (noCrashCheat)
        {
            Debug.Log("Collisions on.");
        }
        else
        {
            Debug.Log("Collisions off.");
        }
        noCrashCheat = !noCrashCheat;
    }
}
