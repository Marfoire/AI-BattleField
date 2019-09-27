using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticleSystemThenDelete : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<ParticleSystem>().Play();
    }

    void FixedUpdate()
    {
        if(GetComponent<ParticleSystem>().isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}
