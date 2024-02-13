using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour {

    public GameObject[] item;

    private void Start()
    {
        item = new GameObject[11];
        item[1] = transform.Find("Pistol").gameObject;
        item[2] = transform.Find("PistolClip").gameObject;
        item[3] = transform.Find("AK47").gameObject;
        item[4] = transform.Find("AK47Clip").gameObject;
    }

    public GameObject initializeItem(int ID, Transform parent)
    {
        GameObject newItem = Instantiate(item[ID], parent);
        newItem.name = item[ID].name;
        newItem.transform.localPosition = item[ID].GetComponent<ItemBehavior>().localPosition;

        return newItem;
    }
	
}
