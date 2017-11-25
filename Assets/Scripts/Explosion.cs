using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    void Explode()
    {
        var exp = GetComponent<ParticleSystem>();
        var main = exp.main;
        exp.Play();
        Destroy(gameObject, main.duration);
    }
}
