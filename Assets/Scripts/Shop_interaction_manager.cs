using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_interaction_manager : MonoBehaviour
{
    [Header("Shop")]
    [SerializeField]
    private string LabelText = "(E) Purchase";
    [SerializeField]
    private GameObject UILabelPrefab;
    [SerializeField]
    private float[] RotationXYZ = new float[3];
    [SerializeField]
    private float[] PositionOffsetXYZ = new float[3];
    

    private GameObject UILabel;
    private bool IsShopActive = false;
    // Start is called before the first frame update
    void Start()
    {
        UILabel = Instantiate(UILabelPrefab, transform.position, Quaternion.identity);
        UILabel.GetComponent<TextMesh>().text = LabelText;
        UILabel.transform.rotation = Quaternion.Euler(RotationXYZ[0], RotationXYZ[1], RotationXYZ[2]);
        UILabel.transform.position += new Vector3(PositionOffsetXYZ[0], PositionOffsetXYZ[1], PositionOffsetXYZ[2]);
        UILabel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            UILabel.SetActive(true);
            Player_input_manager.instance.setInteractable(gameObject);
            IsShopActive = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player")
        {
            UILabel.SetActive(false);
            IsShopActive = false;
        }
    }

    // Getters, Setters
    public bool getIsShopActive() { 
        return IsShopActive;
    }

    public void setIsShopActive(bool setIsShopActive) { 
        IsShopActive = setIsShopActive;
    }


    public void buy() {
        Debug.Log("Destroyed Shop");
        Destroy(UILabel);
        Destroy(gameObject);   
    }
}
