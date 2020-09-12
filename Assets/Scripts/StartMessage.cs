using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMessage : TextTriggerController
{
    // Start is called before the first frame update
    private void Start()
    {
        messageController = messageBox.GetComponent<MessageController>();
        StartCoroutine(MessageTimeout());
    }

    IEnumerator MessageTimeout ()
    {
        yield return new WaitForSeconds(0.1f);
        ShowMessageP();
    }

}
