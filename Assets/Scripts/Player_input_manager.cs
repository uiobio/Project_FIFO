using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_input_manager : MonoBehaviour
{
    public static Player_input_manager instance;

    private void Awake() // makes Player_input_manager callable in any script: Player_input_manager.instance.[]
    {
        instance = this;
    }

    [Header("Movement")]
    // Player's movespeed
    [SerializeField]
    private float moveSpeed;
    // The amount of force that is multiplied to the orientation vector when dashing
    [Header("Dash")]
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashDistance;
    private float dashCompleteTime = -1;
    // Projectile prefab for the FireProjectile action
    [Header("Projectile")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float ProjectilePlaneY;

    [Header("Slash")]
    [SerializeField]
    private GameObject slashPrefab;

    // From InputManager
    private float horizontalInput;
    private float verticalInput;
    private bool dashInput;
    private bool interactInput;
    private bool fireProjectileInput;
    private bool attackInput;

    // Other GameObject, set when the player attempts to interact with them
    private GameObject interactable;

    // Dynamically updated movement direction; Set to the zero vector when not moving
    private Vector3 moveDirection;
    // Effectively the same as the moveDirection, except when not moving, the orientation stores the last non-zero moveDirection to allow dashing while stationary.
    private Vector3 orientation = new Vector3(0, 0, 1);
    private Vector3 up = new Vector3(-1, 0, -1);
    private Vector3 right = new Vector3(-1, 0, 1);
    private Vector3 aimPoint = new Vector3(0, 0, 0);

    private Rigidbody rb;
    private Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        cam = Camera.main;

        // Set the initial rotation and projectile aim of the player
        aimPoint.y = ProjectilePlaneY;
        AimToMousePoint();
        RotatePlayerToMousePoint();
    }


    void Update()
    {
        // If the player is dashing, continue with fixed velocity, reject new player inputs
        // Otherwise, read inputs
        if (Time.time < dashCompleteTime)
        {
            rb.linearVelocity = orientation.normalized * dashSpeed;
        }
        else
        {
            // Called once per frame. Reads input & updates vars
            GetInput();
            AimToMousePoint();
            RotatePlayerToMousePoint();
        }

        // If dash input pressed and the dash is not on cooldown, execute the dash
        if (dashInput && !Cooldown_manager.instance.IsDashOnCooldown)
        {
            StartDash(moveDirection.normalized);
        }

        // If interact input pressed and an interactable object has been set, execute interaction behavior with it.
        if (interactInput && interactable)
        {
            Interact(interactable);
        }

        // If fireProjectile input pressed and the projectile is not on cooldown, fire the projectile
        if (fireProjectileInput && !Cooldown_manager.instance.IsFireProjectileOnCooldown)
        {
            FireProjectile();
        }

        if (attackInput){
            BasicAttack();
        }
    }

    void FixedUpdate()
    {
        // Called once per set amount of time. Moves Player
        MovePlayer();
    }

    void GetInput()
    {
        // Sets all input variables to their respective keybinds (as defined in Project Settings --> Input Manager)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        dashInput = Input.GetButtonDown("Dash");
        interactInput = Input.GetButtonDown("Interact");
        fireProjectileInput = Input.GetButtonDown("Fire2");
        attackInput = Input.GetButtonDown("Fire1");
    }

    void MovePlayer()
    {
        // Calculate moveDirection
        moveDirection = up * verticalInput + right * horizontalInput;

        // Store the last non-zero moveDirection as the orientation
        if (!moveDirection.Equals(new Vector3(0, 0, 0)))
        {
            orientation = moveDirection.normalized;
        }

        // Add the forces in direction specified by moveDirection and speed specified by moveSpeed
        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);

    }

    void StartDash(Vector3 vect)
    {
        // Dashes a with a speed specified by dashScalar and direction specified by orientation
        Cooldown_manager.instance.UpdateDashCooldown();
        rb.linearVelocity = orientation * dashSpeed;
        dashCompleteTime = Time.time + (dashDistance / dashSpeed);
    }

    void Interact(GameObject interactable)
    {
        // If the other GameObject is a ShopItem and the shop is active, buy the item.
        if (interactable.tag == "ShopItem" && interactable.GetComponent<Shop_interaction_manager>().IsShopActive)
        {
            interactable.GetComponent<Shop_interaction_manager>().buy();
        }
    }

    // Instantiates a projectile that moves toward the position defined by aimPoint at the time of firing
    void FireProjectile()
    {
        // Projectile starts at the position of the "nose" of the player
        GameObject projectile = Instantiate(projectilePrefab, new Vector3(transform.Find("Forward").position.x, ProjectilePlaneY, transform.Find("Forward").position.z), transform.rotation);
        projectile.transform.LookAt(aimPoint);
        projectile.layer = 7; // Set to PlayerProjectiles layer, this makes it so it can only hit enemies and obstacles, but not the player.
        Cooldown_manager.instance.UpdateFireProjectileCooldown();
    }

    void BasicAttack()
    {
        GameObject Slash = Instantiate(slashPrefab, new Vector3(transform.Find("Forward").position.x, transform.position.y, transform.Find("Forward").position.z), transform.rotation);
        Slash.transform.parent = transform;
    }

    // Calculates the world position on the projectile firing plane to which the mouse is pointing
    void AimToMousePoint()
    {
        float hit;
        Plane plane = new Plane(Vector3.up, -1 * ProjectilePlaneY);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out hit))
        {
            aimPoint = ray.GetPoint(hit);
        }
    }

    // Rotates the player towards the aimPoint
    void RotatePlayerToMousePoint()
    {
        Vector3 direction = new Vector3(aimPoint.x, transform.position.y, aimPoint.z);
        direction = direction - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20f);
    }

    // Waits until the player a) confirms that they want to replace the selected upgrade with some shopItem upgrade or b) leaves from the radius of the shopItem.
    // If the player confirms they want to replace, then swap out the upgrade. If not, do nothing.
    // While loop is so the player can make live selections with the upgrade slot UI to choose which upgrade to swap. The text will thus update in real time.
    // It's a coroutine and not a function so it can run alongside Update() blocks, without interrupting core game processes.
    public IEnumerator SelectAndConfirmReplace(Upgrade newUpgrade, GameObject shop)
    {
        yield return null; // Wait one frame so no extraneous inputs can leak through
        while (true)
        {
            int tempIndex = Level_manager.instance.CurrentlySelectedUpgradeIndex;
            shop.GetComponent<Shop_interaction_manager>().ActiveLabelTextHotkeyInfo = "(E) Confirm \n (MB1) Select \n<size=80%><color=" + shop.GetComponent<Shop_interaction_manager>().LabelTextHotkeyInfoColor + "> Replace [<i>" + Level_manager.instance.PlayerHeldUpgrades[Level_manager.instance.CurrentlySelectedUpgradeIndex].Name + "</i>] with the following upgrade?</color><size=100%>";
            shop.GetComponent<Shop_interaction_manager>().MakeFullFormattedTextString();

            // Waits until one and only one of three things happens:
            // a) the player interacts with the ShopItem, confirming the replacement
            // b) the player leaves the radius of the ShopItem's trigger collider, cancelling the replacement
            // c) the player clicks on a new upgrade icon in the upgrade slot UI, thus continuing the while loop and reloading the text to show the newly selected upgrade to swap.
            yield return new WaitUntil(() => interactInput ^ !shop.GetComponent<Shop_interaction_manager>().IsShopActive ^ tempIndex != Level_manager.instance.CurrentlySelectedUpgradeIndex);
            if (interactInput)
            {
                Level_manager.instance.swapOutUpgrade(newUpgrade, shop);
                break;
            }
            if (!shop.GetComponent<Shop_interaction_manager>().IsShopActive)
            {
                break;
            }
        }
    }

    // Getters, Setters
    public GameObject Interactable
    {
        get { return interactable; }
        set { interactable = value; }
    }

    public Vector3 AimPoint
    {
        get { return aimPoint; }
        set { aimPoint = value; }
    }
}
