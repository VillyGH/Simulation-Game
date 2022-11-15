using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateSafety : WizardState
{
    void Start()
    {
        regenerationRythm = 0.3f;
    }
    public override void MoveWizard(){}

    public override void RegenWizard()
    {
        RegenerateHealth();
    }

    public override void ManageStateChange()
    {
        if(wizardManager.GetHealth() >= wizardManager.GetBaseHealth() || !wizardManager.GetTarget().activeSelf)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.Normal);
        }
    }

}
