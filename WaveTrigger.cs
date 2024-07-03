using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    public WaveSystem waveSystem;

    private bool waveStarted = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !waveStarted)
        {
            Debug.Log("Wave Started");
            waveSystem.StartWaveSystem();
            waveStarted = true;
            gameObject.SetActive(false); // Disable the trigger after it's been activated
        }
    }
}