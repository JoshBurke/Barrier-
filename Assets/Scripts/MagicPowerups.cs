using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowerups : MonoBehaviour {

    public AILibrary lib;
    public NewBossAI nb;
    public enum PowerUp { NONE, FIRE, ICE, THUNDER, empty }
    GameObject obj;

    public PowerUp currPowerUp = PowerUp.NONE; //Default powerup is none

    public bool FireAvailable;  //These three booleans are true if the powerup is available for use.
    public bool IceAvailable;   //If the boolean is set to true, the ability is selectable on the menu.
    public bool ThunderAvailable; //If set to false, the icon will not show up on the menu and it will not be selectable.

    void Start () {
        FireAvailable = true;
        IceAvailable = true;
        ThunderAvailable = true;
    }
	
	void Update () {
        obj = GameObject.Find("NewBoss(Clone)");
        if (obj != null)
        {
        lib = obj.GetComponent<AILibrary>();
        nb = obj.GetComponent<NewBossAI>();
            switch (currPowerUp)
            {
                case PowerUp.NONE:
                    break;
                case PowerUp.FIRE:
                    if (FireAvailable)
                    {
                        lib.attackMethod = "FireMagic";
                        FireAvailable = false;
                        break;

                    }
                    break;
                    //Fire powerup is the active powerup, check for input or do something

                case PowerUp.ICE:
                    if (IceAvailable) {
                        lib.attackMethod = "IceMagic";
                        IceAvailable = false;
                        nb.isfreeze = true;
                    }
                    //Ice powerup is active powerup

                    break;
                case PowerUp.THUNDER:
                    if (ThunderAvailable) {
                        Debug.Log("thunder");
                        lib.attackMethod = "ThunderMagic";
                        ThunderAvailable = false;
                    }
                    //Thunder powerup is active powerup

                    break;
            }
        }
    }
}
