using UnityEngine;

public class PlayerRunController : MonoBehaviour
{
    public Animator torsoAnimator;
    public Animator legsAnimator;

    private PlayerManager _pm;
    private InputManager _im;

    private void Start()
    {
        _pm = PlayerManager.instance;
        _im = InputManager.instance;
    }

    private void Update()
    {
        if (_im.GetKeyDown(KeyCode.LeftShift))
            _pm.running = !_pm.running;
        torsoAnimator.SetBool("Running", _pm.running);
        legsAnimator.SetBool("Running", _pm.running);
    }
}
