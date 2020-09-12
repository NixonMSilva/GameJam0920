using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTriggerController : MonoBehaviour
{
    public GameObject player;
    public GameObject messageBox;

    public GameObject followUpMessage;

    [TextArea]
    public string textToShow = "";

    [HideInInspector]
    public MessageController messageController;

    public bool canDestroy = true;

    private void Start ()
    {
        messageController = messageBox.GetComponent<MessageController>();
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            ShowMessage();
        }
    }

    private void ShowMessage ()
    {
        messageController.SetText(textToShow);
        messageController.ShowMessageBox();

        if (followUpMessage != null)
        {
            followUpMessage.SetActive(true);
        }

        if (canDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    public void ShowMessageP ()
    {
        ShowMessage();
    }
}
