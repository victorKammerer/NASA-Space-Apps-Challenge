using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;  
using SimpleJSON;

public class OSD379APIManager : MonoBehaviour
{

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI identifierText;
    public TextMeshProUGUI dateText;
    private string apiUrl = "https://osdr.nasa.gov/osdr/data/osd/meta/379";

    void Start()
    {
        StartCoroutine(GetRequest(apiUrl));
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
                    string description = data["study"]["OSD-379"]["studies"][0]["description"];
                    string identifier = data["study"]["OSD-379"]["studies"][0]["identifier"];
                    string publicReleaseDate = data["study"]["OSD-379"]["studies"][0]["publicReleaseDate"];
                    string submissionDate = data["study"]["OSD-379"]["studies"][0]["submissionDate"];

                    publicReleaseDate = publicReleaseDate.Replace("-", " ");
                    submissionDate = submissionDate.Replace("-", " ");

                    titleText.text = title;
                    descriptionText.text = description;
                    identifierText.text = identifier;
                    dateText.text = submissionDate + " - " + publicReleaseDate;
                }
                else
                {
                    Debug.LogWarning("Expected fields are missing in the JSON response.");
                }
            }
        }
    }
}
