using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    public static TransitionScript instance;
    public GameObject ball;
    public float transitionTime = 2;

    void Start()
    {
        if (instance != null)
            Destroy(this);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void BallTransitionToScene(string sceneName, bool isFromTheLeft = true)
    {
        Vector3 previousPosition = ball.transform.localPosition;
        if (!isFromTheLeft)
            ball.transform.localPosition = new Vector3(
                -previousPosition.x,
                previousPosition.y,
                previousPosition.z
            );
        ball.transform.DOLocalMoveX(0, transitionTime / 2)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);
                float targetPosition = isFromTheLeft ? -previousPosition.x : previousPosition.x;
                ball.transform.DOLocalMoveX(targetPosition, transitionTime / 2)
                    .OnComplete(() =>
                    {
                        ball.transform.localPosition = previousPosition;
                    });
            });
    }
}
