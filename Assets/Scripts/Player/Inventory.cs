using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Inventory : NetworkBehaviour {

    private GameObject[] inventory;

    private GUIManager guiManager;
    private Animator animator;
    private PlayerState playerState;
    private ItemData itemData;

    public int selectedSlot;
    public int startingItem;
    public bool hidingItem;

	void Start () {
        if (!isLocalPlayer)
        {
            enabled = false;
            return;
        }
        selectedSlot = -1;
        inventory = new GameObject[10];
        guiManager = Camera.main.transform.Find("GUI").GetComponent<GUIManager>();
        animator = GetComponent<Animator>();
        playerState = GetComponent<PlayerState>();
        itemData = GameObject.Find("ItemData").GetComponent<ItemData>();
    }

    public void SelectSlot(int newSlot)
    {
        bool animatorState = false;
        guiManager.DrawSlotBorder(newSlot);

        if (selectedSlot!=-1 && inventory[selectedSlot] != null && inventory[selectedSlot].GetComponent<ItemBehavior>().inHand)
        {
            inventory[selectedSlot].GetComponent<MeshRenderer>().enabled = false;
            playerState.UpdateItemInHand(0);
        }

        if (selectedSlot == newSlot)
        {
            selectedSlot = -1;
        }
        else
        {
            selectedSlot = newSlot;

            if (inventory[selectedSlot]!=null && inventory[selectedSlot].GetComponent<ItemBehavior>().inHand)
            {
                playerState.UpdateItemInHand(inventory[selectedSlot].GetComponent<ItemBehavior>().ID);
                if (!hidingItem)
                {
                    animatorState = true;
                    inventory[selectedSlot].GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }

        if (!hidingItem)
            animator.SetBool("itemInHand", animatorState);
    }

    public void HideDrawnItem(bool value)
    {
        if (selectedSlot != -1 && inventory[selectedSlot] != null && inventory[selectedSlot].GetComponent<ItemBehavior>().inHand) {
            inventory[selectedSlot].GetComponent<MeshRenderer>().enabled = !value;
            animator.SetBool("itemInHand", !value);
        }
        hidingItem = value;
    }

    public ItemBehavior GetItemInHand()
    {
        if (selectedSlot != -1 && inventory[selectedSlot] != null && inventory[selectedSlot].GetComponent<ItemBehavior>().inHand)
        {
            return inventory[selectedSlot].GetComponent<ItemBehavior>();
        } else
        {
            return null;
        }
    }

    void SetSlot(int slot, GameObject item)
    {
        if (item==null)
        {
            inventory[slot] = null;
            guiManager.SetSlotPicture(slot, null);
        } else
        {
            inventory[slot] = item;
            guiManager.SetSlotPicture(slot, item.GetComponent<ItemBehavior>().inventorySprite);
        }
    }

    public void ReloadGun()
    {
        for (int i = 1; i < 11; i++)
        {
            if (i == 10)
                i = 0;

            if (inventory[i] != null && inventory[i].gameObject.name == (inventory[selectedSlot].name + "Clip"))
            {
                ItemBehavior gunScript = inventory[selectedSlot].GetComponent<ItemBehavior>();
                gunScript.bulletsInClip = gunScript.clipSize;
                SetSlot(i, null);
                return;
            }

            if (i == 0)
                return;
        }
    }

    public bool pickupItem(int item)
    {
        for (int i=1; i < 11; i++)
        {
            if (i == 10)
                i = 0;

            if (inventory[i]==null)
            {
                SetSlot(i, itemData.initializeItem(item, transform));
                if (i==selectedSlot)
                {
                    HideDrawnItem(false);
                }
                return true;
            }

            if (i == 0)
                return false;
        }
        return false;
    }
}