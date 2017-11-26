using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WatchAlarm : MonoBehaviour {
    private SteamVR_TrackedController _controller;

    void Start() {
        _controller = GetComponent<SteamVR_TrackedController>() ?? gameObject.AddComponent<SteamVR_TrackedController>();
    }

    // Update is called once per frame
	void Update () {
	    if (!GameManager.GameOver) return;

        
	    SteamVR_Controller.Input((int)_controller.controllerIndex).TriggerHapticPulse(3000);
    }
}
