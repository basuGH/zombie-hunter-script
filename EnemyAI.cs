
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    Transform _target;
    NavMeshAgent _navMeshAgent;
    Animator _animator;
    float _distanceToTarget = Mathf.Infinity;
    [SerializeField] bool _isProvoked = false;


    [SerializeField] float _chaseRange;
    [SerializeField] float _turnSpeed;
    [SerializeField] bool _isAttacking;
    [SerializeField] bool _isAlive;
    [SerializeField] bool _isHit;
    [SerializeField] int _health;
    [SerializeField] float _minDistanceToFullVolume;
    [SerializeField] AudioClip _hitAudio, _deathAudio;
    [SerializeField] GameObject _attackComponent;
    [SerializeField] GameObject[] _onDeathItems;
    private void Awake()
    {
        _target = GameObject.FindWithTag("Player").transform;
    }
    void Start()
    {
        _isAlive = true;
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = Random.Range(2, 6);
        _health = Random.Range(25, 50);
    }

    public void OnTakeDamage()
    {
        _isProvoked = true;
        Debug.Log(_isProvoked);
    }

    void Update()
    {
        if (Player.Instance.IsMissionComplete)
        {
            return;
        }
        AudioVolumeFactor();
        RunAnimation(_navMeshAgent.velocity.magnitude);
        AttackTargetAnimation();

        //Debug.Log(isProvoked);
        ProcessEngageTarget();
        _attackComponent.SetActive(_isAttacking);
    }

    private void ProcessEngageTarget()
    {
        if (_target == null) return;

        _distanceToTarget = Vector3.Distance(transform.position, _target.position);

        if (_isProvoked)
        {
            EngageTarget();
        }
        if (_distanceToTarget <= _chaseRange)
        {
            _isProvoked = true;
        }
        else
        {
            _isProvoked = false;
            _animator.SetBool("IsRunning", false);
        }
    }

    private void RunAnimation(float speed)
    {
        _animator.SetFloat("Speed", speed);
    }

    void EngageTarget()
    {
        if (!_isAlive)
        {
            _navMeshAgent.isStopped = true;
            return;
        }
        if (_isHit)
        {
            _navMeshAgent.isStopped = true;
            return;
        }
        else if (!_isHit)
        {
            _navMeshAgent.isStopped = false;
        }

        FaceTarget();

        if (_distanceToTarget >= _navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (_distanceToTarget <= _navMeshAgent.stoppingDistance)
        {

            _isAttacking = true;
            _navMeshAgent.isStopped = true;

        }
        else
        {
            _isAttacking = false;
            _navMeshAgent.isStopped = false;

        }
        AttackTargetAnimation();

    }

    public void ResetAttackEvent(AnimationEvent animationEvent)
    {
        //  _isAttacking = false;
    }
    void AttackTargetAnimation()
    {
        _animator.SetBool("IsAttacking", _isAttacking);
        //if (_isAttacking)
        //{

        //}
        //else
    }
    void ChaseTarget()
    {

            _navMeshAgent.SetDestination(_target.position);


    }
    private void FaceTarget()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);
    }
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, _chaseRange);

    //}
    public float AudioVolumeFactor()
    {
        float distance = Vector3.Distance(_target.position, transform.position);
        float volume = _minDistanceToFullVolume / distance;
        return volume;

    }
    public void OnHit(AnimationEvent animationEvent)
    {
        AudioManager.Instance.PlaySFX(_hitAudio, AudioVolumeFactor());
    }
    public void Damage(int hitPoint)
    {
        if (_isAlive)
        {
            _health -= hitPoint;
            AudioManager.Instance.PlayBloodSFX(AudioVolumeFactor());
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                _animator.SetTrigger("Hit");

            }
            _isHit = true;

            OnTakeDamage();
            if (_health <= 0)
            {
                OnDeath();
                AudioManager.Instance.PlaySFX(_deathAudio, AudioVolumeFactor());
                _isAlive = false;
                _animator.SetBool("IsAlive", _isAlive);
                Destroy(gameObject, 10f);
            }
        }
    }
     void OnDeath()
    {
        int i = Random.Range(0, _onDeathItems.Length);
        GameObject item = Instantiate(_onDeathItems[i], transform.position + new Vector3(Random.Range(-1f,1f),2, Random.Range(-1f, 1f)), Quaternion.identity);
        Destroy(item, 30f);
    }
    public void ResetHit(AnimationEvent animationEvent)
    {
        _isHit = false;
    }

    public void SetBase1()
    {
        _navMeshAgent.baseOffset = -0.5f;
        _navMeshAgent.height = 1;
    }
    public void SetBase2()
    {
        _navMeshAgent.baseOffset = -1f;
        _navMeshAgent.height = 0.25f;
    }
}