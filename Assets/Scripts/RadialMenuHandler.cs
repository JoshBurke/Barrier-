using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuHandler : MonoBehaviour {

    public bool DEFENSE_ENABLED = true;
    public bool MAGIC_ENABLED = true;
    public GameObject leftShield;
    public GameObject radialMenu;
    public GameObject magicRadialMenu;
    public Transform menuParent;
    public Transform playerEye;
    public DefensePowerups defenseScript;
    public MagicPowerups magicScript;

    private bool defenseMenuDisplayed;
    private bool magicMenuDisplayed;
    private GameObject defenseMenu;
    private GameObject magicMenu;

	// Use this for initialization
	void Start () {
        defenseMenuDisplayed = false;
        magicMenuDisplayed = false;
        defenseMenu = null;
        magicMenu = null;
	}
	
	// Update is called once per frame
	void Update () {
        if (OVRInput.GetDown(OVRInput.RawButton.X) && !defenseMenuDisplayed && !magicMenuDisplayed && DEFENSE_ENABLED)
        {
            defenseMenuDisplayed = true;
            defenseMenu = Instantiate(radialMenu, menuParent);
            defenseMenu.transform.parent = null;
            defenseMenu.transform.LookAt(playerEye.position + Vector3.down * 0.1f);
            defenseMenu.GetComponent<RadialMenu>().SetEnabledPowerups(defenseScript.MagnetAvailable, defenseScript.BoomAvailable, defenseScript.ShieldAvailable, defenseScript.SlowAvailable);
        }
        if (OVRInput.GetUp(OVRInput.RawButton.X))
        {
            defenseMenuDisplayed = false;
            if (defenseMenu)
            {
                defenseScript.currPowerUp = (DefensePowerups.PowerUp)defenseMenu.GetComponent<RadialMenu>().GetSelectedAbility();
                Destroy(defenseMenu);
            }
        }

        if (OVRInput.GetDown(OVRInput.RawButton.Y) && !magicMenuDisplayed && !defenseMenuDisplayed && MAGIC_ENABLED)
        {
            magicMenuDisplayed = true;
            magicMenu = Instantiate(magicRadialMenu, menuParent);
            magicMenu.transform.parent = null;
            magicMenu.transform.LookAt(playerEye.position + Vector3.down * 0.1f);
            magicMenu.GetComponent<RadialMenu>().SetEnabledPowerups(magicScript.FireAvailable, magicScript.IceAvailable, magicScript.ThunderAvailable, false);
        }

        if (OVRInput.GetUp(OVRInput.RawButton.Y))
        {
            magicMenuDisplayed = false;
            if (magicMenu)
            {
                magicScript.currPowerUp = (MagicPowerups.PowerUp)magicMenu.GetComponent<RadialMenu>().GetSelectedAbility();
                Destroy(magicMenu);
            }
        }
    }
}
