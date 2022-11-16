using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateIntrepid : WizardState
{
    private bool isShotting = false;
    private GameObject attackedTarget;
    private bool isAttacked = false;
    private new readonly float speed = 3.25f;
    void Start()
    {
        wizardManager.SetSpeed(speed);
        wizardManager.SetTargetToRandom(wizardManager.GetAllTargetsByType(gameObject.tag, false));
        wizardManager.SetMaxDamage(55);
        spriteRenderer.color = Color.yellow;
    }

    //SI JAMAIS L'UPDATE DE VOTRE ÉTAT DEVIENT DIFFÉRENT DE CELUI DE BASE
    //MASQUER L'UPDATE DE L'ÉTAT DE BASE AVEC VOTRE PROPRE UPDATE.
    //void Update()
    //{
    //    MoveWizard();
    //    ManageStateChange();
    //}

    public override void MoveWizard()
    {
        if (!isShotting)
        {
            if (wizardManager.GetTarget().activeSelf)
            {
                LookAt(wizardManager.GetTarget());
                transform.position = Vector3.MoveTowards(transform.position, wizardManager.GetTarget().transform.position, wizardManager.GetSpeed() * Time.deltaTime);
            }
            else
            {
                wizardManager.SetTargetToRandom(wizardManager.GetAllTargetsByType(gameObject.tag, false));
            }
        }
        else
        {
            wizardShoot.FireBullet(gameObject, false);
        }
    }

    public override void RegenWizard()
    {
        RegenerateHealth();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(innerCollider.Distance(collision).distance < 0.1f && collision.CompareTag("ProjectileBlue") || collision.CompareTag("ProjectileGreen"))
        {
            isAttacked = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.activeInHierarchy && IsGameObjectEnemy(other) && (isAttacked || other.CompareTag("BlueSpawner") || other.CompareTag("GreenSpawner")))
        {
            if (!attackedTarget)
            {
                attackedTarget = other.gameObject;
            }
            LookAt(attackedTarget);
            isShotting = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (IsGameObjectEnemy(other))
        {
            attackedTarget = null;
            isAttacked = false;
            isShotting = false;
        }

        if (innerCollider.Distance(other).distance < 0.1f &&
            other.CompareTag("GreenBush") || other.CompareTag("BlueBush"))
        {
            wizardManager.SetSpeed(speed);
        }
    }

    public override void ManageStateChange(){}
}