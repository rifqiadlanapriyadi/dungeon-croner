using UnityEngine;

public class PlayerMeleeController : MonoBehaviour
{
    public GameObject meleePoint;
    public float meleeRadius = .6f;
    public LayerMask targetLayers;

    public Animator animator;

    public AudioClip[] hitSounds;
    public AudioClip[] killHitSounds;
    public AudioClip[] missSounds;
    private AudioSource _as;

    private PlayerManager _pm;
    private InputManager _im;

    private void Start()
    {
        _pm = PlayerManager.instance;
        _im = InputManager.instance;

        _as = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_im.GetKeyDown(KeyCode.Space) && !_pm.busy)
            animator.SetTrigger("Melee");
    }

    public void DoMeleeEffect()
    {
        Collider2D[] targetsHit = Physics2D.OverlapCircleAll(meleePoint.transform.position, meleeRadius, targetLayers);
        if (targetsHit.Length == 0) PlayRandomSound(missSounds);
        foreach (var targetCollider in targetsHit)
        {
            if (targetCollider.CompareTag("Enemy"))
            {
                EnemyController enemyController = targetCollider.gameObject.GetComponent<EnemyController>();
                if (enemyController.Health == 1) PlayRandomSound(killHitSounds);
                else PlayRandomSound(hitSounds);
                enemyController.HandleHit(1);
            } else if (targetCollider.CompareTag("Destructible")) Destroy(targetCollider.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (meleePoint == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(meleePoint.transform.position, meleeRadius);
    }
    private void PlayRandomSound(AudioClip[] sounds)
    {
        _as.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }
}
