using UnityEngine;

public class SecondAttack : MonoBehaviour
{
    [SerializeField] private int secondAttackDamage = 3;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out EnemyEntity enemyEntity)) {
            enemyEntity.TakeDamageEnemy(secondAttackDamage); 
        }
    }
}
