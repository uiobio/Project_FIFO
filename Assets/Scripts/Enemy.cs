using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Vector3 CameraRotation;
    [SerializeField] private Health health;
	[SerializeField] private Transform healthBar;
	[SerializeField] private int element;
	[SerializeField] private int numChips;
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private Color[] ElementColors = new Color[] { Color.yellow, Color.red, Color.cyan, Color.green };
    [SerializeField] private Animator animator;

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
			LevelManager.Instance.GainCoin(numChips);
			Destroy(gameObject);
		}
		healthBar.eulerAngles = CameraRotation;
	}

	public void SetElement(int elem)
	{
		//Sets the element variable and sets animator (if exists) or sprite color
		element = elem;
		animator = gameObject.GetComponentInChildren<Animator>();
		if (animator != null)
		{
			spriteRenderer.color = Color.white;
			animator.SetInteger("Element", elem);
			animator.SetTrigger("SetElement");
		}
		else
		{
			spriteRenderer.color = ElementColors[elem];
		}
	}
}
