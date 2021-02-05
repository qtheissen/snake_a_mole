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
    public float tweenFrameRate = 60; // How many updates per second for tweens

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
        QuickHideEndScreen(); // Hide end screen before showing with tweens

        StartCoroutine(ShowEndScreenCoroutine()); // Coroutine for pauses between coroutines
    }

    IEnumerator ShowEndScreenCoroutine() // Coroutine for pauses between coroutines
    {
        gameObject.SetActive(true); // Reenable end screen because it could be disabled
        
        StartCoroutine(TweenBackground(1.5f, bTr1, bTr0)); // Show background
        
        yield return new WaitForSeconds(1); // Start dilating game over before background finished
        
        StartCoroutine(TweenGameOver(2f, gOD1, gOD0)); // Dilate game over

        yield return new WaitForSeconds(3); // After 3 seconds show scores and buttons
        
        StartCoroutine(TweenAnchorMin(2, points, pMi1, pMi0)); // Tween points from right to left
        StartCoroutine(TweenAnchorMax(2, points, pMa1, pMa0));
        
        yield return new WaitForSeconds(1.5f); // Delay so that you have to read scores before buttons
        
        StartCoroutine(TweenAnchorMin(2, buttons, bMi1, bMi0)); // Tween points from left to right
        StartCoroutine(TweenAnchorMax(2, buttons, bMa1, bMa0));
    }

    public void QuickHideEndScreen()
    {
        background.color = bTr1; // Hide all end screen items
        
        gameOver.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, gOD1);

        points.anchorMin = pMi1;
        points.anchorMax = pMa1;

        buttons.anchorMin = bMi1;
        buttons.anchorMax = bMa1;
        
        gameObject.SetActive(false); // Disable end screen for ¿performance?
    }

    public void QuickShowEndScreen()
    {
        background.color = bTr0; // Show all end screen items
        
        gameOver.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, gOD0);

        points.anchorMin = pMi0;
        points.anchorMax = pMa0;

        buttons.anchorMin = bMi0;
        buttons.anchorMax = bMa0;

        gameObject.SetActive(true); // Reenable end screen because it could be disabled
    }
    
    public void HideEndScreen()
    {
        QuickShowEndScreen(); // Show end screen before hiding with tweens

        StartCoroutine(HideEndScreenCoroutine()); // Coroutine for pauses between coroutines
    }

    IEnumerator HideEndScreenCoroutine()
    {
        StartCoroutine(TweenAnchorMin(1, buttons, bMi0, bMi1)); // Tween buttons from right to left
        StartCoroutine(TweenAnchorMax(1, buttons, bMa0, bMa1));

        yield return new WaitForSeconds(.5f); // Before buttons finish start tweening buttons
        
        StartCoroutine(TweenAnchorMin(1, points, pMi0, pMi1)); // Tween points from left to right
        StartCoroutine(TweenAnchorMax(1, points, pMa0, pMa1));
        
        yield return new WaitForSeconds(.5f); // Start tweening gameover before points completely disappear

        StartCoroutine(TweenGameOver(1f, gOD0, gOD1)); // Hide game over faster than background
        
        StartCoroutine(TweenBackground(1.25f, bTr0, bTr1)); // Hide background slower than game over text
        
        yield return new WaitForSeconds(1.25f);
        gameObject.SetActive(false); // Disable end screen for ¿performance? after all tweens finished
    }

    IEnumerator TweenBackground(float time, Color from, Color to)
    {
        for (float i = 0; i <= 1; i += 1 / time / tweenFrameRate) // Step from 0 to 1 by rate affected by time and framerate
        {
            background.color = Color.Lerp(from, to, i); // Lerp color smoothly
            
            yield return new WaitForSeconds(1/tweenFrameRate); // Steps shouldnt be affected by time otherwise it will always be 1 second long
        }
    }
    IEnumerator TweenGameOver(float time, float from, float to)
    {
        for (float i = 0; i <= 1; i += 1 / time / tweenFrameRate) // Step from 0 to 1 by rate affected by time and framerate
        {
            gameOver.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Lerp(from, to, i)); // Lerp dilation smoothly
            
            yield return new WaitForSeconds(1/tweenFrameRate); // Steps shouldnt be affected by time otherwise it will always be 1 second long
        }
    }

    
    IEnumerator TweenAnchorMin(float time, RectTransform toTween, Vector2 from, Vector2 to)
    {
        for (float i = 0; i <= 1; i += 1/time/tweenFrameRate) // Step from 0 to 1 by rate affected by time and framerate
        {
            toTween.anchorMin = Vector2.Lerp(from, to, i); // Lerp vector2 smoothly
            
            yield return new WaitForSeconds(1/tweenFrameRate); // Steps shouldnt be affected by time otherwise it will always be 1 second long
        }
    }
    
    IEnumerator TweenAnchorMax(float time, RectTransform toTween, Vector2 from, Vector2 to)
    {
        for (float i = 0; i <= 1; i += 1/time/tweenFrameRate) // Step from 0 to 1 by rate affected by time and framerate
        {
            toTween.anchorMax = Vector2.Lerp(from, to, i); // Lerp vector2 smoothly
            
            yield return new WaitForSeconds(1/tweenFrameRate); // Steps shouldnt be affected by time otherwise it will always be 1 second long
        }
    }
}
