using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;

    public static bool IsPaused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !PlayerStats.IsDead)
        {
            IsPaused = !IsPaused;
        }

        pauseMenu.SetActive(IsPaused);
        Cursor.visible = IsPaused;
        Cursor.lockState = IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = IsPaused ? 0 : 1;
    }

    public void BackToMenu()
    {
        IsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Resume()
    {
        IsPaused = false;
    }
}
