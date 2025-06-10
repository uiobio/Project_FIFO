using System.Linq;
using UnityEngine;

public class Trashcan : MonoBehaviour
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
    // The position of the center point of the top face of this ShopItem
    private Vector3 topFaceCenterPos;
    private Transform colliderTransform;
    private Transform spriteTransform;

    // Instiantiated UI label object from the prefab
    [System.NonSerialized]
    public GameObject UILabel;
    // The currently selected upgrade, as dictated by Level_manager's currentlySelectedUpgradeIndex
    public Upgrade Upgrade;
    // The sprites associated with the Trashcan
    public Sprite TrashcanEmpty;
    public Sprite TrashcanGreen;
    public Sprite TrashcanPurple;
    public Sprite TrashcanRed;
    public Sprite TrashcanYellow;

    public bool IsTrashcanActive { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        colliderTransform = transform.Find("Collider");
        UILabel = Instantiate(labelPrefab, colliderTransform.position, Quaternion.identity);
        UILabel.name = "Trashcan Label";
        UILabel.GetComponent<UpgradeLabel>().IsTrashcan = true;
        UILabel.GetComponent<UpgradeLabel>().Initialize(Upgrade);
        UILabel.transform.rotation = Quaternion.Euler(labelRotationXYZ[0], labelRotationXYZ[1], labelRotationXYZ[2]);
        UILabel.transform.position += new Vector3(labelPositionOffsetXYZ[0], labelPositionOffsetXYZ[1], labelPositionOffsetXYZ[2]);
        UILabel.transform.SetParent(colliderTransform);

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
        topFaceCenterPos = center + new Vector3(0, height + 0.1f, 0);

        // Assign the trashcan sprite
        spriteTransform = transform.Find("Sprite");
        spriteTransform.GetComponent<SpriteRenderer>().sprite = TrashcanEmpty;

        UILabel.SetActive(false);
    }

    // Called when the player interacts with this Instance's gameObject
    public void use()
    {
    }

    // Makes a single formatted string from this ShopItem's hotkey info, name, and description strings
    public string MakeFullFormattedTextString()
    {
        string text = "";
        if (UILabel != null)
        {
            text = UILabel.GetComponent<UpgradeLabel>().MakeFullFormattedTextString(UILabel, true);
        }
        DrawLinesToLabelCorners();
        return text;
    }

    // Either draws the lines to the-corners-of-the-label from the-center-point-of-the-top-face-of-this-ShopItem
    // Or makes them invisible, depending on whether the shop is active.
    public void DrawLinesToLabelCorners()
    {
        if (UILabel != null)
        {
            UILabel.GetComponent<UpgradeLabel>().DrawLinesToLabelCorners(true, topFaceCenterPos, topFaceCenterPos, topFaceCenterPos, topFaceCenterPos);
        }
    }

    public GameObject Label
    {
        get { return UILabel; }
        set { UILabel = value; }
    }
}
