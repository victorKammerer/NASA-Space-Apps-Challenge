using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;  
using SimpleJSON;

public class OSD379MethodologyAPIManager : MonoBehaviour
{

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI identifierText;

    private string apiUMethodologyUrl = "https://osdr.nasa.gov/osdr/data/osd/meta/379";

    void Start()
    {
        StartCoroutine(GetRequest(apiUMethodologyUrl));
    }

    IEnumerator GetRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                JSONNode data = JSON.Parse(jsonResponse);

                if (data["study"]["OSD-379"] != null && data["study"]["OSD-379"]["studies"] != null && data["study"]["OSD-379"]["studies"].Count > 0)
                {
                    string title = data["study"]["OSD-379"]["studies"][0]["title"];
                    string identifier = data["study"]["OSD-379"]["studies"][0]["identifier"];

                    titleText.text = title;
                    identifierText.text = identifier;
                }
                else
                {
                    Debug.LogWarning("Expected fields are missing in the JSON response.");
                }
            }
        }
    }
}
