using UnityEngine;

public class GrenadeAnimationEventHandler : MonoBehaviour
{
    public GrenadeController grenadeController;

    private SpriteRenderer _sprite;
    private Animator _animator;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void ExplosionEnd()
    {
        grenadeController.OnExplisionEnd();
        _sprite.enabled = false;
        _animator.enabled = false;
    }
}
