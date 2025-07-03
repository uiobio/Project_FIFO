using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowDamage : MonoBehaviour
{
    public float Speed;
    public TMP_Text DmgText;

    // Update is called once per frame
    void Update()
    {
        Vector3 p = transform.position;
        transform.position = new Vector3(p.x, p.y + Speed * Time.deltaTime, p.z);
    }

    public void UpdateText(int dmg){
        DmgText.text = "-" + dmg.ToString();
    }

    public void SetColor(Color col){
        DmgText.color = col;
    }
}
