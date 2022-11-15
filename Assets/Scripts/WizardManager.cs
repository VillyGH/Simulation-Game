using System.Collections.Generic;
using UnityEngine;

public class WizardManager : MonoBehaviour
{
	[SerializeField]
	private GameManager gameManager;

	[SerializeField]
	private GameObject target;

	[SerializeField]
	private int health = 200;

	[SerializeField]
	private int baseHealth = 200;

	[SerializeField]
	private int maxDamage = 50;

	[SerializeField]
	private int kills;

	[SerializeField]
	private bool logStateSwitch = false;

	public enum WizardStateToSwitch { Normal, Intrepid, Flee, Hide, Safety }

	private WizardState wizardState;

	public void Awake()
	{
		gameObject.SetActive(false);
		wizardState = GetComponent<WizardState>();
	}

	private void OnEnable()
	{
		health = baseHealth;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		//
	}

	public void ChangeWizardState(WizardStateToSwitch nextState)
	{
		Destroy(wizardState);

		switch (nextState)
		{
			case WizardStateToSwitch.Normal:
				{
					wizardState = gameObject.AddComponent<WizardStateNormal>();
					if(logStateSwitch)
					{
						Debug.Log("Changement de state pour normal");
					}
					break;
				}
			case WizardStateToSwitch.Intrepid:
				{
					wizardState = gameObject.AddComponent<WizardStateIntrepid>();
					if (logStateSwitch)
					{
						Debug.Log("Changement de state pour intrépide");
					}
					break;
				}
			case WizardStateToSwitch.Flee:
				{
					wizardState = gameObject.AddComponent<WizardStateFlee>();
					if (logStateSwitch)
					{

					}
					Debug.Log("Changement de state pour fuite");
					break;
				}
			case WizardStateToSwitch.Hide:
				{
					wizardState = gameObject.AddComponent<WizardStateHide>();
					if (logStateSwitch)
					{
						Debug.Log("Changement de state pour cachette");
					}
					break;
				}
			case WizardStateToSwitch.Safety:
				{
					wizardState = gameObject.AddComponent<WizardStateSafety>();
					if (logStateSwitch)
					{
						Debug.Log("Changement de state pour sureté");
					}
					break;
				}
		}
	}

	private void Update()
	{
		if (Mathf.Abs(transform.position.x) > 25f || Mathf.Abs(transform.position.y) > 12f)
		{
			gameObject.SetActive(false);
		}
		if (kills >= 3)
		{
			ChangeWizardState(WizardStateToSwitch.Intrepid);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((collision.transform.CompareTag("ProjectileBlue") && CompareTag("WizardGreen")) || (collision.transform.CompareTag("ProjectileGreen") && CompareTag("WizardBlue")) && GetComponent<CapsuleCollider2D>().IsTouching(collision))
		{
			health -= GetDamage();
			if (health <= 0)
			{
				Die(collision.gameObject.GetComponent<Projectile>());
				health = 0;
			}
		}
	}

	public int GetDamage()
	{
		return Random.Range(1, maxDamage);
	}
	
	public void Die(Projectile projectile)
	{
		gameObject.SetActive(false);
		projectile.GetSource().GetComponent<WizardManager>().AddKill();
		gameManager.DecreaseEnemyCount(tag);
		ChangeWizardState(WizardStateToSwitch.Normal);
	}

	public GameObject GetTarget()
	{
		return target;
	}

	public List<GameObject> GetAllTargetsByType(string tag, bool cover)
	{
		return gameManager.GetAllTargetsByType(tag, cover);
	}

	public void SetTargetToClosest(List<GameObject> targets)
	{
		GameObject closestTarget = null;
		float smallestDistance = float.MaxValue;
		foreach (GameObject targetObj in targets)
		{
			float newDistance = Vector2.Distance(transform.position, targetObj.transform.position);
			if (smallestDistance > newDistance)
			{
				smallestDistance = newDistance;
				closestTarget = targetObj;
			}
		}
		target = closestTarget;
	}

	public void SetTargetToRandom(List<GameObject> targets)
	{
		if(targets != null)
		{
			target = targets[Random.Range(0, targets.Count - 1)];
		}
	}
	public void AddKill()
	{
		kills++;
	}

	public int GetKills()
	{
		return kills;
	}

	public void Heal(int healing)
	{
		health = health += healing;
		if(health > baseHealth)
		{
			health = baseHealth;
		}
	}

	public int GetHealth()
	{
		return health;
	}

	public int GetBaseHealth()
	{
		return baseHealth;
	}
}