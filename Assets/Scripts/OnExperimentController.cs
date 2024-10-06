using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnExperimentController : MonoBehaviour
{
    private string _currerntExperiment;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        _currerntExperiment = "" + sceneName[sceneName.Length - 1];
    }

    public void GoToExperimentDetail(string type)
    {
        TransitionScript.instance.BoxTransitionToScene(type + " " + _currerntExperiment);
    }

    public void Back()
    {
        TransitionScript.instance.BoxTransitionToScene("Experiments", false);
    }
}
