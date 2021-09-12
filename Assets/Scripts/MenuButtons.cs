using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [HideInInspector] public Menu menu;
    private bool _hasFocussed;

    public GameObject mainMenuWrapper;
    public GameObject optionsMenuWrapper;

    public Button[] buttons;

    public void Play()
    {
        if (_hasFocussed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void Options()
    {
        if (_hasFocussed)
        {
            menu = Menu.Options;
        }
    }

    public void Back()
    {
        if (_hasFocussed)
        {
            menu = Menu.MainMenu;
        }
    }
    
    public void Quit()
    {
        if (_hasFocussed)
        {
            Application.Quit();
        }
    }

    public void TwitterLink()
    {
        Application.OpenURL("https://twitter.com/CasparG00");
    }

    private void Update()
    {
        if (!Application.isFocused)
        {
            _hasFocussed = false;
        }
        else
        {
            if (!_hasFocussed)
            {
                StartCoroutine(FocusTime());
            }
        }

        foreach (var button in buttons)
        {
            button.interactable = _hasFocussed;
        }

        switch (menu)
        {
            case Menu.MainMenu:
                mainMenuWrapper.SetActive(true);
                optionsMenuWrapper.SetActive(false);
                break;
            case Menu.Options:
                mainMenuWrapper.SetActive(false);
                optionsMenuWrapper.SetActive(true);
                break;
        }
    }

    //Prevent Accidental Clicking on Buttons when Focussed out of the Game
    private IEnumerator FocusTime()
    {
        yield return new WaitForSeconds(0.2f);
        _hasFocussed = true;
    }

    public enum Menu
    {
        MainMenu,
        Options,
        Pause
    }
}
