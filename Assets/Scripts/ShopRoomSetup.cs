using UnityEngine;
using System;
using System.Collections.Generic;

public class ShopRoomSetup : MonoBehaviour
{
    public static ShopRoomSetup Instance;
    [Header("Shop")]
    // The ShopItem prefab
    [SerializeField]
    private GameObject shopItemPrefab;
    [SerializeField]
    private GameObject shopItemUpgradePrefab;
    // How many ShopItems in the room
    [SerializeField]
    private int numShopItems;
    // How many small floor squares in between each ShopItem
    [SerializeField]
    private int numSquaresBetweenEachShopItem;
    // How many squares from the empty wall of the room the first ShopItem should be placed
    [SerializeField]
    private int numSquaresFirstShopItemFromEdge;
    // Which wall to line the ShopItems up on
    [SerializeField]
    private string wallDirection;
    [SerializeField]
    private List<int> presetUpgradeIds = new List<int>();

    // List of the 3 upgrades this shop room will display
    List<Upgrade> shopItemUpgrades = new List<Upgrade>();
    // List of the 3 gameObjects associated with the upgrades
    List<GameObject> shopItemUpgradeGameObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateUpgrades();
        for (int i = 0; i < numShopItems; i++)
        {
            GameObject shopItem;
            // Context:
            // Vector3(0.371f, 0.288f, 7.126f) represents the top right corner of the large floor square.
            // The ShopItemPrefab position is at the bottom left corner of the large floor square.
            // 0.471875f is the width of a single, small floor tile
            // 1.5f is to account for the fact that the position is calculated from the center of each ShopItem (so the expected "distance between" is effectively shortened by 2/3)
            if (wallDirection.Equals("Northwest"))
            {
                shopItem = Instantiate(shopItemPrefab, new Vector3(2.5f, 0, 2.5f) + new Vector3(0, 0, -0.471875f * i * numSquaresBetweenEachShopItem + -0.471875f * numSquaresFirstShopItemFromEdge), Quaternion.identity);
            }
            else
            {
                shopItem = Instantiate(shopItemPrefab, shopItemPrefab.transform.position + new Vector3(-0.471875f * i * numSquaresBetweenEachShopItem + -0.471875f * numSquaresFirstShopItemFromEdge, 0, 0), Quaternion.identity);
            }
            Renderer cubeRenderer = shopItem.gameObject.transform.Find("Collider").GetComponent<MeshRenderer>();
            Vector3 center = cubeRenderer.bounds.center;
            float height = cubeRenderer.bounds.extents.y;

            // 0.12f is a good height above the Shop to make the upgrade icon appear more 3D
            GameObject shopItemUpgrade = Instantiate(shopItemUpgradePrefab, center + new Vector3(0, height + 0.12f, 0), Quaternion.identity);

            // Give the label and upgrade each a reference to the appropriate Upgrade object.
            shopItemUpgrade.GetComponent<Upgrade_manager>().upgrade = shopItemUpgrades[i];
            shopItem.GetComponent<ShopItem>().Upgrade = shopItemUpgrades[i];

            // Tell the Upgrade_manager that we want to make a ShopItem (rather than a UI icon)
            shopItemUpgrade.GetComponent<Upgrade_manager>().upgrade.UIOrShopItem = "ShopItem";
            shopItemUpgrade.GetComponent<Upgrade_manager>().shopItem = shopItem.transform;
            shopItemUpgrade.GetComponent<Upgrade_manager>().wallDirection = wallDirection;
            shopItemUpgrade.GetComponent<Upgrade_manager>().CreateGameObjects();

            shopItem.name = "Upgrade ShopItem " + shopItemUpgrade.GetComponent<Upgrade_manager>().upgrade.Name;
            shopItemUpgrade.name = "Data for Upgrade ShopItem " + shopItemUpgrade.GetComponent<Upgrade_manager>().upgrade.Name;

            shopItemUpgradeGameObjects.Add(shopItem);
        }
    }

    // Generate a list of 3 distinct, random upgrades to place on the shops
    // FIXME: make these weighted based on how strong the upgrades are, once the upgrades are implemented
    void GenerateUpgrades()
    {
        int[] generatedIds = new int[numShopItems];

        // Make sure that none of the initial elements are 0s
        for (int i = 0; i < generatedIds.Length; i++)
        { 
            generatedIds[i] = -1;
        }
        int id = -1;
        for (int i = 0; i < numShopItems; i++)
        {
            // If there aren't enough upgrades loaded in the Upgrades list in the Level manager to create 3 random, distinct ShopItems, then do not attempt to randomize them
            if (LevelManager.Instance.Upgrades.Count > 3)
            {
                // If the randomly generated id has already been generated, keep trying
                do
                {
                    id = UnityEngine.Random.Range(0, LevelManager.Instance.Upgrades.Count);
                } while (Array.IndexOf(generatedIds, id) != -1);
                if(i < presetUpgradeIds.Count){
                    id = i;
                }
                generatedIds[i] = id;
            }
            else
            {
                generatedIds[i] = i;
            }
        }
        // Add the upgrades corresponding to the selected Ids to the shopItemUpgrades list
        for (int i = 0; i < generatedIds.Length; i++)
        {
            shopItemUpgrades.Add(LevelManager.Instance.Upgrades[generatedIds[i]]);
        }
    }

    // Update the shop item's label to reflect some upgrade's fields
    public void UpdateShopItemLabel(Upgrade upgrade)
    {
        string nameToSearch = upgrade.Name;
        for (int i = 0; i < shopItemUpgrades.Count; i++) {
            if (shopItemUpgrades[i].Name.Equals(nameToSearch))
            {
                shopItemUpgradeGameObjects[i].GetComponent<ShopItem>().MakeFullFormattedTextString();
            }
        }
    }
}

