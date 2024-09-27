using UnityEngine;

public class PlayerReloadController : MonoBehaviour
{
    public Animator animator;

    private PlayerManager _pm;
    private InputManager _im;

    private void Start()
    {
        _pm = PlayerManager.instance;
        _im = InputManager.instance;
    }

    private void Update()
    {
        if (_im.GetKeyDown(KeyCode.R) && !_pm.busy && !_pm.reloading)
            animator.SetTrigger("Reload");
    }

    public void SetReloading(bool reloading)
    {
        _pm.reloading = reloading;
    }
}
