using UnityEngine;
using UnityEngine.UI;

public class VirtualButtonManager : MonoBehaviour {

    [SerializeField] private GameObject virtualAttackButton;
    [SerializeField] private GameObject virtualMoveJoystick; 

    void Start() {
        #if UNITY_STANDALONE || UNITY_EDITOR
                DisableVirtualButtons();
        #elif UNITY_ANDROID || UNITY_IOS
                    EnableVirtualButtons();
        #endif
    }

    void EnableVirtualButtons() {
        if (virtualAttackButton != null) {
            virtualAttackButton.SetActive(true);
        }
        if (virtualMoveJoystick != null) {
            virtualMoveJoystick.SetActive(true);
        }
    }

    void DisableVirtualButtons() {
        if (virtualAttackButton != null) {
            virtualAttackButton.SetActive(false);
        }
        if (virtualMoveJoystick != null) {
            virtualMoveJoystick.SetActive(false);
        }
    }
}
