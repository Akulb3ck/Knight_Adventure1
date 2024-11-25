using UnityEngine;

public class EnemyShoot : MonoBehaviour {
    [SerializeField] private GameObject _bombPrefab; 
    [SerializeField] private float _throwForce = 10f; 
    
    public void ThrowBomb(Vector3 targetPosition) {
        if (_bombPrefab == null) {
            Debug.LogWarning("Bomb prefab is not assigned in EnemyBombThrower!");
            return;
        }

        GameObject bomb = Instantiate(_bombPrefab, transform.position, Quaternion.identity);

        Vector2 direction = (targetPosition - transform.position).normalized;
        Rigidbody2D bombRb = bomb.GetComponent<Rigidbody2D>();
        if (bombRb != null) {
            bombRb.AddForce(direction * _throwForce, ForceMode2D.Impulse);
        }
    }

    
}
