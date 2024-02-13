using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInteraction : NetworkBehaviour {

    public Inventory inventory;
    public GameObject bulletPrefab;
    public GameObject target;

	void Start () {
        if (!isLocalPlayer)
        {
            enabled = false;
            return;
        }

        inventory = GetComponent<Inventory>();
	}

    public void UseItem(ItemBehavior itemScript, float targetX, float targetY)
    {
        if (itemScript.isGun && itemScript.bulletsInClip > 0 && (Time.time - itemScript.lastShot > itemScript.shotCooldown))
        {
            itemScript.lastShot = Time.time;
            itemScript.bulletsInClip--;
            CmdUseGun(itemScript.transform.position.x, itemScript.transform.position.y, targetX, targetY);
        }
    }

    [Command]
    public void CmdUseGun(float localX, float localY, float targetX, float targetY)
    {
        GameObject target = new GameObject();
        target.transform.position = new Vector3(targetX, targetY, 0);

        GameObject origin = new GameObject();
        origin.transform.position = new Vector3(localX, localY, 0);

        Quaternion rotation = Quaternion.LookRotation(target.transform.position - origin.transform.position, gameObject.transform.TransformDirection(Vector3.back));

        GameObject bullet = Instantiate(bulletPrefab, origin.transform.position, new Quaternion(0, 0, rotation.z, rotation.w));

        Destroy(target);
        Destroy(origin);
        Destroy(bullet, 3);

        NetworkServer.Spawn(bullet);
    }

    public void InteractWithObject(Transform item)
    {
        if (item.GetComponent<InteractableObject>().pickup != 0)
        {
            int id = item.GetComponent<InteractableObject>().pickup;
            if (inventory.pickupItem(id))
            {
                CmdPickupObject(item.gameObject);
            } else
            {
                return;
            }
        }
    }

    [Command]
    public void CmdPickupObject(GameObject item)
    {
        NetworkServer.Destroy(item);
    }
}
