using UnityEngine;

public class GrenadePickupController : PickupController
{
    public int grenadeCount = 1;

    private PlayerGrenadeController _pgc;

    protected override void Start()
    {
        base.Start();

        _pgc = PlayerManager.instance.player.GetComponent<PlayerGrenadeController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_pgc.IsFullGrenades())
        {
            _pgc.AddGrenades(grenadeCount);
            Deactivate();
        }
    }
}
