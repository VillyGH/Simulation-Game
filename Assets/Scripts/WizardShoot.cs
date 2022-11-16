using UnityEngine;

public class WizardShoot : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D projectile;

    private const int initMaxMunitions = 75;

    private int maxMunitions = initMaxMunitions;
    private readonly Rigidbody2D[] munitions = new Rigidbody2D[initMaxMunitions];

    private float rateOfFire = 1.5f;

    private float rateOfFireTime;

    private void Awake()
    {
        for (int i = 0; i < initMaxMunitions; i++)
        {
            munitions[i] = Instantiate(projectile);
            munitions[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        ManageBullets();
    }

    private void ManageBullets()
    {
        if (rateOfFireTime > 0f)
        {
            rateOfFireTime -= Time.deltaTime;
        }
    }


    public void FireBullet(GameObject source, bool upgraded)
    {
        if(upgraded)
        {
            rateOfFire = 1.25f;
        }
        if (rateOfFireTime <= 0f)
        {
            for (int i = 0; i < maxMunitions; i++)
            {
                if (!munitions[i].gameObject.activeInHierarchy)
                {
                    munitions[i].gameObject.SetActive(true);
                    munitions[i].GetComponent<Projectile>().SetSource(source);
                    munitions[i].transform.position = transform.position + transform.up;
                    munitions[i].AddForce(transform.up * 5f, ForceMode2D.Impulse);
                    rateOfFireTime = rateOfFire;
                    break;
                }
                if (i == maxMunitions)
                {
                    maxMunitions += 10;
                }
            }
        }
    }
}
