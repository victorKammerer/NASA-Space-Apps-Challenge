using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnboardingController : MonoBehaviour
{
    public Onboarding[] onboardingSlides;
    public Image[] Knobs;
    public Color knobActiveColor;
    public Color knobUnactiveColor;

    public TextMeshProUGUI title;
    private TypeOutScript titleAnimation;
    public float titleAnimationTime = 2;
    private bool isInAnimation;
    public TextMeshProUGUI body;
    public TextMeshProUGUI buttonLabel;
    private int _currentSlide;

    private void Awake()
    {
        titleAnimation = title.GetComponent<TypeOutScript>();
        titleAnimation.TotalTypeTime = titleAnimationTime;
        SetupSlide();
    }

    public void GoToNextSlide()
    {
        _currentSlide += 1;

        if (_currentSlide == onboardingSlides.Length)
        {
            SceneManager.LoadScene("Experiments");
            return;
        }

        SetupSlide();
    }

    public void GoToPreviousSlide()
    {
        _currentSlide -= 1;

        if (_currentSlide == -1)
        {
            SceneManager.LoadScene("Start");
            return;
        }

        SetupSlide();
    }

    private void SetupSlide()
    {
        if (isInAnimation)
        {
            isInAnimation = false;
            StopAllCoroutines();
        }

        title.text = onboardingSlides[_currentSlide].title;
        titleAnimation.FinalText = title.text;
        StartCoroutine(titleAnimationCoroutine());
        body.text = onboardingSlides[_currentSlide].body;
        buttonLabel.text = onboardingSlides[_currentSlide].buttonLabel;
        titleAnimation.On = true;

        SetupKnob();
    }

    IEnumerator titleAnimationCoroutine()
    {
        titleAnimation.reset = true;
        title.enableAutoSizing = true;
        yield return new WaitForSeconds(0);
        titleAnimation.On = true;
        float previousFontSize = title.fontSize;
        title.enableAutoSizing = false;
        title.fontSize = previousFontSize;
        yield return new WaitForSeconds(titleAnimationTime);
        title.enableAutoSizing = true;
        isInAnimation = false;
    }

    private void SetupKnob()
    {
        int currentKnob = 0;
        foreach (Image knob in Knobs)
        {
            Debug.Log(knob.gameObject.name);
            knob.color = currentKnob == _currentSlide ? knobActiveColor : knobUnactiveColor;
            currentKnob += 1;
        }
    }
}

[Serializable]
public class Onboarding
{
    public string title;
    public string body;
    public string buttonLabel;
}
