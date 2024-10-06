using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnboardingController : MonoBehaviour
{
    public Onboarding[] onboardingSlides;
    public Image[] Knobs;
    public Color knobUnactiveColor;
    public float colorAnimationTime = 0.30f;
    public TextMeshProUGUI title;
    private TypeOutScript titleAnimation;
    public float titleAnimationTime = 2;
    private bool isInAnimation;
    public Image background;
    public TextMeshProUGUI body;
    public Image backButtonImage;
    public TextMeshProUGUI backButtonLabel;
    public Image nextButtonImage;
    public TextMeshProUGUI nextButtonLabel;
    private int _currentSlide;

    private void Awake()
    {
        titleAnimation = title.GetComponent<TypeOutScript>();
        titleAnimation.TotalTypeTime = titleAnimationTime;
        SetupSlide();
        SetupColors(true);
    }

    public void GoToNextSlide()
    {
        _currentSlide += 1;

        if (_currentSlide == onboardingSlides.Length)
        {
            TransitionScript.instance.BoxTransitionToScene("Experiments");
            return;
        }

        SetupSlide();
    }

    public void GoToPreviousSlide()
    {
        _currentSlide -= 1;

        if (_currentSlide == -1)
        {
            TransitionScript.instance.BoxTransitionToScene("Start", false);
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
        nextButtonLabel.text = onboardingSlides[_currentSlide].buttonLabel;
        titleAnimation.On = true;

        background.sprite = onboardingSlides[_currentSlide].backgroundImage;

        SetupColors();
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

    private void SetupColors(bool isInitialSetup = false)
    {
        Color desiredColor = onboardingSlides[_currentSlide].primaryColor;

        if (isInitialSetup)
        {
            nextButtonImage.color = desiredColor;
            backButtonImage.color = desiredColor;
            backButtonLabel.color = desiredColor;
            body.color = onboardingSlides[_currentSlide].bodyColor;
        }
        else
        {
            nextButtonImage.DOColor(desiredColor, colorAnimationTime);
            backButtonImage.DOColor(desiredColor, colorAnimationTime);
            backButtonLabel.DOColor(desiredColor, colorAnimationTime);
            body.DOColor(onboardingSlides[_currentSlide].bodyColor, colorAnimationTime);
        }

        SetupKnob(isInitialSetup);
    }

    private void SetupKnob(bool isInitialSetup = false)
    {
        int currentKnob = 0;
        foreach (Image knob in Knobs)
        {
            Color desiredColor =
                currentKnob == _currentSlide
                    ? onboardingSlides[_currentSlide].primaryColor
                    : knobUnactiveColor;
            if (isInitialSetup)
                knob.color = desiredColor;
            else
                knob.DOColor(desiredColor, colorAnimationTime);
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
    public Color primaryColor;
    public Color bodyColor;
    public Sprite backgroundImage;
}
