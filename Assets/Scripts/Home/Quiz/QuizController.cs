using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

[Serializable] public class Question
{
    public int question_id = 0;
    public int video_id = 0;
    public string question = "";
    public List<Option> options = new List<Option>();
}
[Serializable] public class Option
{
    public int option_id = 0;
    public string option = "";
    public string correct_incorrect = "";
}

public class QuizController : MonoBehaviour
{
    [SerializeField] private Text queNumberText;
    [SerializeField] private Text queHeaderText;
    [SerializeField] private Text queBodyText;
    [SerializeField] private Text optionA_Text;
    [SerializeField] private Text optionB_Text;
    [SerializeField] private Text optionC_Text;
    [SerializeField] private Text optionD_Text;

    [SerializeField] private GameObject greateJobPanel;
    [SerializeField] private GameObject tryAgainJobPanel;

    private List<Question> question = new List<Question>();
    public void OnOptionClick(int optionId)
    {

    }


    //-----------------------
    public void PopulateQuizzesOnVideo(int videoId)
    {
        StartCoroutine(LoadQuestions("video", videoId)); 
    }
    public void PopulateQuizzesOnSubCat(int subcatId)
    {
        StartCoroutine(LoadQuestions("subcat", subcatId));
    }


    IEnumerator LoadQuestions(string quetype, int id)
    {
        HomeMainUIController.EventShowHideLoader.Invoke(true);
        string endPoint = "";
        if(quetype == "video")
        {
            endPoint = WebRequests.Instance.getQueOfVideoEndPoint;
        } 
        else if(quetype == "subcat")
        {
            endPoint = WebRequests.Instance.getQueOfSubCatEndPoint;
        }
        using (UnityWebRequest webRequest = UnityWebRequest.Get(Globals.BASE_URL + endPoint + id))
        {
            // Request and wait for the desired page.
            webRequest.SetRequestHeader("Accept", "application/json");//
            webRequest.SetRequestHeader("Authorization", "Bearer "+Globals.UserLoginDetails.access_token);

            yield return webRequest.SendWebRequest();
            HomeMainUIController.EventShowHideLoader.Invoke(false);
            while(!webRequest.isDone)
                yield return webRequest;
            if (webRequest.isNetworkError)
            { 
                
            }
            else
            {
                print(webRequest.downloadHandler.text);       
            } 
            
        }


    }


    

}