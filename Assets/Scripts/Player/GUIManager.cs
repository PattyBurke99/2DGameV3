using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {

    public GameObject pistolOutline;
    private Transform inventoryBorderPos;
    private int slotDrawn = -1;

    Color Opacity;

    public void Start()
    {
        Opacity = transform.Find("Slot" + 1).GetComponent<SpriteRenderer>().color;
    }

    public void SetSlotPicture(int slot, Sprite sprite)
    {
        GameObject newSlot = transform.Find("Slot" + slot).gameObject;
        if (sprite!=null)
        {
            newSlot.GetComponent<SpriteRenderer>().sprite = sprite;
        } else
        {
            newSlot.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    public void DrawSlotBorder(int newSlot)
    {

        if (slotDrawn!=-1)
            transform.Find("Slot" + slotDrawn).GetComponent<SpriteRenderer>().color = new Color(Opacity.r, Opacity.g, Opacity.b, 0.5882f);

        if (slotDrawn==newSlot)
        {
            transform.Find("Slot" + newSlot).GetComponent<SpriteRenderer>().color = new Color(Opacity.r, Opacity.g, Opacity.b, 0.5882f);
            transform.Find("SelectedItem").GetComponent<SpriteRenderer>().enabled = false;
            slotDrawn = -1;
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                if (newSlot == i)
                {
                    inventoryBorderPos = transform.Find("ItemSlot" + i).gameObject.transform;
                    transform.Find("SelectedItem").position = new Vector3(inventoryBorderPos.position.x, inventoryBorderPos.position.y, -9.5f);
                    transform.Find("SelectedItem").GetComponent<SpriteRenderer>().enabled = true;
                    transform.Find("Slot" + newSlot).GetComponent<SpriteRenderer>().color = new Color(Opacity.r, Opacity.g, Opacity.b, 255);
                    slotDrawn = i;
                }
            }
        }
    }

}
