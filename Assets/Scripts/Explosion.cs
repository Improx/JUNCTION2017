using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Explosion : MonoBehaviour
{

    public ParticleSystem Particles { get; private set; }

    public void Explode(float size) {
        Particles = GetComponent<ParticleSystem>();

        var main = Particles.main;
        main.startSpeed = size;

        Particles.Play();
        Destroy(gameObject, main.duration);

        foreach (var controller in FindObjectsOfType<SteamVR_TrackedController>()) {
            SteamVR_Controller.Input((int)controller.controllerIndex).TriggerHapticPulse(3000);
        }
    }

}