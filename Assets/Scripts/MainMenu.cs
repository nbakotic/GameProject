using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void ContinueButtonPressed()
    {
        print("Continue pressed");
    }

    public void NewGameButtonPressed()
    {
        print("New game pressed");
    }

    public void OptionsButtonPressed()
    {
        print("Options pressed");
        SceneManager.LoadScene("OptionsMenu");
    }

    public void ExitGameButtonPressed()
    {
        print("Exit game pressed");
        Application.Quit();
    }
}
