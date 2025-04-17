using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	private Health H;
	[SerializeField] 
	private Transform healthBar;
	[SerializeField]
	public Vector3 cam_rot;
	[SerializeField]
	private int element;
	[SerializeField]
	private int num_chips;

    void Start()
	{
		H = gameObject.GetComponent<Health>();
	}

	void Update()
	{
		if (H.isDead){
			Debug.Log($"ENEMY {gameObject.name} HAS DIED!!!");
			Level_manager.instance.UpdatePattern(element);
			Level_manager.instance.GainCoin(num_chips);
			Destroy(gameObject);
		}
		healthBar.eulerAngles = cam_rot;
	}

	public void SetElement(int elem){
		element = elem;
	}
}
