using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grabbable : MonoBehaviour
{
    public bool Flying = false;
    public abstract void Throw(Vector3 velocity);
    public abstract void Grab();

}
