using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class FaceRecogThingy : MonoBehaviour
{

    WebCamTexture webcamTexture;
    CascadeClassifier cascade;
    OpenCvSharp.Rect myFace;
    public RawImage rawImage;
    private int counter=0;
    public Text angryTxt;
    public Text disgustTxt;
    public Text fearTxt;
    public Text happyTxt;
    public Text sadTxt;
    public Text surpriseTxt;
    public Text neutralTxt;
    public Text topEmotionTxt;
    public Text FaceDetectedTxt;
    private float[] scores = new float[7];
    /*
     0 = angry
     1 = disgust
     2 = fear
     3 = happy
     4 = sad
     5 = surprised
     6 = neutral
     */
    private bool faceDetected;
    //private string baseAddress = "http://127.0.0.1:5000/getEmotion/";
    //private string imgAdd = "D:\\Louie files\\School work\\3rd year Summer\\Capstone 1\\Flask_test\\nervous.png";
    
    void Start()
    {
        //activates webcam and displays to raw image
        WebCamDevice[] devices = WebCamTexture.devices;

        webcamTexture = new WebCamTexture(devices[0].name);
        webcamTexture.Play();
        cascade = new CascadeClassifier(Application.dataPath + @"/haarcascade_frontalface_default.xml");

        rawImage.texture = webcamTexture;
        rawImage.material.mainTexture = webcamTexture;

       

    }

    void Update()
    {
        //GetComponent<Renderer>().material.mainTexture = webcamTexture;
        Mat frame = OpenCvSharp.Unity.TextureToMat(webcamTexture);

        findNewFace(frame);
        //Debug.Log("space Button was pressed");
       if (Input.GetKeyDown("space") && faceDetected == true)
        {
            saveImages();
            StartCoroutine(GetRequest("http://127.0.0.1:5000/getEmotion/" + Application.dataPath + "/images/" + counter + ".png", 1));
            StartCoroutine(GetRequest("http://127.0.0.1:5000/getTopEmotion/" + Application.dataPath + "/images/" + counter + ".png", 2));
            StartCoroutine(delayInSeconds());
            
        }
        if (Input.GetKeyDown("left"))
        {
            eventChanger(scores);
        }
        if (Input.GetKeyDown("up"))
        {
            deleteImages();
            Debug.Log("up Button was pressed");


        }

    }

    void eventChanger(float[] emotions) // scene switcher based on emotion
    {
        /*
        0 = angry
        1 = disgust
        2 = fear
        3 = happy
        4 = sad
        5 = surprised
        6 = neutral
        */
        if (emotions[0] >= 0.5)
        {
            Debug.Log("angry Emotion Score + " + emotions[0].ToString());
            SceneManager.LoadScene("AngryScene");
        }
        if (emotions[1] >= 0.85)
        { 
        
        
        }



    }


    IEnumerator delayInSeconds()
    {

        yield return new WaitForSeconds(10);
        Debug.Log("okay");
    
    
    }
    IEnumerator GetRequest(string uri,int checker) //gets emotion and get top emotion from flask
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            yield return webRequest.SendWebRequest();
            switch (checker)
            {
                case 1://get emotions scores
                    if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                    {
                        Debug.Log("Error: " + webRequest.error);
                    }
                    else
                    {
                        //parses data returned from flask 
                        JSONNode results = JSON.Parse(webRequest.downloadHandler.text);
                        Debug.Log("Results " + results);
                        string[] emotionsArray = new string[7];
                        emotionsArray[0] = results["angry"];
                        emotionsArray[1] = results["disgust"];
                        emotionsArray[2] = results["fear"];
                        emotionsArray[3] = results["happy"];
                        emotionsArray[4] = results["sad"];
                        emotionsArray[5] = results["surprise"];
                        emotionsArray[6] = results["neutral"];
                        if (emotionsArray[0] == null)
                        {
                            Debug.Log("No Face Detected");
                        }
                        else
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                switch (i) //assigns data into data array and prints 
                                {
                                    case 0:
                                        Debug.Log("angry : " + emotionsArray[i]);
                                        angryTxt.text = emotionsArray[i].ToString();
                                        scores[0] = float.Parse(emotionsArray[i]);
                                        
                                        break;
                                    case 1:
                                        Debug.Log("disgust : " + emotionsArray[i]);
                                        disgustTxt.text = emotionsArray[i].ToString();
                                        scores[1] = float.Parse(emotionsArray[i]);
                                        break;
                                    case 2:
                                        Debug.Log("fear : " + emotionsArray[i]);
                                        fearTxt.text = emotionsArray[i].ToString();
                                        scores[2] = float.Parse(emotionsArray[i]);
                                        break;
                                    case 3:
                                        Debug.Log("happy : " + emotionsArray[i]);
                                        happyTxt.text = emotionsArray[i].ToString();
                                        scores[3] = float.Parse(emotionsArray[i]);
                                        break;
                                    case 4:
                                        Debug.Log("sad : " + emotionsArray[i]);
                                        sadTxt.text = emotionsArray[i].ToString();
                                        scores[4] = float.Parse(emotionsArray[i]);
                                        break;
                                    case 5:
                                        Debug.Log("surprise: " + emotionsArray[i]);
                                        surpriseTxt.text = emotionsArray[i].ToString();
                                        scores[5] = float.Parse(emotionsArray[i]);
                                        break;
                                    case 6:
                                        Debug.Log("neutral : " + emotionsArray[i]);
                                        neutralTxt.text = emotionsArray[i].ToString();
                                        scores[6] = float.Parse(emotionsArray[i]);
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 2: //gets top emotion and score
                    if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                    {
                        Debug.Log("Error: " + webRequest.error);
                    }
                    else
                    {
                        topEmotionTxt.text = "Top Emotion: " + webRequest.downloadHandler.text;
                        //JSONNode results = JSON.Parse(webRequest.downloadHandler.text);
                        //string[] topEmotion = new string[2];
                        //topEmotion[0] = results["emotions"];
                        //topEmotion[1] = results["score"];

                        //if (topEmotion[0] == null)
                        //{
                        //    Debug.Log("No Face Detected");
                        //}
                        //else
                        //{
                        //    topEmotionTxt.text = "Top Emotion: " + topEmotion.ToString();
                        //}
                    }
                    break;
            }
            
        }

    }
    void findNewFace(Mat frame)
    {
        var faces = cascade.DetectMultiScale(frame, 1.1,2, HaarDetectionType.ScaleImage);

        if (faces.Length >= 1)
        {
            //Debug.Log("Face Detected");
            FaceDetectedTxt.text = "Face Detected";
            faceDetected = true;
            myFace = faces[0];

        }
        else {
            FaceDetectedTxt.text = "No Face Detected";
            faceDetected = false;
            //Debug.Log("No face detected");
        }

    }

    void display(Mat frame)
    {
        if (myFace != null)
        {
            frame.Rectangle(myFace, new Scalar(250, 0, 0),2);

        }
        Texture newTexture = OpenCvSharp.Unity.MatToTexture(frame);
        GetComponent<Renderer>().material.mainTexture = newTexture;
    }

    void saveImages() //saves images of user
    {
        Texture2D texture = new Texture2D(rawImage.texture.width, rawImage.texture.height, TextureFormat.ARGB32, false);

        texture.SetPixels(webcamTexture.GetPixels());
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/images/" + ++counter + ".png", bytes);
        Debug.Log("Image Saved");
        refreshEditor();
    }

    void deleteImages() //deletes all images in folder
    {
        while (counter > 0)
        {

            string file_Path = Application.dataPath + "/images/" + counter + ".png";

            if ((!File.Exists(file_Path)))
            {
                Debug.Log("File doesnt Exist");

            }
            else
            {
                File.Delete(file_Path);
                Debug.Log("File " + file_Path + " has been Deleted");
                counter--;
            }
        }
        Debug.Log("images has been deleted");
        refreshEditor();
    
    
    }
    void refreshEditor()//refreshes folder
    {
        UnityEditor.AssetDatabase.Refresh();
    
    
    }
}
