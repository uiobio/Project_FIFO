using UnityEngine;

public class Shop_room_setup : MonoBehaviour
{
    [Header("Shop")]
    // The ShopItem prefab
    [SerializeField]
    private GameObject ShopItemPrefab;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Context:
        // Vector3(0.371f, 0.288f, 7.126f) represents the top right corner of the large floor square.
        // The ShopItemPrefab position is at the bottom left corner of the large floor square.
        // 0.375f is the width of a single, small floor tile
        // 1.5f is to account for the fact that the position is calculated from the center of each ShopItem (so the expected "distance between" is effectively shorted by 2/3)
        if (wallDirection.Equals("Northwest"))
        {
            for (int i = 0; i < numShopItems; i++)
            {

                GameObject shopItem = Instantiate(ShopItemPrefab, new Vector3(0.371f, 0.288f, 7.126f) + new Vector3(0, 0, -0.375f * 1.5f * i * numSquaresBetweenEachShopItem + -0.375f * numSquaresFirstShopItemFromEdge), Quaternion.identity);
                shopItem.name = shopItem.name + "_" + i.ToString();
            }
        }
        else
        {
            for (int i = 0; i < numShopItems; i++)
            {
                GameObject shopItem = Instantiate(ShopItemPrefab, ShopItemPrefab.transform.position + new Vector3(-0.375f * 1.5f * i * numSquaresBetweenEachShopItem + -0.375f * numSquaresFirstShopItemFromEdge, 0, 0), Quaternion.identity);
                shopItem.name = shopItem.name + "_" + i.ToString();
            }
        }
    }
}
