using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentsController : MonoBehaviour
{
    public void LoadExperiment(int experimentNumber)
    {
        TransitionScript.instance.BoxTransitionToScene("Experiment " + experimentNumber);
    }
}
