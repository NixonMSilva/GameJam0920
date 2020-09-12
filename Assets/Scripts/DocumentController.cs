using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentController : MonoBehaviour
{

    public GameObject endLevelTrigger;
    public GameObject nextLevelTransition;

    public GameObject foundAllDocumentsMessage;

    private int documentsTaken = 0;

    private int totalDocuments;

    private void Awake ()
    {
        totalDocuments = GameObject.FindGameObjectsWithTag("docs").Length;
        Debug.Log("There are " + totalDocuments + " documents!");
    }

    public void TookDocument ()
    {
        documentsTaken++;
        if (documentsTaken == totalDocuments)
        {
            endLevelTrigger.SetActive(false);
            nextLevelTransition.SetActive(true);
            foundAllDocumentsMessage.SetActive(true);
        }
    }

    public int GetCurrentDocuments ()
    {
        return documentsTaken;
    }
}
