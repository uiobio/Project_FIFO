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

    void Start()
	{
		H = gameObject.GetComponent<Health>();
	}

	void Update()
	{
		if (H.isDead){
			Level_manager.instance.UpdatePattern(element);
			Destroy(gameObject);
		}
		healthBar.eulerAngles = cam_rot;
	}

	public void SetElement(int elem){
		element = elem;
	}
}
