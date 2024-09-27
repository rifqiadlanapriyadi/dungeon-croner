using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    public float timer = 2f;
    private float _explodeTime;

    public float explosionRadius = 1f;

    public LayerMask targetLayers;
    public LayerMask wallLayer;

    private Rigidbody2D _rb;
    private Vector3 _initialPos;
    private bool _withMaxDistance;
    private float _maxDistance;
    private bool _hasExploded;

    public Animator animator;
    private bool _explosionAnimationFinished;

    public AudioClip[] landSounds;
    public AudioClip[] explosionSounds;
    private AudioSource _as;
    private float _finishSoundTime;

    private PlayerManager _pm;

    private void OnEnable()
    {
        _withMaxDistance = false;
        _maxDistance = 0f;
    }

    private void Start()
    {
        _as = GetComponent<AudioSource>();
        _finishSoundTime = Time.time;

        _pm = PlayerManager.instance;

        _explodeTime = Time.time + timer;
        _explosionAnimationFinished = false;

        _rb = GetComponent<Rigidbody2D>();
        _initialPos = transform.position;
    }

    private void Update()
    {
        if (_explosionAnimationFinished && Time.time >= _finishSoundTime) Destroy(gameObject);

        if (_withMaxDistance && (transform.position - _initialPos).magnitude >= _maxDistance)
        {
            _rb.velocity = Vector2.zero;
            _withMaxDistance = false;
            PlayRandomSound(landSounds);
        }

        if (!_hasExploded && Time.time >= _explodeTime)
        {
            Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayers);
            foreach (var targetCollider in hitTargets)
            {
                Vector2 difference = targetCollider.transform.position - transform.position;
                float raycastDistance = Mathf.Min(difference.magnitude, explosionRadius);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, difference, raycastDistance, wallLayer);
                if (hit.collider == null)
                {
                    if (targetCollider.CompareTag("Enemy"))
                        targetCollider.gameObject.GetComponent<EnemyController>().HandleHit(4);
                    else if (targetCollider.CompareTag("Player"))
                        _pm.HandleHit();
                    else if (targetCollider.CompareTag("Destructible"))
                        Destroy(targetCollider.gameObject);
                }
            }

            animator.SetTrigger("Explode");
            _hasExploded = true;
            PlayRandomSound(explosionSounds);
        }
    }

    public void SetMaxDistance(float maxDistance)
    {
        _withMaxDistance = true;
        _maxDistance = maxDistance;
    }

    public void OnExplisionEnd()
    {
        _explosionAnimationFinished = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void PlayRandomSound(AudioClip[] sounds)
    {
        AudioClip clip = sounds[Random.Range(0, sounds.Length)];
        _as.PlayOneShot(clip);
        _finishSoundTime = Mathf.Max(_finishSoundTime, Time.time + clip.length);
    }
}
