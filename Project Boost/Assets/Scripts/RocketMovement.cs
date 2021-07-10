using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [SerializeField]
    private float rocketBoostSpeed = 1000f;
    [SerializeField]
    private float rocketRotationSpeed = 50f;
    [SerializeField]
    private AudioClip movementAudio;

    private Rigidbody _rigidBody;
    private AudioSource _audioSource;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessBoost();
        ProcessRotation();

    }

    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(rocketRotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-rocketRotationSpeed);
        }
    }

    private void ApplyRotation(float rotationFrame)
    {
        _rigidBody.freezeRotation = true; // freezing rotation so that we can manually rotate
        transform.Rotate(Vector3.right * rotationFrame * Time.deltaTime);
        _rigidBody.freezeRotation = false; // unfreezing rotation so that the world physics can take over
    }

    private void ProcessBoost()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // We add the relative force because if we rotate, we need to point into the direction in which is rotated
            _rigidBody.AddRelativeForce(Vector3.up * rocketBoostSpeed * Time.deltaTime);
            
            if (!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(movementAudio);
            }
        } else
        {
            StopAudio();
        }
    }

    private void StopAudio()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
