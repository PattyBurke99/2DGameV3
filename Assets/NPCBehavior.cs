using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NPCBehavior : NetworkBehaviour {

    public enum State { Idle, Targeting}
    public State state;

    void Start () {
        if (!isServer)
        {
            enabled = false;
            return;
        }
	}
	
	void Update () {
		switch (state)
        {
            case State.Idle:
                break;
            case State.Targeting:
                break;
        }
	}
}
