using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticleSystemThenDelete : MonoBehaviour
{

    private void Awake()
    {
        GetComponent<ParticleSystem>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<ParticleSystem>().isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}
