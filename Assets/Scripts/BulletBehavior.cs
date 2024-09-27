using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float bulletSpeed = 10f;

    private Vector3 _initialPos;
    private bool _withMaxDistance;
    private float _maxDistance;

    public Animator animator;
    private bool _puffAnimationFinished;

    public AudioClip[] hitSounds;
    public AudioClip[] ricochetSounds;
    private AudioSource _as;
    private float _finishSoundTime;

    private Rigidbody2D _rb;
    private Collider2D _coll;

    private BulletPool _bp;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _as = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _rb.velocity = transform.up * bulletSpeed;
        _initialPos = transform.position;
        _finishSoundTime = Time.time;
        _puffAnimationFinished = false;
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
    }

    private void Start()
    {
        _bp = BulletPool.instance;
    }

    private void Update()
    {
        bool finishPuff = _puffAnimationFinished && Time.time >= _finishSoundTime;
        bool reachedMaxDistance = _withMaxDistance && (transform.position - _initialPos).magnitude >= _maxDistance;
        if (finishPuff || reachedMaxDistance) _bp.EnqueueToPool(gameObject);

    }

    public void SetMaxDistance(bool withMaxDistance, float maxDistance)
    {
        _withMaxDistance = withMaxDistance;
        _maxDistance = maxDistance;
    }

    public void OnFinishPuffAnimation()
    {
        _puffAnimationFinished = true;
    }

    public void SetColliderBodyActive(bool active)
    {
        _coll.enabled = active;
        _rb.simulated = active;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Object") || collision.CompareTag("Enemy") || collision.CompareTag("Destructible"))
        {
            if (collision.CompareTag("Enemy")) PlayRandomSound(hitSounds);
            else PlayRandomSound(ricochetSounds);
            animator.SetTrigger("Puff");
            SetColliderBodyActive(false);
        }
        if (collision.CompareTag("Enemy")) {
            EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
            enemyController.HandleHit(1);
        } else if (collision.CompareTag("Destructible"))
            Destroy(collision.gameObject);
    }

    private void PlayRandomSound(AudioClip[] sounds)
    {
        AudioClip clip = sounds[Random.Range(0, sounds.Length)];
        _as.PlayOneShot(clip);
        _finishSoundTime = Mathf.Max(_finishSoundTime, Time.time + clip.length);
    }
}
