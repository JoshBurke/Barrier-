using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPowerups : MonoBehaviour {

    public AILibrary lib;
    public NewBossAI nb;
    public enum PowerUp { NONE, FIRE, ICE, THUNDER, empty }

    public PowerUp currPowerUp = PowerUp.NONE; //Default powerup is none

    public bool FireAvailable;  //These three booleans are true if the powerup is available for use.
    public bool IceAvailable;   //If the boolean is set to true, the ability is selectable on the menu.
    public bool ThunderAvailable; //If set to false, the icon will not show up on the menu and it will not be selectable.

    void Start () {
		
	}
	
	void Update () {
        switch (currPowerUp)
        {
            case PowerUp.NONE:
                break;
            case PowerUp.FIRE:
                //Fire powerup is the active powerup, check for input or do something
                lib.attackMethod = "FireMagic";
                FireAvailable = false;
                break;
            case PowerUp.ICE:
                //Ice powerup is active powerup
                lib.attackMethod = "IceMagic";
                IceAvailable = false;
                nb.isfreeze = true;
                break;
            case PowerUp.THUNDER:
                //Thunder powerup is active powerup
                lib.attackMethod = "ThunderMagic";
                ThunderAvailable = false;
                break;
        }
    }
}
