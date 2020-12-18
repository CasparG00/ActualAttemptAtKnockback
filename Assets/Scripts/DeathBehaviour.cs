using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathBehaviour : MonoBehaviour
{
    public GameObject ui;
    public GameObject deathUi;
    public GameObject weapon;

    public Text deathScore;
    public Text highScore;

    public Volume v;

    private ColorAdjustments _ca;

    private void Start()
    {
        if (v.profile.TryGet<ColorAdjustments>(out var colorAdjustments)) _ca = colorAdjustments;
        
        ui.SetActive(true);
        deathUi.SetActive(false);
    }

    private void Update()
    {
        if (!PlayerStats.IsDead) return;
        
        ui.SetActive(false);
        weapon.SetActive(false);
        deathUi.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        
        _ca.saturation.value = Mathf.SmoothStep(_ca.saturation.value, -25, Time.deltaTime * 10f);
        
        var tempHighScore = PlayerPrefs.GetInt("HighScore", 0);

        deathScore.text = "CURRENT SCORE: " + PlayerStats.Score;
        highScore.text = "HIGH SCORE: " + tempHighScore;

        if (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1)) return;
        if (PlayerStats.Score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", PlayerStats.Score);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
