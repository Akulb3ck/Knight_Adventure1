using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class GoblinVisual : MonoBehaviour {
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    [SerializeField] private GameObject _enemyShadow;
    [SerializeField] private int _maxBombs = 2; 
    private int _remainingBombs;
    [SerializeField] private float _bombThrowDistance = 5f; 
    private bool _canThrowBombs = true; 

    private Animator _animator;
    private EnemyShoot _bombThrowed;

    private const string IS_RUNNING = "IsRunning";
    private const string TAKEHIT = "TakeHit";
    private const string IS_DIE = "IsDie";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK = "Attack";
    private const string RANGEATTACK = "RangeAttack";

    SpriteRenderer _spriteRenderer;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bombThrowed = GetComponent<EnemyShoot>();
    }

    private void Start() {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
        _enemyEntity.OnDeath += _enemyEntity_OnDeath;

        _remainingBombs = _maxBombs;
    }

    private void _enemyEntity_OnDeath(object sender, System.EventArgs e) {
        _animator.SetBool(IS_DIE, true);
        _spriteRenderer.sortingOrder = 0;
        _enemyShadow.SetActive(false);
    }

    private void _enemyEntity_OnTakeHit(object sender, System.EventArgs e) {
        _animator.SetTrigger(TAKEHIT);
    }

    private void OnDestroy() {
        _enemyAI.OnEnemyAttack -= _enemyAI_OnEnemyAttack;
    }

    private void Update() {
        _animator.SetBool(IS_RUNNING, _enemyAI.IsRunning);
        _animator.SetFloat(CHASING_SPEED_MULTIPLIER, _enemyAI.GetRoamingAnimationSpeed());
    }

    public void TriggerAttackAnimationTurnOff() {
        _enemyEntity.PolygonColliderTurnOff();
    }

    public void TriggerAttackAnimationTurnOn() {
        _enemyEntity.PolygonColliderTurnOn();
    }

    public void ThrowBombAnimationEvent() {
        if (_remainingBombs > 0 && _canThrowBombs) {
            _bombThrowed.ThrowBomb(Player.Instance.transform.position);
            _remainingBombs--;

            if (_remainingBombs <= 0) {
                _canThrowBombs = false;
            }
        }
    }

    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e) {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

        if (_canThrowBombs && _remainingBombs > 0 && distanceToPlayer <= _bombThrowDistance && distanceToPlayer > _enemyAI._attackingDistance) {
            _animator.SetTrigger(RANGEATTACK); 
        } else if (distanceToPlayer <= _enemyAI._attackingDistance) {
            _animator.SetTrigger(ATTACK); 
        }
    }

}
