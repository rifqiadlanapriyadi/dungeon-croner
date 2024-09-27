using UnityEngine;

public class BulletAnimationEventHandler : MonoBehaviour
{
    public BulletBehavior bulletBehavior;

    private SpriteRenderer _sprite;
    private Animator _animator;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void OnPuffEnd()
    {
        gameObject.SetActive(false);
        bulletBehavior.OnFinishPuffAnimation();
    }
}
