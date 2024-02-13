using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 4000f);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Destroy(gameObject);
    }
}
