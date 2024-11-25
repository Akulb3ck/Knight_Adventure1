using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour {
    [SerializeField] private float _explosionRadius = 2f;
    [SerializeField] private int _explosionDamage = 5;
    [SerializeField] private GameObject _explosionEffectPrefab;
    [SerializeField] private float _gravity = -9.81f;

    private Animator _animator;
    private bool _hasLanded = false;
    private Vector3 _targetPosition;
    private Rigidbody2D _rigidbody2D;

    private void Awake() {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>(); 
    }

    private void Start() {
        _targetPosition = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y, transform.position.z);
        StartCoroutine(ThrowBomb());
    }

    private IEnumerator ThrowBomb() {
        Vector3 startPosition = transform.position;
        Vector3 velocity = CalculateThrowVelocity(startPosition, _targetPosition);
        float timeElapsed = 0f;

        while (!_hasLanded) {
            timeElapsed += Time.deltaTime;
            float x = Mathf.Lerp(startPosition.x, _targetPosition.x, timeElapsed);
            float y = Mathf.Lerp(startPosition.y, _targetPosition.y, timeElapsed) + (velocity.y * timeElapsed + 0.5f * _gravity * Mathf.Pow(timeElapsed, 2));

            if (y <= _targetPosition.y + 0.1f) {
                _hasLanded = true;
                transform.position = new Vector3(x, _targetPosition.y, startPosition.z);

                _rigidbody2D.linearVelocity = Vector2.zero;

                Explode();
                yield break;
            }

            transform.position = new Vector3(x, y, startPosition.z);
            yield return null;
        }

    }

    private Vector3 CalculateThrowVelocity(Vector3 startPosition, Vector3 targetPosition) {
        float distance = Vector3.Distance(startPosition, targetPosition);
        float angle = 45f; // Оптимальний кут для параболічного польоту
        float radians = angle * Mathf.Deg2Rad;

        float velocityX = distance / Mathf.Cos(radians);
        float velocityY = velocityX * Mathf.Tan(radians);

        return new Vector3(velocityX, velocityY, 0f);
    }

    private void Explode() {
        if (_animator != null) {
            _animator.SetTrigger("Explode");
        }

        DealDamageAtExplosion();
    }

    public void DealDamageAtExplosion() {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (Collider2D playerCollider in hitPlayers) {
            if (playerCollider.CompareTag("Player")) {
                Player player = playerCollider.GetComponent<Player>();
                if (player != null) {
                    player.TakeDamage(_explosionDamage);
                }
            }
        }
    }

    public void OnDestroy() {
        if (_animator != null) {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }
}
