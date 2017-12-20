using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuChoice : MonoBehaviour {

    public bool selected = false;
    public bool available = false;
    public float timeSelected;
    public GameObject icon;
	void Start () {
		
	}

    private void Update()
    {
        if (icon != null)
        {
            if (available)
            {
                icon.GetComponent<SpriteRenderer>().color = Color.black;
            }
            else
            {
                icon.GetComponent<SpriteRenderer>().color = (Color.red / 2) + (Color.grey / 2);
            }
        }
    }

    public void SetAvailable(bool a)
    {
        available = a;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shield") && available)
        {
            selected = true;
            timeSelected = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shield"))
        {
            selected = false;
        }
    }
}
