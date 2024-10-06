using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TypeOutScript : MonoBehaviour
{
    public bool On = true;
    public bool reset = false;
    public String FinalText;

    public float TotalTypeTime = -1f;

    public float TypeRate;
    private float LastTime;

    public string RandomCharactor;
    public float RandomCharacterChangeRate = 0.1f;
    private float RandomCharacterTime;

    public int i;

    void Start()
    {
        FinalText = GetComponent<TextMeshProUGUI>().text;
    }

    private string RandomChar()
    {
        byte value = (byte)UnityEngine.Random.Range(41f, 128f);

        string c = Encoding.ASCII.GetString(new byte[] { value });

        return c;
    }

    public void Skip()
    {
        GetComponent<TextMeshProUGUI>().text = FinalText;
        On = false;
    }

    // Update is called once per frame
    void OnGUI()
    {
        if (TotalTypeTime != -1f)
        {
            TypeRate = TotalTypeTime / (float)FinalText.Length;
        }

        if (On == true)
        {
            if (Time.time - RandomCharacterTime >= RandomCharacterChangeRate)
            {
                // RandomCharactor = RandomChar();
                RandomCharacterTime = Time.time;
            }

            try
            {
                GetComponent<TextMeshProUGUI>().text = FinalText.Substring(0, i) + "|";
            }
            catch (ArgumentOutOfRangeException)
            {
                On = false;
            }

            if (Time.time - LastTime >= TypeRate)
            {
                i++;
                LastTime = Time.time;
            }

            bool isChar = false;

            while (isChar == false)
            {
                if ((i + 1) < FinalText.Length)
                {
                    if (FinalText.Substring(i, 1) == " ")
                    {
                        i++;
                    }
                    else
                    {
                        isChar = true;
                    }
                }
                else
                {
                    isChar = true;
                }
            }

            if (GetComponent<TextMeshProUGUI>().text.Length == FinalText.Length + 1)
            {
                // RandomCharactor = RandomChar();
                GetComponent<TextMeshProUGUI>().text = FinalText;
                On = false;
            }
        }

        if (reset == true)
        {
            //			GetComponent<>().text = "";
            GetComponent<TextMeshProUGUI>().text = "";
            i = 0;
            reset = false;
        }
    }
}
