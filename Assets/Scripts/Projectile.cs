using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject allyWizard;
    [SerializeField]
    private GameObject enemyProjectile;
    [SerializeField]
    private GameObject source;
    private void Start()
    {
        gameObject.SetActive(false);
        
    }
    private void Update()
    {
        if (Mathf.Abs(transform.position.x) > 25f || Mathf.Abs(transform.position.y) > 10f)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetSource(GameObject source)
    {
        this.source = source;
    }

    public GameObject GetSource()
    {
        return source;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetType() == typeof(CapsuleCollider2D) && IsValidTarget(collision))
        {
            gameObject.SetActive(false);
        }
    }

    private bool IsValidTarget(Collider2D collision)
    {
        return !collision.gameObject.CompareTag("ProjectileBlue") && !collision.gameObject.CompareTag("ProjectileGreen") && !(CompareTag("ProjectileBlue")
            && collision.CompareTag("WizardBlue")) && !(CompareTag("ProjectileGreen") && collision.CompareTag("WizardGreen")) 
            && !(CompareTag("ProjectileBlue") && collision.CompareTag("BlueSpawner")) && !(CompareTag("ProjectileGreen") && collision.CompareTag("GreenSpawner"));
    }
}
