using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void StartGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame() {
        #if UNITY_EDITOR
                EditorApplication.ExitPlaymode(); 
        #else
                Application.Quit(); 
        #endif
    }
}
