using UnityEngine;

public class AmmoPackController : PickupController
{
    public int ammoCount = 10;

    private PlayerShootController _psc;

    protected override void Start()
    {
        base.Start();

        _psc = PlayerManager.instance.player.GetComponent<PlayerShootController>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_psc.IsFullAmmo())
        {
            _psc.AddAmmo(ammoCount);
            Deactivate();
        }
    }
}
