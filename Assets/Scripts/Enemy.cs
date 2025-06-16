using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField]
	private Health health;
	[SerializeField]
	private Transform healthBar;
	[SerializeField]
	public Vector3 cam_rot;
	[SerializeField]
	private int element;
	[SerializeField]
	private int num_chips;
	[SerializeField]
	private SpriteRenderer SR;

	[SerializeField]
	Color[] ElementColors = new Color[] { Color.yellow, Color.red, Color.cyan, Color.green };

	[SerializeField]
	private Animator anim;

	void Start()
	{
		health = gameObject.GetComponent<Health>();
	}

	void Update()
	{
		if (health.IsDead)
		{
			Debug.Log($"ENEMY {gameObject.name} HAS DIED!!!");
			LevelManager.Instance.UpdatePattern(element);
			LevelManager.Instance.GainCoin(num_chips);
			Destroy(gameObject);
		}
		healthBar.eulerAngles = cam_rot;
	}

	public void SetElement(int elem)
	{
		//Sets the element variable and sets animator (if exists) or sprite color
		element = elem;
		anim = gameObject.GetComponentInChildren<Animator>();
		if (anim != null)
		{
			SR.color = Color.white;
			anim.SetInteger("Element", elem);
			anim.SetTrigger("SetElement");
		}
		else
		{
			SR.color = ElementColors[elem];
		}
	}
}
