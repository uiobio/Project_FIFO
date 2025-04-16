using UnityEngine;

public class ShopItemCollider : MonoBehaviour
{
    // Called when another collider enters the trigger hitbox
    private void OnTriggerEnter(Collider other)
    {
        ShopItem shopItem = transform.parent.GetComponent<ShopItem>();
        // If the other collider is the player, and the item has not already been bought:
        // Make the UI label visible, allow the player to interact with this instance's gameObject, draw the lines
        // Update the text to reflect changes in game state
        if (other.gameObject.CompareTag("Player") && !shopItem.Bought)
        {
            shopItem.Label.GetComponent<UpgradeLabel>().ChangeLabelTextBasedOnGameState(shopItem.upgrade);
            shopItem.MakeFullFormattedTextString();
            shopItem.Label.SetActive(true);
            // Update the TextMeshPro component according to the new active text
            Player_input_manager.instance.Interactable = transform.parent.gameObject;
            shopItem.IsShopActive = true;
            shopItem.DrawLinesToLabelCorners();
        }
    }

    // Called when another collider exits the trigger hitbox
    private void OnTriggerExit(Collider other)
    {
        ShopItem shopItem = transform.parent.GetComponent<ShopItem>();
        // If the other collider is the player, and the item has not already been bought:
        // Make the UI label invisible and disallow the player to interact with this instance's gameObject; make the lines invisble
        if (other.gameObject.CompareTag("Player") && !shopItem.Bought)
        {
            shopItem.Label.SetActive(false);
            shopItem.IsShopActive = false;
            shopItem.DrawLinesToLabelCorners();
        }
    }
}
