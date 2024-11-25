using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    [SerializeField] private float movingSpeed = 10f;
    [SerializeField] private int _maxHealth;
    [SerializeField] private GameOver gameOver;
    [SerializeField] private HealthBar healthBar;
    public event EventHandler OnTakeHitPlayer;
    public event EventHandler OnDeathPlayer;
    public Joystick joystick;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D _polygonCollider2D;
    private BoxCollider2D _boxCollider2D;
    private int _currentHealth;
    public bool IsDead => _currentHealth <= 0;
    public bool DieToGameOver = false;

    private Vector2 inputVector;

    private Rigidbody2D rb;

    private float minMovingSpeed = 0.1f;
    private bool isRunning = false;

    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void Start() {
        _currentHealth = _maxHealth;
        healthBar.SetHealth(_currentHealth, _maxHealth);
    }

    private void Update() {
        #if UNITY_STANDALONE || UNITY_EDITOR
                HandleKeyboardInput();
        #elif UNITY_ANDROID || UNITY_IOS
                    HandleTouchInput();
        #endif
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleMovement() {
        if (IsDead) return;

        rb.MovePosition(rb.position + inputVector * (movingSpeed * Time.deltaTime));

        if (Mathf.Abs(inputVector.x) > minMovingSpeed || Mathf.Abs(inputVector.y) > minMovingSpeed) {
            isRunning = true;
        } else {
            isRunning = false;
        }
    }

    private void HandleKeyboardInput() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        inputVector = new Vector2(horizontal, vertical);
    }

    private void HandleTouchInput() {
        inputVector = new Vector2(joystick.Horizontal, joystick.Vertical);
    }

    private void DetectDeath() {
        if (_currentHealth <= 0) {
            _boxCollider2D.enabled = false;
            _polygonCollider2D.enabled = false;
            gameOver.HeroDied();
            OnDeathPlayer?.Invoke(this, EventArgs.Empty);
        }
    }

    public void TakeDamage(int damage) {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        healthBar.SetHealth(_currentHealth, _maxHealth);
        OnTakeHitPlayer?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    public bool IsRunning() {
        return isRunning;
    }

    public Vector3 GetPlayerScreenPosition() {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

    
}