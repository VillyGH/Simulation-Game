using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WizardState : MonoBehaviour
{
    //Attention: l'état ne cesse de changer: donc à chaque fois que vous 
    //changez d'état: tout ceci sera réinitialisé.
    protected WizardManager wizardManager;
    protected WizardShoot wizardShoot;

    protected float speed;
    protected float regenerationRythm = 1f;
    protected float regenerationTime = 0f;

    //Attention: les méthodes de la boucle de jeu de Unity:
    //méthodes privé: va être appellé ici, mais aussi dans la sous-classe
    //Soyez très prudent avant de mettre du code ici
    //Il faudrait vraiment que ce soit universel à tous les états.
    void Awake()
    {
        wizardManager = GetComponent<WizardManager>();
        wizardShoot = GetComponent<WizardShoot>();
    }

    void Update()
    {
        MoveWizard();
        RegenWizard();
        ManageStateChange();
    }

    public void RegenerateHealth()
    {
        regenerationTime += Time.deltaTime;
        if (regenerationTime > regenerationRythm)
        {
            wizardManager.Heal(10);
            regenerationTime = 0;
        }
    }

    public void LookAt(GameObject target)
    {
        Vector3 relative = transform.InverseTransformPoint(target.transform.position);
        float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
        transform.Rotate(0, 0, -angle);
    }
    public bool IsGameObjectEnemy(Collider2D other)
    {
        return (CompareTag("WizardGreen") && other.CompareTag("WizardBlue")) || (CompareTag("WizardBlue") && other.CompareTag("WizardGreen"))
            || (CompareTag("WizardGreen") && other.CompareTag("BlueSpawner")) || (CompareTag("WizardBlue") && other.CompareTag("GreenSpawner"));
    }

    public abstract void MoveWizard();
    public abstract void RegenWizard();
    public abstract void ManageStateChange();

}
