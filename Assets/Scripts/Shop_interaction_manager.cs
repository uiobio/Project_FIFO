using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Shop_interaction_manager : MonoBehaviour
{
    [Header("Shop")]
    // UI label prefab that pops up when player comes near
    [SerializeField]
    private GameObject labelPrefab;

    // Position and rotation of UI label
    [SerializeField]
    private float[] labelRotationXYZ;
    [SerializeField]
    private float[] labelPositionOffsetXYZ;

    // Hitbox size of the UI label popup trigger
    [SerializeField]
    private float[] labelTriggerHitboxSize;
    
    // Instiantiated UI label object from the prefab
    private GameObject label;

    private bool isShopActive = false;
    private bool bought = false;

    // The position of the center point of the top face of this ShopItem
    private Vector3 topFaceCenterPos;

    // The upgrade associated with this ShopItem.
    public Upgrade upgrade;

    // Start is called before the first frame update
    void Start()
    { 
        label = Instantiate(labelPrefab, transform.position, Quaternion.identity);
        label.name = "Upgrade ShopItem Label " + upgrade.Name;
        
        label.transform.rotation = Quaternion.Euler(labelRotationXYZ[0], labelRotationXYZ[1], labelRotationXYZ[2]);
        label.transform.position += new Vector3(labelPositionOffsetXYZ[0], labelPositionOffsetXYZ[1], labelPositionOffsetXYZ[2]);
        label.transform.SetParent(gameObject.transform);

        // Sets the hitbox size for the UI-label-popup-trigger of this ShopItem
        BoxCollider[] colliders = GetComponents<BoxCollider>();
        BoxCollider triggerCollider = colliders.FirstOrDefault(c => c.isTrigger);
        if (triggerCollider)
        {
            triggerCollider.size = new Vector3(labelTriggerHitboxSize[0], labelTriggerHitboxSize[1], labelTriggerHitboxSize[2]);
        }

        // Assign the position of the center point of the top face of this ShopItem
        Renderer cubeRenderer = GetComponent<MeshRenderer>();
        Vector3 center = cubeRenderer.bounds.center;
        float height = cubeRenderer.bounds.extents.y;
        topFaceCenterPos = center + new Vector3(0, height + 0.1f, 0);


        // Make the label invisible
        label.GetComponent<UpgradeLabel>().Initialize(upgrade);
        MakeFullFormattedTextString();
        label.SetActive(false);
    }

    // Called when another collider enters the trigger hitbox
    private void OnTriggerEnter(Collider other) 
    {
        // If the other collider is the player, and the item has not already been bought:
        // Make the UI label visible, allow the player to interact with this instance's gameObject, draw the lines
        // Update the text to reflect changes in game state
        if (other.gameObject.CompareTag("Player") && !bought)
        {
            label.GetComponent<UpgradeLabel>().ChangeLabelTextBasedOnGameState(upgrade);
            label.SetActive(true);
            // Update the TextMeshPro component according to the new active text
            Player_input_manager.instance.Interactable = gameObject;
            isShopActive = true;
            DrawLinesToLabelCorners();
        }
    }

    // Called when another collider exits the trigger hitbox
    private void OnTriggerExit(Collider other) 
    {
        // If the other collider is the player, and the item has not already been bought:
        // Make the UI label invisible and disallow the player to interact with this instance's gameObject; make the lines invisble
        if (other.gameObject.tag == "Player" && !bought)
        {
            label.SetActive(false);
            isShopActive = false;
            DrawLinesToLabelCorners();
        }
    }

    // Called when the player interacts with this instance's gameObject
    public void buy()
    {
        // Attempts to add the upgrade to the player's list. If this fails, cancel the buy.
        if (!Level_manager.instance.AddPlayerUpgrade(upgrade, gameObject))
        {
            return;
        }
        destroyChildren();
    }

    // Destroys the children of this ShopItem (the upgrade, label, and lines)
    public void destroyChildren()
    {
        // Stops this gameObject's attempts to interact with the label
        bought = true;
        // Destroys all children of this instance's gameObject
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Makes a single formatted string from this ShopItem's hotkey info, name, and description strings
    public string MakeFullFormattedTextString()
    {
        string text = "";
        if (label != null)
        {
            text = label.GetComponent<UpgradeLabel>().MakeFullFormattedTextString(label, true);
        }
        DrawLinesToLabelCorners();
        return text;
    }

    // Either draws the lines to the-corners-of-the-label from the-center-point-of-the-top-face-of-this-ShopItem
    // Or makes them invisible, depending on whether the shop is active.
    private void DrawLinesToLabelCorners() 
    {
        if (label != null)
        {
            label.GetComponent<UpgradeLabel>().DrawLinesToLabelCorners(isShopActive, topFaceCenterPos, topFaceCenterPos, topFaceCenterPos, topFaceCenterPos);
        }
    }

    // Getters, Setters
    public bool IsShopActive 
    {
        get { return isShopActive; }
        set { isShopActive = value; }
    }

    public GameObject Label 
    { 
        get { return label; }
        set { label  = value; }
    }
}
