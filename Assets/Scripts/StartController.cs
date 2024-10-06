using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    public void LoadOnboarding()
    {
        TransitionScript.instance.BoxTransitionToScene("Onboarding");
    }

    public void LoadExperiments()
    {
        TransitionScript.instance.BoxTransitionToScene("Experiments");
    }
}
