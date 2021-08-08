using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
public class flaskConnector : MonoBehaviour
{

    private string baseAddress = "http://127.0.0.1:5000/getEmotion/";
    private string imgAdd = "D:\\Louie files\\School work\\3rd year Summer\\Capstone 1\\Flask_test\\nervous.png";
    public GameObject Text;
    Coroutine temp;

    void Start()
    {
        //imgAdd = Application.dataPath.ToString() + "/images/";
        //StartCoroutine(GetRequest("http://127.0.0.1:5000/getEmotion/D:\\Louie files\\School work\\3rd year Summer\\Capstone 1\\Flask_test\\nervous.png"));

    }

    void Update()
    {
        

    }

    
    IEnumerator GetRequest(string uri)
    {
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                /*
                JSONNode results = JSON.Parse(webRequest.downloadHandler.text);
                string[] emotionsArray = new string[7];
                emotionsArray[0] = results["angry"];
                emotionsArray[1] = results["disgust"];
                emotionsArray[2] = results["fear"];
                emotionsArray[3] = results["happy"];
                emotionsArray[4] = results["neutral"];
                emotionsArray[5] = results["sad"];
                emotionsArray[6] = results["surpise"];
                for (int i = 0; i < 7; i++)
                {
                    Debug.Log(emotionsArray[i]);
                }*/

                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    
    }
}
