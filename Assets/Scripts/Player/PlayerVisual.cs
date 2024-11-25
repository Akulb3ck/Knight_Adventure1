using System;
using System.Collections;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class PlayerVisual : MonoBehaviour {

    public GameObject SecondAttack;
    private Collider2D secondAttackCollider;

    [SerializeField] private int _damageAmount = 2;

    private int _currentHealth;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D _polygonCollider2D;
    public Joystick joystick;
    public Button attackButton;

    private const string IS_RUNNING = "IsRunning";
    private const string TAKE_HIT = "TakeHit";
    private const string IS_DIE = "IsDie";
    private const string ATTACK = "Attack";
    private const string SECOND_ATTACK = "SecondAttack";

    private int attackCounter = 0;
    private float lastAttackTime = 0f;
    private float doubleClickTime = 0.7f;
    private bool canAttack = true;
    private bool isDead = false;
    private float attackCooldownForSecond = 1.5f;

    private void Awake() {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
        PolygonColiderTurnOff();
        if (SecondAttack != null) {
            secondAttackCollider = SecondAttack.GetComponent<Collider2D>();
        }

        DisableSecondAttackCollider();
    }

    private void Start() {
        Player.Instance.OnTakeHitPlayer += _Player_OnTakeHit;
        Player.Instance.OnDeathPlayer += _Player_OnDeath;
    }

    private void Update() {
        animator.SetBool(IS_RUNNING, Player.Instance.IsRunning());
        AdjustPlayerFacingDirection();
        #if UNITY_STANDALONE || UNITY_EDITOR
                GameInput.Instance.OnPlayerAttack += GameInput_OnPlayerAttack;
        #elif UNITY_ANDROID || UNITY_IOS
        #endif
    }

    private void OnDestroy() {
        #if UNITY_STANDALONE || UNITY_EDITOR
                GameInput.Instance.OnPlayerAttack -= GameInput_OnPlayerAttack;
        #endif
    }

    private void GameInput_OnPlayerAttack(object sender, EventArgs e) {
         PerformAttack();
    }

    private IEnumerator AttackCooldownForSecond() {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownForSecond);
        canAttack = true;
    }


    public void PerformAttack() {
        float timeSinceLastAttack = Time.time - lastAttackTime;
        lastAttackTime = Time.time;
        if (canAttack && !isDead) {
            if (timeSinceLastAttack <= doubleClickTime) {
                attackCounter++;
            } else {
                attackCounter = 1;
            }

            if (attackCounter == 1) {
                animator.SetTrigger(ATTACK);
            } else if (attackCounter >= 2) {
                animator.SetTrigger(SECOND_ATTACK);
                attackCounter = 0;
                StartCoroutine(AttackCooldownForSecond());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.TryGetComponent(out EnemyEntity enemyEntity)) {
            enemyEntity.TakeDamageEnemy(_damageAmount);
        }
    }

    private void _Player_OnTakeHit(object sender, System.EventArgs e) {
        animator.SetTrigger(TAKE_HIT);
    }

    private void AdjustPlayerFacingDirection() {
        Vector2 movementDirection = new Vector2(joystick.Horizontal, joystick.Vertical);

        if (movementDirection.x < 0) {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } else if (movementDirection.x > 0) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }



    private void _Player_OnDeath(object sender, System.EventArgs e) {
        animator.SetBool(IS_DIE, true);
        spriteRenderer.sortingOrder = 0;
    }

    public void PolygonColiderTurnOff() {
        _polygonCollider2D.enabled = false;
    }

    public void PolygonColiderTurnOn() {
        _polygonCollider2D.enabled = true;
    }

    public void EnableSecondAttackCollider() {
        if (secondAttackCollider != null) {
            secondAttackCollider.enabled = true;
        }
    }

    public void DisableSecondAttackCollider() {
        if (secondAttackCollider != null) {
            secondAttackCollider.enabled = false;
        }
    }

    

}
