using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {

    public bool on;
    public Sprite icon;
    public abstract void TurnOn();

}
