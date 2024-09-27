using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    private PlayerManager _pm;
    private PlayerShootController _psc;
    private PlayerMeleeController _pmc;
    private PlayerGrenadeController _pgc;
    private PlayerWeaponSwapController _pwsc;
    private PlayerReloadController _prc;
    private PlayerDeathController _pdc;

    private void Start()
    {
        _pm = PlayerManager.instance;
        _psc = _pm.player.GetComponent<PlayerShootController>();
        _pmc = _pm.player.GetComponent<PlayerMeleeController>();
        _pgc = _pm.player.GetComponent<PlayerGrenadeController>();
        _pwsc = _pm.player.GetComponent<PlayerWeaponSwapController>();
        _prc = _pm.player.GetComponent<PlayerReloadController>();
        _pdc = _pm.player.GetComponent<PlayerDeathController>();
    }

    public void SetBusy()
    {
        _pm.busy = true;
    }

    public void SetNotBusy()
    {
        _pm.busy = false;
    }

    public void SetNotRunning()
    {
        _pm.running = false;
    }

    public void PistolShoot()
    {
        _psc.Shoot();
    }

    public void MeleeHit()
    {
        _pmc.DoMeleeEffect();
    }

    public void SetReloading()
    {
        _prc.SetReloading(true);
    }

    public void SetNotReloading()
    {
        _prc.SetReloading(false);
    }

    public void ReloadPistol()
    {
        _psc.ReloadPistol();
    }

    public void ReloadRifle()
    {
        _psc.ReloadRifle();
    }

    public void ThrowGrenade()
    {
        _pgc.ThrowGrenade();
    }

    public void ToggleWeapon()
    {
        _pwsc.ToggleWeapon();
    }

    public void DeathAnimationFinish()
    {
        _pdc.OnDeathAnimationFinish();
    }
}
