using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public float walkSpeed = 5f;
    public float runSpeed = 7.5f;

    public Animator torsoAnimator;
    public Animator legsAnimator;

    private Rigidbody2D _rb;

    private Vector2 _movement;
    private Vector2 _mousePos;

    private PlayerManager _pm;
    private InputManager _im;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pm = PlayerManager.instance;
        _im = InputManager.instance;
    }

    private void Update()
    {
        _movement.x = _im.GetAxisRaw("Horizontal");
        _movement.y = _im.GetAxisRaw("Vertical");

        _mousePos = cam.ScreenToWorldPoint(_im.MousePosition());

        bool moving = _movement != Vector2.zero;
        torsoAnimator.SetBool("Moving", moving);
        legsAnimator.SetBool("Moving", moving);
    }

    private void FixedUpdate()
    {
        float speed = _pm.running ? runSpeed : walkSpeed;
        _rb.MovePosition(_rb.position + _movement.normalized * speed * Time.fixedDeltaTime);

        Vector2 lookDir = _mousePos - _rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        _rb.rotation = angle;
    }
}
