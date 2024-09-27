using Pathfinding;
using UnityEngine;

public enum EnemyState
{
    Patrol,
    Chase,
    Attack
}

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    public float enemyAttackPeriod = 3f;
    public float enemyAttackRadius = 2f;
    public float sightDistance = 5f;
    public LayerMask sightCastMask;
    public Animator torsoAnimator;
    public Animator legsAnimator;

    public int startingHealth = 4;
    private int _health;
    public int Health
    {
        get { return _health; }
        private set { _health = value; }
    }

    public float patrolPeriod = 5f;
    public float patrolPeriodBuffer = 3f;
    public float patrolRadius = 4f;
    private GameObject _targetPatrolPosition;
    private float _nextPatrolTime;
    private Vector2 _patrolPosition;

    public bool hostile = true;
    public bool respawn = false;
    public float respawnTimer = 2f;

    private float _nextAttackTime;

    private PlayerManager _pm;
    private GameManager _gm;

    private Collider2D _coll;

    private AIPath _aiPath;
    private Seeker _seeker;
    private AIDestinationSetter _destinationSetter;

    public AudioClip[] hitAttackSounds;
    public AudioClip[] missedAttackSounds;
    public AudioClip[] deathSounds;
    private AudioSource _as;

    public float walkSpeed = 3f;
    public float runSpeed = 5f;

    private EnemyState _state;
    private bool _running;
    public bool Running
    {
        get { return _running; }
        private set {
            torsoAnimator.SetBool("Running", value);
            legsAnimator.SetBool("Running", value);
            _running = value; 
        }
    }
    public float rageDuration = 2f;
    private float _rageUntil;

    private bool _busy;
    public bool Busy
    {
        get { return _busy; }
        set { _busy = value; }
    }

    private void Start()
    {
        _aiPath = GetComponent<AIPath>();
        _seeker = GetComponent<Seeker>();
        _destinationSetter = GetComponent<AIDestinationSetter>();

        _patrolPosition = transform.position;
        _targetPatrolPosition = new GameObject("Target Position");
        _targetPatrolPosition.tag = "Temp";

        ResetValues();

        _gm = GameManager.instance;
        if (!respawn && _gm != null) _gm.AddEnemy(this);
        _pm = PlayerManager.instance;
        if (_pm != null) player = _pm.player;

        _coll = GetComponent<Collider2D>();

        _as = GetComponent<AudioSource>();
    }

    private void ResetValues()
    {
        Busy = false;
        Health = startingHealth;
        _nextPatrolTime = Time.time;
        _nextAttackTime = Time.time;
        _state = EnemyState.Patrol;
        Running = false;
        _rageUntil = Time.time;
    }

    private void Update()
    {
        UpdatePatrolPosition();
        UpdateState();
        UpdateAnimation();
        UpdateRunning();
        UpdateSpeed();
    }

    private void UpdateState()
    {
        if (Busy)
        {
            _destinationSetter.target = null;
            return;
        }

        if (_pm == null || !_pm.IsAlive() || !hostile)
        {
            _state = EnemyState.Patrol;
            _destinationSetter.target = _targetPatrolPosition.transform;
        }
        else if (CheckAttack())
        {
            _state = EnemyState.Attack;
            _destinationSetter.target = null;
            if (Time.time >= _nextAttackTime)
            {
                _nextAttackTime = Time.time + enemyAttackPeriod;
                torsoAnimator.SetTrigger("Attack");
            }
        }
        else if (CheckChase())
        {
            _state = EnemyState.Chase;
            _destinationSetter.target = player.transform;
        }
        else
        {
            _state = EnemyState.Patrol;
            _destinationSetter.target = _targetPatrolPosition.transform;
        }
    }

    private bool CheckAttack()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance > enemyAttackRadius) return false;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float angle = Vector2.Angle(transform.up, direction);
        return angle <= 180;
    }

    private bool CheckChase()
    {
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance >= sightDistance) return false;
        Vector2 direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, sightCastMask);
        return hit.collider != null && hit.collider.transform == player.transform;
    }
    private Vector2 GetRandomPatrolPosition()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2);
        float randomDistance = Mathf.Sqrt(Random.Range(0f, 1f)) * patrolRadius;

        float offsetX = randomDistance * Mathf.Cos(randomAngle);
        float offsetY = randomDistance * Mathf.Sin(randomAngle);

        return new Vector2(_patrolPosition.x + offsetX, _patrolPosition.y + offsetY);
    }

    private void UpdatePatrolPosition()
    {
        if (Time.time < _nextPatrolTime) return;
        _nextPatrolTime = Time.time + Random.Range(patrolPeriod, patrolPeriod + patrolPeriodBuffer);
        Vector2 patrolPosition = GetRandomPatrolPosition();
        _targetPatrolPosition.transform.position = patrolPosition;
    }

    private void UpdateAnimation()
    {
        bool moving = _aiPath.velocity.magnitude > 0.1f;
        if (torsoAnimator) torsoAnimator.SetBool("Moving", moving);
        if (legsAnimator) legsAnimator.SetBool("Moving", moving);
    }

    private void UpdateRunning()
    {
        if ((_state == EnemyState.Chase || _state == EnemyState.Attack) && Time.time <= _rageUntil)
            Running = true;
        else Running = false;
    }

    private void UpdateSpeed()
    {
        _aiPath.maxSpeed = Running ? runSpeed : walkSpeed;
    }

    public void HandleAttackFrame()
    {
        if (CheckAttack() && !_pm.IsInvincible)
        {
            PlayRandomSound(hitAttackSounds);
            _pm.HandleHit();
        }
        else PlayRandomSound(missedAttackSounds);
    }

    public void HandleHit(int damage)
    {
        _rageUntil = Time.time + rageDuration;
        Health = Mathf.Max(Health - damage, 0);
        if (Health > 0) torsoAnimator.SetTrigger("Hurt");
        else HandleDeath();
    }

    private void HandleDeath()
    {
        _gm.RemoveEnemy(this);
        torsoAnimator.SetTrigger("Die");
        PlayRandomSound(deathSounds);
        EnableComponents(false);
    }

    public void OnDeathAnimationFinish()
    {
        torsoAnimator.enabled = false;
        enabled = false;
        if (respawn) Invoke("Respawn", respawnTimer);
    }

    private void Respawn()
    {
        enabled = true;
        torsoAnimator.enabled = true;
        EnableComponents(true);
        ResetValues();
        transform.position = _patrolPosition;

        torsoAnimator.Rebind();
        torsoAnimator.Update(0f);
        legsAnimator.Rebind();
        legsAnimator.Update(0f);
    }

    private void EnableComponents(bool enable)
    {
        legsAnimator.gameObject.SetActive(enable);
        torsoAnimator.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = enable ? "Unit Top" : "Corpse";
        torsoAnimator.gameObject.GetComponent<EnemyAnimationEventHandler>().enabled = enable;
        _coll.enabled = enable;
        _aiPath.enabled = enable;
        _seeker.enabled = enable;
        _destinationSetter.enabled = enable;
    }

    private void OnDrawGizmos()
    {
        DrawAttackGizmos();
        DrawLOSGizmos();
        DrawPatrolRadius();
    }
    private void DrawAttackGizmos()
    {
        Gizmos.color = Color.red;

        // Set the number of segments to divide the half-circle
        int segments = 30;
        float halfCircleAngle = 90f;
        float radius = enemyAttackRadius;

        // Calculate the angle step between each segment
        float angleStep = (halfCircleAngle * 2) / segments;

        // The direction the unit is facing (use transform.up for 2D top-down)
        Vector3 forward = transform.up;

        // Calculate the initial direction for the left edge of the half-circle
        Vector3 leftEdgeDirection = Quaternion.Euler(0, 0, -halfCircleAngle) * forward;
        Vector3 previousPoint = transform.position + leftEdgeDirection * radius;

        for (int i = 1; i <= segments; i++)
        {
            // Rotate the forward direction step by step to form the half-circle
            float currentAngle = -halfCircleAngle + (i * angleStep);
            Vector3 pointDirection = Quaternion.Euler(0, 0, currentAngle) * forward;

            // Calculate the point on the circumference
            Vector3 point = transform.position + pointDirection * radius;

            // Draw the line between the previous point and the current point
            Gizmos.DrawLine(previousPoint, point);

            // Update the previous point
            previousPoint = point;
        }

        // Draw the base lines from the enemy's position to the edges of the half-circle
        Vector3 rightEdgeDirection = Quaternion.Euler(0, 0, halfCircleAngle) * forward;
        Vector3 leftEdge = transform.position + leftEdgeDirection * radius;
        Vector3 rightEdge = transform.position + rightEdgeDirection * radius;

        Gizmos.DrawLine(transform.position, leftEdge);
        Gizmos.DrawLine(transform.position, rightEdge);
    }
    private void DrawLOSGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sightDistance);
    }
    private void DrawPatrolRadius()
    {
        Vector2 center = Application.isPlaying ? _patrolPosition : transform.position;
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(center, patrolRadius);
    }

    private void PlayRandomSound(AudioClip[] sounds)
    {
        _as.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }
}
