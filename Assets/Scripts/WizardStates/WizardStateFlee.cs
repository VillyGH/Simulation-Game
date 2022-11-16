using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateFlee : WizardState
{
    private new readonly float speed = 3.25f;
    void Start()
    {
        wizardManager.SetSpeed(speed);
        wizardManager.SetTargetToClosest(wizardManager.GetAllTargetsByType(gameObject.tag, true));
    }

    public override void MoveWizard()
    {
        LookAt(wizardManager.GetTarget());
        transform.position = Vector3.MoveTowards(transform.position, wizardManager.GetTarget().transform.position, wizardManager.GetSpeed() * Time.deltaTime);
    }

    public override void RegenWizard()
    {
        RegenerateHealth();
    }

    public override void ManageStateChange()
    {
        if (Vector2.Distance(transform.position, wizardManager.GetTarget().transform.position) < 0.2f)
        {
            if (wizardManager.GetTarget().CompareTag("BlueBush") || wizardManager.GetTarget().CompareTag("GreenBush"))
            {
                wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.Hide);
            }
            else
            {
                wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.Safety);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (innerCollider.Distance(other).distance < 0.1f &&
            other.CompareTag("GreenBush") || other.CompareTag("BlueBush"))
        {
            wizardManager.SetSpeed(speed);
        }
    }
}
