using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        // Load scene Gameplay (cần thêm scene vào Build Settings)
        SceneManager.LoadScene("Gameplay");
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        // Thoát game
        Application.Quit();
        Debug.Log("Game is quitting..."); // Debug để kiểm tra trong Editor
    }
}
