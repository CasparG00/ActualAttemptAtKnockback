using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Button[] buttons;

    public Text highScoreText;

    [Header("Transition Settings")]
    public Transform mainCamera;
    public Transform mainMenuCameraPos;
    public Transform mainMenuOptionsCameraPos;
    public Transform mainMenuGameInfoCameraPos;
    private Transform _currentView;

    public float transitionSpeed = 2f;
    [Space]
    public GameObject mainMenuWrapper;
    public GameObject optionsWrapper;
    public GameObject gameInfoWrapper;

    private bool _hasFocussed;
    public TurretEnemy mainTurret;
    public TurretEnemy secondaryTurret;

    private void Start()
    {
        optionsWrapper.SetActive(false);
        gameInfoWrapper.SetActive(false);
        mainMenuWrapper.SetActive(true);

        _currentView = mainMenuCameraPos;
        
        secondaryTurret.menuState = TurretEnemy.MenuState.Inactive;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _hasFocussed = true;
    }
    
    private void Update()
    {
        highScoreText.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore", 0);;
        
        _hasFocussed = Application.isFocused;

        foreach (var button in buttons)
        {
            button.interactable = _hasFocussed;
        }
    }

    private void LateUpdate()
    {
        mainCamera.position = Vector3.Slerp(mainCamera.position, _currentView.position, Time.deltaTime * transitionSpeed);
        mainCamera.rotation = Quaternion.Slerp(mainCamera.rotation, _currentView.rotation, Time.deltaTime * transitionSpeed);
    }

    public void Play()
    {
        PlayerStats.IsDead = false;
        PlayerStats.Score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Options()
    {
        mainMenuWrapper.SetActive(false);
        gameInfoWrapper.SetActive(false);
        optionsWrapper.SetActive(true);

        _currentView = mainMenuOptionsCameraPos;
    }

    public void GameInfo()
    {
        mainMenuWrapper.SetActive(false);
        optionsWrapper.SetActive(false);
        gameInfoWrapper.SetActive(true);

        _currentView = mainMenuGameInfoCameraPos;
        
        mainTurret.menuState = TurretEnemy.MenuState.Inactive;
    }

    public void Back()
    {
        optionsWrapper.SetActive(false);
        gameInfoWrapper.SetActive(false);
        mainMenuWrapper.SetActive(true);

        _currentView = mainMenuCameraPos;

        mainTurret.menuState = TurretEnemy.MenuState.Active;
    }
    
    public void Quit() => Application.Quit();

    public void TwitterLink() => Application.OpenURL("https://twitter.com/CasparG00");

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
    }
    
    public void ShowControls(bool showControls)
    {
        ControlHelper.ShowControls = showControls;
    }

    public void ResetHighScore() => PlayerPrefs.SetInt("HighScore", 0);
}
