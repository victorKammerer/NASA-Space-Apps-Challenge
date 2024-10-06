using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScript : MonoBehaviour
{
    public static TransitionScript instance;
    public GameObject square;
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

    public void BoxTransitionToScene(
        string sceneName,
        bool isFromTheLeft = true,
        Color color = new Color()
    )
    {
        if (color == new Color())
            square.GetComponent<Image>().color = Color.black;
        else
            square.GetComponent<Image>().color = color;

        Vector3 previousPosition = square.transform.localPosition;
        if (!isFromTheLeft)
            square.transform.localPosition = new Vector3(
                -previousPosition.x,
                previousPosition.y,
                previousPosition.z
            );
        square
            .transform.DOLocalMoveX(0, transitionTime / 2)
            .OnComplete(() =>
            {
                SceneManager.LoadScene(sceneName);
                float targetPosition = isFromTheLeft ? -previousPosition.x : previousPosition.x;
                square
                    .transform.DOLocalMoveX(targetPosition, transitionTime / 2)
                    .OnComplete(() =>
                    {
                        square.transform.localPosition = previousPosition;
                    });
            });
    }
}
