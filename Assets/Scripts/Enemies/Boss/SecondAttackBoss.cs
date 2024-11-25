using UnityEngine;

public class SecondAttackBoss : MonoBehaviour
{
    [SerializeField] private int secondAttackBossDamage = 6;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out Player palyer)) {
            palyer.TakeDamage(secondAttackBossDamage);
        }
    }
}

