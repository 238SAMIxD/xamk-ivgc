using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TMP_Text killsText = null;
    public TMP_Text scoreText = null;

    private void Awake() {
        killsText.text = "Demons killed: " + GameController.instance.kills;
        scoreText.text = "Score: " + GameController.instance.score;
    }

    public void NewGame() {
        SceneManager.LoadScene(1);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
}
