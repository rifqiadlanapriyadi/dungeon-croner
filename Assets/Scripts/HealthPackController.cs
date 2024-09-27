using UnityEngine;

public class HealthPackController : PickupController
{
    public int healthCount = 1;

    private PlayerManager _pm;

    protected override void Start()
    {
        base.Start();

        _pm = PlayerManager.instance;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_pm.IsFullHealth())
        {
            _pm.AddHealth(healthCount);
            Deactivate();
        }
    }
}
