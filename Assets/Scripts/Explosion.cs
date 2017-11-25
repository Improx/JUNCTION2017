using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public void Explode(float size)
    {
        var exp = GetComponent<ParticleSystem>();
        var main = exp.main;
        main.startSpeed = size;
        
        exp.Play();
        Destroy(gameObject, main.duration);
    }
}
