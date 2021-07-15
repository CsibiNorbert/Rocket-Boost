using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startPosition;

    [SerializeField]
    Vector3 movePosition;

    [SerializeField]
    [Range(0,1)]
    float movementFactor = 0f;

    [SerializeField]
    float period = 2f;

    const float tau = Mathf.PI * 2; // 6.283 full cycle
    float rawSinWave;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) // the reason why we don`t put == 0 is because floating numbers are always different by a tiny bit
            return;

        float cycles = Time.time * period; // 2 seconds elapsed is one full cycle, its growing
        rawSinWave = Mathf.Sin(cycles * tau); // and this will give a number between -1 and 1

        // recalculated to go froom 0 to 1
        movementFactor = (rawSinWave + 1f) / 2; // here we want the factor to be from 0 to 1, hence we add one to the rawWave so that it goes from -1 to 0 and then we divide by 2 because when its 1, the wave will be 2.

        Vector3 offset = movePosition * movementFactor;
        transform.position = startPosition + offset;
    }
}
