using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TempRoofScript : MonoBehaviour {

	
	void OnTriggerEnter2D (Collider2D other) {
        if (other.GetComponent<NetworkIdentity>() != null)
        {
            if (other.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
	}

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<NetworkIdentity>() != null)
        {
            if (other.GetComponent<NetworkIdentity>().isLocalPlayer)
            {
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}
