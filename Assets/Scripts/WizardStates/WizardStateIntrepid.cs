using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateIntrepid : WizardState
{
    private bool isShotting = false;
    private GameObject attackedTarget;
    private bool isAttacked = false;
    void Start()
    {
        speed = 3.25f;
        wizardManager.SetTargetToRandom(wizardManager.GetAllTargetsByType(gameObject.tag, false));
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
                transform.position = Vector3.MoveTowards(transform.position, wizardManager.GetTarget().transform.position, speed * Time.deltaTime);
            }
            else
            {
                wizardManager.SetTargetToRandom(wizardManager.GetAllTargetsByType(gameObject.tag, false));
            }
        }
        else
        {
            wizardShoot.FireBullet(gameObject);
        }
    }

    public override void RegenWizard()
    {
        RegenerateHealth();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetType() == typeof(CapsuleCollider2D))
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
    }

    public override void ManageStateChange(){}
}