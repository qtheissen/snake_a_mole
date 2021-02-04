using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Image background;
    public TextMeshProUGUI gameOver;
    public RectTransform points;
    public RectTransform buttons;

    [Space(10)]
    public float tweenFrameRate = 60; // How many updates per second

    [Header("Background transparency")]
    public Color bTr0 = new Color(0,0,0, .65f); // end screen background transparency 0 (standard)
    public Color bTr1 = new Color(0,0,0,0); // end screen background transparency 1 (hidden)
    
    [Header("GameOver dilates")]
    public float gOD0 = 0f; // game Over Dilate 0 (standard)
    public float gOD1 = -1f; // game Over Dilate 1 (hidden)
    
    [Header("Points anchors")]
    public Vector2 pMi0 = new Vector2(.3f, .49f); // points Anchor Min 0 (standard)
    public Vector2 pMa0 = new Vector2(.7f, .59f); // points Anchor Max 0 (standard)
    [Space(10)]
    public Vector2 pMi1 = new Vector2(1.3f, .49f); // points Anchor Min 1 (hidden)
    public Vector2 pMa1 = new Vector2(1.7f, .59f); // points Anchor Max 1 (hidden)
    
    [Header("Button anchors")]
    public Vector2 bMi0 = new Vector2(.35f, .2f); // buttons Anchor Min 0 (standard)
    public Vector2 bMa0 = new Vector2(.65f, .375f); // buttons Anchor Max 0 (standard)
    [Space(10)]
    public Vector2 bMi1 = new Vector2(1.35f, .2f); // buttons Anchor Min 1 (hidden)
    public Vector2 bMa1 = new Vector2(1.65f, .375f); // buttons Anchor Max 1 (hidden)

    public void ShowEndScreen()
    {
        QuickHideEndScreen();

        StartCoroutine(ShowEndScreenCoroutine()); // Coroutine for pauses between coroutines
    }

    IEnumerator ShowEndScreenCoroutine() // Coroutine for pauses between coroutines
    {
        StartCoroutine(TweenBackground(1.5f, bTr1, bTr0));
        
        yield return new WaitForSeconds(1);
        
        StartCoroutine(TweenGameOver(2f, gOD1, gOD0));

        yield return new WaitForSeconds(3); // After 3 seconds show scores and buttons
        
        StartCoroutine(TweenAnchorMin(2, points, pMi1, pMi0));
        StartCoroutine(TweenAnchorMax(2, points, pMa1, pMa0));
        
        yield return new WaitForSeconds(1.5f); // Delay so that you have to read scores before buttons
        
        StartCoroutine(TweenAnchorMin(2, buttons, bMi1, bMi0));
        StartCoroutine(TweenAnchorMax(2, buttons, bMa1, bMa0));
    }

    public void QuickHideEndScreen()
    {
        background.color = bTr1;
        
        gameOver.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, gOD1);

        points.anchorMin = pMi1;
        points.anchorMax = pMa1;

        buttons.anchorMin = bMi1;
        buttons.anchorMax = bMa1;
    }
    
    public void HideEndScreen()
    {
        // TEMP
        QuickHideEndScreen();
    }

    IEnumerator TweenBackground(float time, Color from, Color to)
    {
        for (float i = 0; i <= 1; i += 1 / time / tweenFrameRate)
        {
            background.color = Color.Lerp(from, to, i);
            
            yield return new WaitForSeconds(1/tweenFrameRate);
        }
    }
    IEnumerator TweenGameOver(float time, float from, float to)
    {
        for (float i = 0; i <= 1; i += 1 / time / tweenFrameRate)
        {
            gameOver.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(from, to, i));
            
            yield return new WaitForSeconds(1/tweenFrameRate);
        }
    }

    
    IEnumerator TweenAnchorMin(float time, RectTransform toTween, Vector2 from, Vector2 to)
    {
        for (float i = 0; i <= 1; i += 1/time/tweenFrameRate)
        {
            toTween.anchorMin = Vector2.Lerp(from, to, i);
            yield return new WaitForSeconds(1/tweenFrameRate);
        }
    }
    
    IEnumerator TweenAnchorMax(float time, RectTransform toTween, Vector2 from, Vector2 to)
    {
        for (float i = 0; i <= 1; i += 1/time/tweenFrameRate)
        {
            toTween.anchorMax = Vector2.Lerp(from, to, i);
            yield return new WaitForSeconds(1/tweenFrameRate);
        }
    }
}
