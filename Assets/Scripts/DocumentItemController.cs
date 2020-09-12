using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentItemController : MonoBehaviour
{
    public GameObject documentController;

    private DocumentController dc;

    private void Awake ()
    {
        dc = documentController.GetComponent<DocumentController>();
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            dc.TookDocument();
            Destroy(this.gameObject);
        }
    }
}
