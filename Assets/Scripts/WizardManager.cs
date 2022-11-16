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

    [SerializeField]
    private float speed = 1.0f;

    private bool removeBushes;

    public enum WizardStateToSwitch { Normal, Intrepid, Flee, Hide, Safety }

    private WizardState wizardState;

    private CapsuleCollider2D innerCollider;

    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        gameObject.SetActive(false);
        wizardState = GetComponent<WizardState>();
        innerCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        kills = 0;
        spriteRenderer.color = Color.white;
        health = baseHealth;
        ChangeWizardState(WizardStateToSwitch.Normal);
    }

    public void ChangeWizardState(WizardStateToSwitch nextState)
    {
        Destroy(wizardState);

        switch (nextState)
        {
            case WizardStateToSwitch.Normal:
                {
                    wizardState = gameObject.AddComponent<WizardStateNormal>();
                    if (logStateSwitch)
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
                        Debug.Log("Changement de state pour fuite");
                    }
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (innerCollider.Distance(collision).distance < 0.1f && 
            (CompareTag("WizardGreen") && collision.CompareTag("ProjectileBlue") || (CompareTag("WizardBlue") && collision.CompareTag("ProjectileGreen"))))
        {
            health -= GetDamage();
            if (health <= 0)
            {
                Die(collision.gameObject.GetComponent<Projectile>());
                health = 0;
            }
        }
        if (innerCollider.Distance(collision).distance < 0.1f &&
            CompareTag("WizardGreen") && collision.CompareTag("GreenBush") || (CompareTag("WizardBlue") && collision.CompareTag("BlueBush")))
        {
            speed += 1f;
        }
        else if (innerCollider.Distance(collision).distance < 0.1f &&
          CompareTag("WizardGreen") && collision.CompareTag("BlueBush") || (CompareTag("WizardBlue") && collision.CompareTag("GreenBush")))
        {
            speed -= 0.5f;
        }
    }

    

    public int GetDamage()
    {
        return Random.Range(1, maxDamage);
    }

    public void Die(Projectile projectile)
    {
        projectile.GetSource().GetComponent<WizardManager>().AddKill();
        gameManager.ChangeEnemyCount(tag, -1);
        Destroy(wizardState);
        gameObject.SetActive(false);
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public List<GameObject> GetAllTargetsByType(string tag, bool cover)
    {
        List<GameObject> targets = gameManager.GetAllTargetsByType(tag, cover);
        if (removeBushes)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if(targets[i].CompareTag("BlueBush") || targets[i].CompareTag("GreenBush"))
                {
                    targets.Remove(targets[i]);
                }
            }
        }
        return targets;
    }

    public void SetTargetToRemoveFromPool()
    {
        removeBushes = true;
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
        removeBushes = false;
    }

    public void SetTargetToRandom(List<GameObject> targets)
    {
        if (targets.Count == 0)
        {
            target = gameManager.GetOneOfLastEnemiesAlive(tag);
            if (!target)
            {
                target = new GameObject();
            }
            return;
        }

        target = targets[Random.Range(0, targets.Count - 1)];
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
        if (health > baseHealth)
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

    public void SetMaxDamage(int maxDamage)
    {
        this.maxDamage = maxDamage;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public float GetSpeed()
    {
        return this.speed;
    }
}