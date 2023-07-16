using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public string nameScene;

    public void NewGame()
    {
        SceneManager.LoadScene(nameScene);
    }

    public void ExitButton()
    {
        // Debug.Log("Exit");
        Application.Quit();
    }
}
