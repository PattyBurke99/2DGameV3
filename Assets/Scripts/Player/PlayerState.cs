using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerState : NetworkBehaviour {

    private ItemData itemData;

    [SyncVar]
    public bool isSprinting;
    [SyncVar]
    public int itemInHand;

    public int drawnItem;
    public bool hidingItem;

	void Start () {

        itemData = GameObject.Find("ItemData").GetComponent<ItemData>();
        drawnItem = 0;
        hidingItem = false;

        if (!isLocalPlayer)
        {
            UpdateItemInHand(0);
            UpdateIsSprinting(false);
        }
	}
	
	void Update () {
		if (!isLocalPlayer)
        {
            if (drawnItem!=itemInHand)
            {
                if (drawnItem != 0)
                {
                    foreach (Transform child in transform)
                    {
                        if (child.GetComponent<ItemBehavior>() != null && child.GetComponent<ItemBehavior>().ID == drawnItem)
                        {
                            Destroy(child.gameObject);
                        }
                    }
                }
                if (itemInHand != 0)
                {
                    GameObject newItem = itemData.initializeItem(itemInHand, transform);
                    if (!isSprinting)
                        newItem.GetComponent<MeshRenderer>().enabled = true;
                }

                drawnItem = itemInHand;
            }
            if (hidingItem!=isSprinting)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent<ItemBehavior>() != null && child.GetComponent<ItemBehavior>().ID == drawnItem)
                    {
                        child.GetComponent<MeshRenderer>().enabled = !isSprinting;
                    }
                }
                hidingItem = isSprinting;
            }

        }
	}

    public void UpdateItemInHand(int newValue)
    {
        if (isServer)
        {
            itemInHand = newValue;
        }
        else
        {
            CmdUpdateItemInHand(newValue);
        }
    }

    [Command]
    void CmdUpdateItemInHand(int newValue)
    {
        UpdateItemInHand(newValue);
    }

    public void UpdateIsSprinting(bool newValue)
    {
        if (isServer)
        {
            isSprinting = newValue;
        }
        else
        {
            CmdUpdateIsSprinting(newValue);
        }
    }

    [Command]
    void CmdUpdateIsSprinting(bool newValue)
    {
        UpdateIsSprinting(newValue);
    }
}
