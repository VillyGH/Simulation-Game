using UnityEngine;

public class SpawnerController : MonoBehaviour
{
	[SerializeField]
	private GameManager gameManager;

	[SerializeField]
	private int health = 500;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((CompareTag("BlueSpawner") && collision.gameObject.CompareTag("ProjectileGreen")) || (CompareTag("GreenSpawner") && collision.gameObject.CompareTag("ProjectileBlue")))
		{
			Projectile projectile = collision.gameObject.GetComponent<Projectile>();
			health -= projectile.GetSource().GetComponent<WizardManager>().GetDamage();
			CheckIfDie();
		}
	}

	private void CheckIfDie()
	{
		if (health <= 0)
		{
			gameManager.SpawnPointDeactivated(gameObject);
			gameObject.SetActive(false);
		}
	}
}
