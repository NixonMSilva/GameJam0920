using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HideTilemapCollider : MonoBehaviour
{

    private TilemapRenderer tilemap;

    private void Awake ()
    {
        tilemap = GetComponent<TilemapRenderer>();
    }

    private void Start ()
    {
        tilemap.enabled = false;
    }
}
