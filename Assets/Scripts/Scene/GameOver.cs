using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameOver : MonoBehaviour {
    public GameObject gameOverScreen;
    public Sprite[] gameOverSprites;
    public Image gameOverImage;
    public float spriteChangeDuration = 0.2f;
    public float delayBeforeGameOver = 2f;


    private int currentSpriteIndex = 0;
    private float nextSpriteChangeTime = 0f;

    private GameInput gameInput;

    void Start() {
        gameOverScreen.SetActive(false);
    }

    public void HeroDied() {
        StartCoroutine(ShowGameOverAfterDelay());
    }

    private IEnumerator ShowGameOverAfterDelay() {
        yield return new WaitForSecondsRealtime(delayBeforeGameOver);

        Time.timeScale = 0f;
        gameOverScreen.SetActive(true);
        currentSpriteIndex = 0;
        nextSpriteChangeTime = Time.unscaledTime;
    }

    void Update() {
        if (gameOverScreen.activeSelf) {
            ChangeSprites();
        }
    }

    private void ChangeSprites() {
        if (currentSpriteIndex < gameOverSprites.Length) {
            if (Time.unscaledTime > nextSpriteChangeTime) {
                gameOverImage.sprite = gameOverSprites[currentSpriteIndex];
                currentSpriteIndex++;
                nextSpriteChangeTime = Time.unscaledTime + spriteChangeDuration;
            }
        }
        ActivateMainMenu();
    }

    private void ActivateMainMenu() {
        StartCoroutine(LoadMainMenuAfterDelay());
    }

    private IEnumerator LoadMainMenuAfterDelay() {

        yield return new WaitForSecondsRealtime(4f);

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
