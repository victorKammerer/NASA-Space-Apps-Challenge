﻿/*
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections.Generic;
using CodeMonkey.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Window_Graph : MonoBehaviour
{
    [SerializeField]
    private Sprite dotSprite;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private List<GameObject> gameObjectList;
    public GraphType graphType;

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();

        gameObjectList = new List<GameObject>();

        List<int> valueList = new List<int>()
        {
            5,
            98,
            56,
            45,
            30,
            22,
            17,
            15,
            13,
            17,
            25,
            37,
            40,
            36,
            33
        };
        IGraphVisual lineGraphVisual = new LineGraphVisual(
            graphContainer,
            dotSprite,
            Color.green,
            new Color(1, 1, 1, .5f)
        );
        IGraphVisual barChartVisual = new BarChartVisual(graphContainer, Color.green, .8f);

        switch (graphType)
        {
            case GraphType.BAR:
                ShowGraph(
                    valueList,
                    barChartVisual,
                    -1,
                    (int _i) => "Day " + (_i + 1),
                    (float _f) => "$" + Mathf.RoundToInt(_f)
                );
                break;
            case GraphType.LINE:
                ShowGraph(
                    valueList,
                    lineGraphVisual,
                    -1,
                    (int _i) => "Day " + (_i + 1),
                    (float _f) => "$" + Mathf.RoundToInt(_f)
                );
                break;
        }
    }

    private void ShowGraph(
        List<int> valueList,
        IGraphVisual graphVisual,
        int maxVisibleValueAmount = -1,
        Func<int, string> getAxisLabelX = null,
        Func<float, string> getAxisLabelY = null
    )
    {
        if (getAxisLabelX == null)
        {
            getAxisLabelX = delegate(int _i)
            {
                return _i.ToString();
            };
        }
        if (getAxisLabelY == null)
        {
            getAxisLabelY = delegate(float _f)
            {
                return Mathf.RoundToInt(_f).ToString();
            };
        }

        if (maxVisibleValueAmount <= 0)
        {
            maxVisibleValueAmount = valueList.Count;
        }

        foreach (GameObject gameObject in gameObjectList)
        {
            Destroy(gameObject);
        }
        gameObjectList.Clear();

        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMaximum = valueList[0];
        float yMinimum = valueList[0];

        for (
            int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0);
            i < valueList.Count;
            i++
        )
        {
            int value = valueList[i];
            if (value > yMaximum)
            {
                yMaximum = value;
            }
            if (value < yMinimum)
            {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0)
        {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        yMinimum = 0f; // Start the graph at zero

        float xSize = graphWidth / (maxVisibleValueAmount + 1);

        int xIndex = 0;

        for (
            int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0);
            i < valueList.Count;
            i++
        )
        {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((valueList[i] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            gameObjectList.AddRange(
                graphVisual.AddGraphVisual(new Vector2(xPosition, yPosition), xSize)
            );

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<TextMeshProUGUI>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            xIndex++;
        }

        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<TextMeshProUGUI>().text = getAxisLabelY(
                yMinimum + (normalizedValue * (yMaximum - yMinimum))
            );
            gameObjectList.Add(labelY.gameObject);
        }
    }

    private interface IGraphVisual
    {
        List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth);
    }

    private class BarChartVisual : IGraphVisual
    {
        private RectTransform graphContainer;
        private Color barColor;
        private float barWidthMultiplier;

        public BarChartVisual(
            RectTransform graphContainer,
            Color barColor,
            float barWidthMultiplier
        )
        {
            this.graphContainer = graphContainer;
            this.barColor = barColor;
            this.barWidthMultiplier = barWidthMultiplier;
        }

        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth)
        {
            GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth);
            return new List<GameObject>() { barGameObject };
        }

        private GameObject CreateBar(Vector2 graphPosition, float barWidth)
        {
            GameObject gameObject = new GameObject("bar", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = barColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
            rectTransform.sizeDelta = new Vector2(barWidth * barWidthMultiplier, graphPosition.y);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(.5f, 0f);
            return gameObject;
        }
    }

    private class LineGraphVisual : IGraphVisual
    {
        private RectTransform graphContainer;
        private Sprite dotSprite;
        private GameObject lastDotGameObject;
        private Color dotColor;
        private Color dotConnectionColor;

        public LineGraphVisual(
            RectTransform graphContainer,
            Sprite dotSprite,
            Color dotColor,
            Color dotConnectionColor
        )
        {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColor = dotColor;
            this.dotConnectionColor = dotConnectionColor;
            lastDotGameObject = null;
        }

        public List<GameObject> AddGraphVisual(Vector2 graphPosition, float graphPositionWidth)
        {
            List<GameObject> gameObjectList = new List<GameObject>();
            GameObject dotGameObject = CreateDot(graphPosition);
            gameObjectList.Add(dotGameObject);
            if (lastDotGameObject != null)
            {
                GameObject dotConnectionGameObject = CreateDotConnection(
                    lastDotGameObject.GetComponent<RectTransform>().anchoredPosition,
                    dotGameObject.GetComponent<RectTransform>().anchoredPosition
                );
                gameObjectList.Add(dotConnectionGameObject);
            }
            lastDotGameObject = dotGameObject;
            return gameObjectList;
        }

        private GameObject CreateDot(Vector2 anchoredPosition)
        {
            GameObject gameObject = new GameObject("dot", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().sprite = dotSprite;
            gameObject.GetComponent<Image>().color = dotColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            return gameObject;
        }

        private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
        {
            GameObject gameObject = new GameObject("dotConnection", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = dotConnectionColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(
                0,
                0,
                UtilsClass.GetAngleFromVectorFloat(dir)
            );
            return gameObject;
        }
    }
}

public enum GraphType
{
    BAR,
    LINE
}
