using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject gameOverText; // Reference to the Game Over Text UI element
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Check if the game is over and if the player presses 'R' to restart
        if (isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    /// <summary>
    /// Checks whether the game is currently running.
    /// </summary>
    public bool IsGameOver()
    {
        return isGameOver;
    }

    /// <summary>
    /// Restarts the game by reloading the scene.
    /// </summary>
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Handles the game-over state.
    /// </summary>
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverText.SetActive(true); // Enable the Game Over UI
        Debug.Log("Game Over! Notifying systems...");
    }
}