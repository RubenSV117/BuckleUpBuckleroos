using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys particle system once it is done playing
/// 
/// Ruben Sanchez
/// 4/5/18
/// </summary>
public class DestroyParticleOnFinish : MonoBehaviour
{
    private ParticleSystem _particle;

    // Use this for initialization
    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_particle && !_particle.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
