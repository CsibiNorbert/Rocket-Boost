using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    private float levelLoadDelay = 1f;
    [SerializeField]
    private AudioClip collisionAudio;
    [SerializeField]
    private AudioClip finishAudio;
    [SerializeField]
    private ParticleSystem crashParticle;
    [SerializeField]
    private ParticleSystem successParticle;

    private AudioSource _audioSource;

    private bool isTransitioning = false;
    private bool isCollisionDisabled = false;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebug();
    }

    private void RespondToDebug()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GoToNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            isCollisionDisabled = !isCollisionDisabled;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || isCollisionDisabled)
            return;

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            case "Fuel":
                Debug.Log("Rocket Fuel");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        successParticle.Play();
        isTransitioning = true;
        _audioSource.Stop();
        _audioSource.PlayOneShot(finishAudio);

        GetComponent<RocketMovement>().enabled = false;
        Invoke("GoToNextLevel", levelLoadDelay);
    }

    private void StartCrashSequence()
    {
        crashParticle.Play();
        isTransitioning = true;
        _audioSource.Stop(); // Stop all sounds
        _audioSource.PlayOneShot(collisionAudio);

        GetComponent<RocketMovement>().enabled = false;
        Invoke("ReloadCurrentScene", levelLoadDelay);
    }

    private void GoToNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    private void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
