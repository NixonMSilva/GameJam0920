using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    public Text messageText;

    public GameObject player;

    private PlayerMovement playerController;

    public bool isActive = false;

    private void Awake ()
    {
        playerController = player.GetComponent<PlayerMovement>();
        if (playerController != null)
        {
            Debug.Log(playerController.gameObject.name);
        }
        gameObject.SetActive(false);
    }

    public void Update ()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                HideMessageBox();
            }
        }
    }

    public void SetText (string newText)
    {
        messageText.text = newText;
    }

    public void ShowMessageBox ()
    {
        gameObject.SetActive(true);
        isActive = true;
        Time.timeScale = 0f;
    }

    public void HideMessageBox ()
    {
        gameObject.SetActive(false);
        isActive = false;
        Time.timeScale = 1f;
    }
}
