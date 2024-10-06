using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    public void LoadOnboarding()
    {
        SceneManager.LoadScene("Onboarding");
    }

    public void LoadExperiments()
    {
        SceneManager.LoadScene("Experiments");
    }
}
