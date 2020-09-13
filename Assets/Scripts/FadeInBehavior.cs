using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInBehavior : MonoBehaviour
{

    public CanvasGroup canvas;

    private void Start ()
    {
        StartCoroutine(FadeIn(canvas, 0f, 1f, 3.0f));   
    }

    IEnumerator FadeIn (CanvasGroup canvas, float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float endTime = Time.time + duration;
        float elapsedTime = 0f;

        while (Time.time <= endTime)
        {
            elapsedTime = Time.time - startTime; // update the elapsed time
            var percentage = 1 / (duration / elapsedTime); // calculate how far along the timeline we are
            if (startAlpha > endAlpha) // if we are fading out/down 
            {
                canvas.alpha = startAlpha - percentage; // calculate the new alpha
            }
            else // if we are fading in/up
            {
                canvas.alpha = startAlpha + percentage; // calculate the new alpha
            }

            yield return new WaitForEndOfFrame(); // wait for the next frame before continuing the loop
        }
        canvas.alpha = endAlpha;


    }
}
