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
    [SerializeField]
    private ParticleSystem jetParticle;
    [SerializeField]
    private ParticleSystem rightParticle;
    [SerializeField]
    private ParticleSystem leftParticle;

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
            LeftRotation();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RightRotation();
        }
        else
        {
            StopRotation();
        }
    }

    private void StopRotation()
    {
        leftParticle.Stop();
        rightParticle.Stop();
    }

    private void RightRotation()
    {
        if (!rightParticle.isPlaying)
        {
            rightParticle.Play();
        }
        ApplyRotation(-rocketRotationSpeed);
    }

    private void LeftRotation()
    {
        if (!leftParticle.isPlaying)
        {
            leftParticle.Play();
        }
        ApplyRotation(rocketRotationSpeed);
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
            StartThrust();
        }
        else
        {
            StopThrust();
        }
    }

    private void StopThrust()
    {
        jetParticle.Stop();
        StopAudio();
    }

    private void StartThrust()
    {
        if (!jetParticle.isPlaying)
        {
            jetParticle.Play();
        }
        // We add the relative force because if we rotate, we need to point into the direction in which is rotated
        _rigidBody.AddRelativeForce(Vector3.up * rocketBoostSpeed * Time.deltaTime);

        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(movementAudio);
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
