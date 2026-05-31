using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject gameOverPanel;
    public GameObject playerHealth;

    private void Awake()
    {
        instance = this;
        playerHealth.SetActive(true);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        playerHealth.SetActive(false);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        CancelInvoke();
        PlayerHealthManager.instance.ResetHealth();
        gameOverPanel.SetActive(false);
        playerHealth.SetActive(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {   
        CancelInvoke();
    }

}
