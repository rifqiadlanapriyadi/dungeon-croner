using UnityEngine;

public class EnemyAnimationEventHandler : MonoBehaviour
{
    public EnemyController enemyController;

    public void HandleAttackFrame()
    {
        enemyController.HandleAttackFrame();
    }

    public void SetBusy()
    {
        enemyController.Busy = true;
    }

    public void SetNotBusy()
    {
        enemyController.Busy = false;
    }

    public void DeathAnimationFinish()
    {
        enemyController.OnDeathAnimationFinish();
    }
}
