using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 5f;

    void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
        transform.Translate(movement * speed * Time.deltaTime);
    }
}

