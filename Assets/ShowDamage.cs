using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowDamage : MonoBehaviour
{
    public float speed;
    public TMP_Text t_dmg;

    // Update is called once per frame
    void Update()
    {
        Vector3 p = transform.position;
        transform.position = new Vector3(p.x, p.y + speed * Time.deltaTime, p.z);
    }

    public void UpdateText(int dmg){
        t_dmg.text = "-" + dmg.ToString();
    }

    public void SetColor(Color col){
        t_dmg.color = col;
    }
}
