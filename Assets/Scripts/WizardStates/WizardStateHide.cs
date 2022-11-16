using UnityEngine;

public class WizardStateHide : WizardState
{
    private bool isShotting = false;
    private bool healed;

    void Start()
    {
        regenerationRythm = 0.6f;
        healed = false;
        wizardManager.SetMaxDamage(55);
    }

    public override void MoveWizard()
    {
        if (isShotting)
        {
            wizardShoot.FireBullet(gameObject, true);
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
        if (wizardManager.GetHealth() == wizardManager.GetBaseHealth() || (wizardManager.GetHealth() >= 65 / 100 && isShotting))
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
        if (other.gameObject.activeInHierarchy && IsGameObjectEnemy(other))
        {
            LookAt(other.gameObject);
            isShotting = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (IsGameObjectEnemy(other))
        {
            isShotting = false;
        }
    }
}
