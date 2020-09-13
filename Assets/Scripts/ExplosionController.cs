using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{

    private AudioManager audioMgr;
    private SpriteRenderer sr;

    private void Awake ()
    {
        audioMgr = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioMgr.PlaySound("Explosion");
        StartCoroutine(FadeOut(sr, 1f, 0f, 2.0f));
    }

    IEnumerator FadeOut (SpriteRenderer explosion, float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;
        float elapsedTime = 0f;

        Debug.Log("Bakuretsu mahou");

        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime; // update the elapsed time
            var percentage = 1 / (duration / elapsedTime); // calculate how far along the timeline we are
            if (startAlpha > endAlpha) // if we are fading out/down 
            {
                explosion.color = new Color(1f, 1f, 1f, startAlpha - percentage);
            }
            else // if we are fading in/up
            {
                explosion.color = new Color(1f, 1f, 1f, startAlpha + percentage);
            }

            yield return new WaitForEndOfFrame(); // wait for the next frame before continuing the loop
        }
        explosion.color = new Color(1f, 1f, 1f, endAlpha);
    }
}
