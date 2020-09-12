using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject UI_instructions;
    public GameObject UI_mainBtns;

    // Start is called before the first frame update
    public void PlayGame ()
    {
        SceneManager.LoadScene("Map01");
        Time.timeScale = 1f;
    }

    public void ShowInstructions ()
    {
        UI_instructions.SetActive(true);
        UI_mainBtns.SetActive(false);
    }

    public void HideInstructions ()
    {
        UI_instructions.SetActive(false);
        UI_mainBtns.SetActive(true);
    }

    public void ExitGame ()
    {
        Application.Quit();
    }
}
