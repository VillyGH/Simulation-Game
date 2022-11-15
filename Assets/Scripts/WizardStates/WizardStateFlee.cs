using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateFlee : WizardState
{
    // Start is called before the first frame update
    void Start()
    {
        speed = 4f;
        wizardManager.SetTargetToClosest(wizardManager.GetAllTargetsByType(gameObject.tag, true));
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
        LookAt(wizardManager.GetTarget());
        transform.position = Vector3.MoveTowards(transform.position, wizardManager.GetTarget().transform.position, speed * Time.deltaTime);
    }

    public override void RegenWizard()
    {
        regenerationTime += Time.deltaTime;
        if (regenerationTime > regenerationRythm)
        {
            wizardManager.Heal(10);
            regenerationTime = 0;
        }
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
}
