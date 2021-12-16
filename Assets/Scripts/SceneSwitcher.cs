using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Menu() {
        SceneManager.LoadScene("Menu");
    }

    public void Credits() {
        SceneManager.LoadScene("Credits");
    }

    public void About() {
        SceneManager.LoadScene("About");
    }

    public void Game() {
        SceneManager.LoadScene("Simulation");
    }
}
