using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameWin : MonoBehaviour {
    public GameObject gameWinScreen;
    public Sprite[] gameWinSprites;
    public Image gameWinImage;
    public float spriteChangeDuration = 0.1f;
    public float delayBeforeGameOver = 2f;


    private int currentSpriteIndex = 0;
    private float nextSpriteChangeTime = 0f;

    private GameInput gameInput;

    void Start() {
        gameWinScreen.SetActive(false);
    }

    public void BossDied() {
        StartCoroutine(ShowGameOverAfterDelay());
    }

    private IEnumerator ShowGameOverAfterDelay() {
        yield return new WaitForSecondsRealtime(delayBeforeGameOver);

        Time.timeScale = 0f;
        gameWinScreen.SetActive(true);
        currentSpriteIndex = 0;
        nextSpriteChangeTime = Time.unscaledTime;
    }

    void Update() {
        if (gameWinScreen.activeSelf) {
            ChangeSprites();
        }
    }

    private void ChangeSprites() {
        if (currentSpriteIndex < gameWinSprites.Length) {
            if (Time.unscaledTime > nextSpriteChangeTime) {
                gameWinImage.sprite = gameWinSprites[currentSpriteIndex];
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

        yield return new WaitForSecondsRealtime(6f);

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
