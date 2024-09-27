using UnityEngine;

public class PlayerInvincibilityController : MonoBehaviour
{
    public float invincibilityDuration = 2f;
    public float blinkOn = 0.2f;
    public float blinkOff = 0.1f;

    public SpriteRenderer torsoSprite;
    public SpriteRenderer legsSprite;

    private float _until;
    private float _toggle;

    public PlayerManager _pm;

    private void Start()
    {
        _until = Time.time;
        _toggle = Time.time;

        _pm = PlayerManager.instance;
    }

    private void Update()
    {
        UpdateInvincibility();
    }

    public void StartInvincibility()
    {
        _pm.IsInvincible = true;
        _until = Time.time + invincibilityDuration;
        _toggle = Time.time + blinkOn;
    }

    private void UpdateInvincibility()
    {
        if (_pm.IsInvincible)
        {
            if (Time.time >= _until) EndInvincibility();
            else UpdateBlink();
        }
    }

    private void UpdateBlink()
    {
        if (Time.time >= _toggle)
        {
            bool nextBlinkState = !torsoSprite.enabled;
            torsoSprite.enabled = nextBlinkState;
            legsSprite.enabled = nextBlinkState;
            _toggle = Time.time + (nextBlinkState ? blinkOn : blinkOff);
        }
    }

    private void EndInvincibility()
    {
        _pm.IsInvincible = false;
        torsoSprite.enabled = true;
        legsSprite.enabled = true;
    }
}
