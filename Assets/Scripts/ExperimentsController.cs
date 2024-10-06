using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExperimentsController : MonoBehaviour
{
    public void LoadExperiment(int experimentNumber)
    {
        SceneManager.LoadScene("Experiment " + experimentNumber);
    }
}
