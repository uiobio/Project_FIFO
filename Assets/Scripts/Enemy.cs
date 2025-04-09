using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	private Health H;
	[SerializeField] 
	private Transform healthBar;
	[SerializeField]
	public Vector3 cam_rot;

    void Start()
	{
		H = gameObject.GetComponent<Health>();
	}

	void Update()
	{
		if (H.isDead){
			Destroy(gameObject);
		}
		healthBar.eulerAngles = cam_rot;
	}
}
