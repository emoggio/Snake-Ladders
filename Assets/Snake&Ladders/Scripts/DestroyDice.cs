using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDice : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Play the explosion sound effect.
        //m_ExplosionAudio.Play();

        // Destroy the shell.
        Destroy(gameObject);
    }
}
