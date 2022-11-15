using UnityEngine;

public class WizardStateHide : WizardState
{
    private bool isShotting = false;
    private GameObject attackedTarget;
    private bool healed;

    void Start()
    {
        regenerationRythm = 0.3f;
        healed = false;
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
        if (!isShotting && gameObject.activeSelf)
        {
            regenerationTime += Time.deltaTime;
            if (regenerationTime > regenerationRythm)
            {
                wizardManager.Heal(10);
                regenerationTime = 0;
                healed = true;
            }
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
        if (wizardManager.GetHealth() < wizardManager.GetBaseHealth() / 4 && healed)
        {
            wizardManager.ChangeWizardState(WizardManager.WizardStateToSwitch.Flee);
            wizardManager.SetTargetToRemoveFromPool();
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
