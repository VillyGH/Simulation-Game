using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateNormal : WizardState
{
    private bool isShotting = false;
    private GameObject attackedTarget;
    void Start()
    {
        speed = 2.5f;
    }

    private void OnEnable()
    {
        wizardManager.SetTargetToRandom(wizardManager.GetAllTargetsByType(gameObject.tag, false));
    }

    public override void MoveWizard()
    {
        if (!isShotting)
        {
            if(wizardManager.GetTarget().activeSelf)
            {
                LookAt(wizardManager.GetTarget());
                transform.position = Vector3.MoveTowards(transform.position, wizardManager.GetTarget().transform.position, speed * Time.deltaTime);
            } else
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
        if (!isShotting)
        {
            RegenerateHealth();
        }
    }

    public override void ManageStateChange()
    {
        if (wizardManager.GetKills() >= 3)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.Intrepid);
        }
        if (wizardManager.GetHealth() < wizardManager.GetBaseHealth() / 4)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.Flee);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.activeInHierarchy && IsGameObjectEnemy(other))
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
            isShotting = false;
        }
    }
}