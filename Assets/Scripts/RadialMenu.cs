using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour {

    public GameObject ULChoice;
    public GameObject URChoice;
    public GameObject LRChoice;
    public GameObject LLChoice;
    public GameObject CenterChoice;
    private GameObject[] Choices;
    private SpriteRenderer[] sprites = new SpriteRenderer[5];
    private Collider[] colliders = new SphereCollider[5];

    private GameObject choice;

    // Use this for initialization
    void Start () {
        sprites[0] = ULChoice.GetComponent<SpriteRenderer>();
        sprites[1] = URChoice.GetComponent<SpriteRenderer>();
        sprites[2] = LRChoice.GetComponent<SpriteRenderer>();
        sprites[3] = LLChoice.GetComponent<SpriteRenderer>();
        sprites[4] = CenterChoice.GetComponent<SpriteRenderer>();
        Choices = new GameObject[5] { ULChoice, URChoice, LRChoice, LLChoice, CenterChoice };

        choice = null;
    }
	
	// Update is called once per frame
	void Update () {
        choice = getChoice();
        SpriteRenderer ren = null;
        if (choice)
        {
            ren = choice.GetComponent<SpriteRenderer>();
        }
        foreach (SpriteRenderer s in sprites)
        {
            if (s.Equals(ren))
            {
                s.color = Color.yellow;
            }
            else
            {
                s.color = Color.white;
            }
        }
	}

    GameObject getChoice()
    {
        GameObject lastSelected = null;
        float lastSelectedTime = 0;
        foreach (GameObject g in Choices)
        {
            if (g.GetComponent<MenuChoice>().selected && g.GetComponent<MenuChoice>().timeSelected > lastSelectedTime)
            {
                lastSelected = g;
                lastSelectedTime = g.GetComponent<MenuChoice>().timeSelected;
            }
        }
        return lastSelected;
    }

    public int GetSelectedAbility()
    {
        if(choice == ULChoice)
        {
            return 1;
        } else if(choice == URChoice)
        {
            return 2;
        } else if(choice == LLChoice)
        {
            return 3;
        } else if(choice == LRChoice)
        {
            return 4;
        }
        else
        {
            return 0;
        }
    }

    public void SetEnabledPowerups(bool UL, bool UR, bool LR, bool LL)
    {
        ULChoice.GetComponent<MenuChoice>().SetAvailable(UL);
        URChoice.GetComponent<MenuChoice>().SetAvailable(UR);
        LRChoice.GetComponent<MenuChoice>().SetAvailable(LR);
        LLChoice.GetComponent<MenuChoice>().SetAvailable(LL);
    }
}
