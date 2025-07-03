using UnityEngine;

public class TrashcanCollider : MonoBehaviour
{   
    // Called when another collider enters the trigger hitbox
    private void OnTriggerEnter(Collider other)
    {
        Trashcan trashcan = transform.parent.GetComponent<Trashcan>();
        LevelManager levelManager = LevelManager.Instance.GetComponent<LevelManager>();
        if (levelManager.CurrentlySelectedUpgradeIndex >= levelManager.PlayerHeldUpgrades.Count)
        {
            levelManager.CurrentlySelectedUpgradeIndex = levelManager.PlayerHeldUpgrades.Count - 1;
        }
        // If the other collider is the player, and the player has at least one upgrade:
        if (other.gameObject.CompareTag("Player") && levelManager.PlayerHeldUpgrades.Count >= 1)
        {
            LevelManager.Instance.RecyclePlayerUpgrade(transform.parent.gameObject);
            trashcan.Upgrade = levelManager.PlayerHeldUpgrades[levelManager.CurrentlySelectedUpgradeIndex];
            trashcan.UILabel.SetActive(true);
            trashcan.UILabel.GetComponent<UpgradeLabel>().SetNewUpgrade(trashcan.Upgrade);
            trashcan.UILabel.GetComponent<UpgradeLabel>().ChangeLabelTextBasedOnGameState(trashcan.Upgrade);
            trashcan.MakeFullFormattedTextString();
            trashcan.IsTrashcanActive = true;
            // Update the TextMeshPro component according to the new active text
            PlayerInputManager.Instance.Interactable = transform.parent.gameObject;
            trashcan.DrawLinesToLabelCorners();
            
        }
    }

    // Called when another collider exits the trigger hitbox
    private void OnTriggerExit(Collider other)
    {
        Trashcan trashcan = transform.parent.GetComponent<Trashcan>();
        LevelManager levelManager = LevelManager.Instance.GetComponent<LevelManager>();
        // If the other collider is the player, and the item has not already been bought:
        // Make the UI label invisible and disallow the player to interact with this Instance's gameObject; make the lines invisble
        if (other.gameObject.CompareTag("Player") && levelManager.PlayerHeldUpgrades.Count >= 1)
        {
            trashcan.UILabel.SetActive(false);
            trashcan.IsTrashcanActive = false;
            trashcan.DrawLinesToLabelCorners();
        }
    }
}
