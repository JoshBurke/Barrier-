using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
    public Material NormalMat;
    public Material OverlappedMat;

    public GameObject sword;

    private bool isOverlapped;
    private Renderer ren;
    private int swordOn;
    private bool isSwordEnabled;

    public static GameObject currSword;

	void Start () {
        isOverlapped = false;
        ren = GetComponent<Renderer>();
        swordOn = 0;
        currSword = null;
        isSwordEnabled = false;
    }
	
	
	void Update () {
        if(isSwordEnabled)
        {
            if (this.gameObject.name == "LeftShield")
            {
                if (OVRInput.GetDown(OVRInput.Button.Three))
                {
                    if (swordOn == 0 || swordOn == 1)
                    {
                        if (Shield.currSword != null)
                        {
                            Destroy(Shield.currSword.gameObject);
                        }
                        swordOn = 1;
                        Shield.currSword = Instantiate(sword);
                        Shield.currSword.transform.parent = this.transform;
                        Shield.currSword.transform.rotation = this.transform.rotation * Quaternion.Euler(new Vector3(-50, 0, 0));
                        Shield.currSword.transform.position = this.transform.position;
                        Shield.currSword.transform.localScale = new Vector3(16, 16, 1);
                    }
                }
            }

            if (this.gameObject.name == "RightShield")
            {
                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    if (swordOn == 0 || swordOn == 2)
                    {
                        if (Shield.currSword != null)
                        {
                            Destroy(Shield.currSword.gameObject);
                        }
                        swordOn = 2;
                        Shield.currSword = Instantiate(sword);
                        Shield.currSword.transform.parent = this.transform;
                        Shield.currSword.transform.rotation = this.transform.rotation * Quaternion.Euler(new Vector3(-50, 0, 0));
                        Shield.currSword.transform.position = this.transform.position;
                        Shield.currSword.transform.localScale = new Vector3(16, 16, 1);
                    }
                }
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        if (other.tag == "Shield")
        {
            isOverlapped = true;
            ren.material = OverlappedMat;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        GameObject other = collider.gameObject;
        if (other.tag == "Shield")
        {
            isOverlapped = false;
            ren.material = NormalMat;
        }
    }

    public bool IsOverlapping()
    {
        return isOverlapped;
    }
}
