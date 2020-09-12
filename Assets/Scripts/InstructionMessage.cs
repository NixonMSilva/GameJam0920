using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionMessage : TextTriggerController
{
    // Start is called before the first frame update
    void Start()
    {
        messageController = messageBox.GetComponent<MessageController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ShowMessageP();
    }
}
