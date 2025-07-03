using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopItem : MonoBehaviour
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

    private Transform colliderTransform;
    private Transform spriteTransform;

    // The upgrade associated with this ShopItem.
    public Upgrade Upgrade;

    // The sprites associated with the pedastal
    public Sprite PedastalEmpty;
    public Sprite PedastalEmptyShadow;
    public Sprite PedastalUp;
    public Sprite PedastalUpShadow;

    // Start is called before the first frame update
    void Start()
    { 
        colliderTransform = transform.Find("Collider");
        label = Instantiate(labelPrefab, colliderTransform.position, Quaternion.identity);
        label.name = "Upgrade ShopItem Label " + Upgrade.Name;
        
        label.transform.rotation = Quaternion.Euler(labelRotationXYZ[0], labelRotationXYZ[1], labelRotationXYZ[2]);
        label.transform.position += new Vector3(labelPositionOffsetXYZ[0], labelPositionOffsetXYZ[1], labelPositionOffsetXYZ[2]);
        label.transform.SetParent(colliderTransform);

        // Sets the hitbox size for the UI-label-popup-trigger of this ShopItem
        BoxCollider[] colliders = colliderTransform.GetComponents<BoxCollider>();
        BoxCollider triggerCollider = colliders.FirstOrDefault(c => c.isTrigger);
        if (triggerCollider)
        {
            triggerCollider.size = new Vector3(labelTriggerHitboxSize[0], labelTriggerHitboxSize[1], labelTriggerHitboxSize[2]);
        }

        // Assign the position of the center point of the top face of this ShopItem
        Renderer cubeRenderer = colliderTransform.GetComponent<MeshRenderer>();
        Vector3 center = cubeRenderer.bounds.center;
        float height = cubeRenderer.bounds.extents.y;
        topFaceCenterPos = center + new Vector3(0, height + 0.5f, 0);

        // Assign the pedastal sprite
        spriteTransform = transform.Find("Sprite");
        spriteTransform.GetComponent<SpriteRenderer>().sprite = PedastalUp;

        // Make the label invisible
        label.GetComponent<UpgradeLabel>().Initialize(Upgrade);
        label.GetComponent<UpgradeLabel>().ChangeLabelTextBasedOnGameState(Upgrade);
        MakeFullFormattedTextString();
        label.SetActive(false);
    }

    // Called when the player interacts with this Instance's gameObject
    public void Buy()
    {
        // Attempts to add the upgrade to the player's list. If this fails, cancel the buy.
        if (!LevelManager.Instance.AddPlayerUpgrade(Upgrade, gameObject))
        {
            return;
        }
        DestroyChildren();
    }

    public void DestroyChildren()
    {
        // Stops this gameObject's attempts to interact with the label
        bought = true;
        Destroy(transform.GetChild(2).gameObject);
        spriteTransform.GetComponent<SpriteRenderer>().sprite = PedastalEmpty;
        Label.SetActive(false);
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
    public void DrawLinesToLabelCorners() 
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

    public bool Bought
    {
        get => bought;
        set => bought = value;
    }
}
