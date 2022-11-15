using UnityEngine;

public class WizardStateHide : WizardState
{
    private bool isShotting = false;
    private GameObject attackedTarget;

    void Start()
    {
        regenerationRythm = 0.3f;
    }

    public override void MoveWizard()
    {
        if (isShotting)
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
        if (wizardManager.GetHealth() == wizardManager.GetBaseHealth())
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.Normal);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.activeInHierarchy && IsGameObjectEnemy(other) && wizardManager.GetHealth() >= wizardManager.GetBaseHealth() * 65 / 100)
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
