using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    private Animator animator;
    private Rigidbody2D rigidBody;
    private new Camera camera;
    private Inventory inventory;
    private PlayerState playerState;
    private PlayerInteraction playerInteraction;

    private float runMultiplier = 1.0f;
    private bool isRunning = false;
    public float speed;
    private int mask;

    private Transform highlightedObject = null;

    void Start () {

        if (!isLocalPlayer)
        {
            enabled = false;
            return;
        }

        camera = Camera.main;
        inventory = GetComponent<Inventory>();
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        playerState = GetComponent<PlayerState>();
        playerInteraction = GetComponent<PlayerInteraction>();

        CmdRegisterChunkLoader();
    }

    [Command]
    void CmdRegisterChunkLoader()
    {
        GameObject.Find("[Server]").GetComponent<ChunkLoader>().RegisterPlayer(transform);
    }

    private void Update()
    {

        if (Input.GetMouseButton(0))
        {
            if (!isRunning && inventory.GetItemInHand() != null && inventory.GetItemInHand().usable)
            {
                playerInteraction.UseItem(inventory.GetItemInHand(), camera.ScreenToWorldPoint(Input.mousePosition).x, camera.ScreenToWorldPoint(Input.mousePosition).y);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (highlightedObject!=null)
            {
                playerInteraction.InteractWithObject(highlightedObject.parent);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            {
                if (!isRunning && inventory.GetItemInHand() != null && inventory.GetItemInHand().isGun
                    && inventory.GetItemInHand().bulletsInClip != inventory.GetItemInHand().clipSize)
                {
                    inventory.ReloadGun();
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) != isRunning)
        {
            isRunning = !isRunning;
            ToggleSprint();
        }

        for (int i = 0; i < 10; ++i)
        {
            if (Input.GetKeyDown("" + i))
            {
                inventory.SelectSlot(i);
            }
        }
    }

    void FixedUpdate() {

        camera.GetComponent<Transform>().position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, -10);

        if (!Application.isFocused)
        {
            return;
        }

        float input = -Input.GetAxis("Vertical");
        rigidBody.AddForce(gameObject.transform.up * speed * input * runMultiplier);

        var velocity = this.GetComponent<Rigidbody2D>().velocity;
        animator.SetFloat("Speed", velocity.magnitude);

        var playerPos = Camera.main.WorldToScreenPoint(transform.position);
        var dir = Input.mousePosition - playerPos;

        Vector3 mousePosition = new Vector3(0, 0, (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + 90f);
        transform.rotation = Quaternion.Euler(mousePosition);

        InteractionDetection(); 
    }

    void InteractionDetection()
    {
        mask = LayerMask.GetMask("Interactable");
        Transform interactionTarget = Physics2D.Raycast(transform.position - (transform.up / 2), -transform.up, 1.0f, mask).transform;
        if (highlightedObject!=null)
        {
            if (interactionTarget==null || interactionTarget != highlightedObject)
            {
                highlightedObject.GetComponent<SpriteRenderer>().enabled = false;
                highlightedObject = null;
            }
        }

        if (interactionTarget!=null && highlightedObject==null)
        {
            if (interactionTarget.GetComponent<InteractableObject>() != null)
            {
                highlightedObject = interactionTarget.Find("InteractionOutline");
                highlightedObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    void ToggleSprint()
    {
        if (isRunning)
        {
            runMultiplier = 2.0f;
        } else
        {
            runMultiplier = 1.0f;
        }
        playerState.UpdateIsSprinting(isRunning);
        inventory.HideDrawnItem(isRunning);
        animator.SetFloat("runMultiplier", runMultiplier);
    }
}
