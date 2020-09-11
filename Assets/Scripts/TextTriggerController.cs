using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTriggerController : MonoBehaviour
{
    public GameObject player;
    public GameObject messageBox;

    public string textToShow = "";

    private MessageController messageController;

    private void Start ()
    {
        messageController = messageBox.GetComponent<MessageController>();
        if (messageController != null)
            Debug.Log(messageController.gameObject.name);
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            messageController.SetText(textToShow);
            messageController.ShowMessageBox();
            Destroy(this);
        }
        
    }

}
