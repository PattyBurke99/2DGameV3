using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour {

    public int ID;
    public Sprite inventorySprite;
    public bool inHand;
    public Vector3 localPosition;
    public bool usable;

    public bool isGun;
    public int clipSize;
    public int bulletsInClip;
    public float shotCooldown;
    public float lastShot;
}
