using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class BossVisual : MonoBehaviour {
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private EnemyEntity _enemyEntity;
    [SerializeField] private GameObject _enemyShadow;
    [SerializeField] private GameWin gameWin;
    private PolygonCollider2D _polygonCollider2D;
    private PolygonCollider2D _secondPolygonCollider; 
    public GameObject SecondAttackBoss;  
    private Collider2D secondAttackBossCollider;

    private Animator _animator;

    private const string IS_RUNNING = "IsRunning";
    private const string TAKEHIT = "TakeHit";
    private const string IS_DIE = "IsDie";
    private const string CHASING_SPEED_MULTIPLIER = "ChasingSpeedMultiplier";
    private const string ATTACK1 = "Attack1";
    private const string ATTACK2 = "Attack2";

    SpriteRenderer _spriteRenderer;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();


        if (SecondAttackBoss != null) {
            _secondPolygonCollider = SecondAttackBoss.GetComponent<PolygonCollider2D>();
            if (_secondPolygonCollider == null) {
                Debug.LogError("PolygonCollider2D на SecondAttackBoss не знайдено!");
            }


            secondAttackBossCollider = SecondAttackBoss.GetComponent<Collider2D>();
            if (secondAttackBossCollider == null) {
                Debug.LogError("Collider2D на SecondAttackBoss не знайдено!");
            }
        } else {
            Debug.LogError("SecondAttackBoss не призначено в інспекторі!");
        }

        TriggerAttackAnimationTurnOff();
        DisableSecondAttackCollider();
    }


    private void Start() {
        _enemyAI.OnEnemyAttack += _enemyAI_OnEnemyAttack;
        _enemyEntity.OnTakeHit += _enemyEntity_OnTakeHit;
        _enemyEntity.OnDeath += _enemyEntity_OnDeath;
    }

    private void _enemyEntity_OnDeath(object sender, System.EventArgs e) {
        _animator.SetBool(IS_DIE, true);
        _spriteRenderer.sortingOrder = 0;
        _enemyShadow.SetActive(false);
        gameWin.BossDied();
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
        _polygonCollider2D.enabled = false; 
    }

    public void TriggerAttackAnimationTurnOn() {
        _polygonCollider2D.enabled = true; 
    }

    public void EnableSecondAttackCollider() {
        secondAttackBossCollider.enabled = true; 

    }

    public void DisableSecondAttackCollider() {
        secondAttackBossCollider.enabled = false; 

    }

    private void _enemyAI_OnEnemyAttack(object sender, System.EventArgs e) {
        int randomAttack = UnityEngine.Random.Range(0, 2);

        if (randomAttack == 0) {
            _animator.SetTrigger(ATTACK1); 
        } else {
            _animator.SetTrigger(ATTACK2);
            EnableSecondAttackCollider();
        }
    }
}
