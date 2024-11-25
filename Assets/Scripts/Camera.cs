using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    private Transform player;
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate() {
        Vector3 temp = transform.position;
        temp.x = player.position.x;
        temp.y = player.position.y;

        transform.position = temp;
    }

}
