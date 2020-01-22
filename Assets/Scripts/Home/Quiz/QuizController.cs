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
[Serializable] public class QuestionsRoot
{
    public List<Question> questions = new List<Question>(); 
}
[Serializable] public class Option
{
    public int option_id = 0;
    public string option = "";
    public string correct_incorrect = "";
}

[Serializable] public class GetQuetionResponse : ResponseBase
{
    public QuestionsRoot data = new QuestionsRoot();
}

public class QuizController : MonoBehaviour
{
    [SerializeField] private Text queNumberText; 
    [SerializeField] private Text queBodyText;
    [SerializeField] private Text optionA_Text;
    [SerializeField] private Text optionB_Text;
    [SerializeField] private Text optionC_Text;
    [SerializeField] private Text optionD_Text;

    [SerializeField] private GameObject greateJobPanel;
    [SerializeField] private GameObject tryAgainJobPanel;
     

    private List<Question> AllAvailableQuestions = new List<Question>();
    public void OnOptionClick(int optionId)
    {
        if(optionId == currectOptionIndex)
        {
            greateJobPanel.SetActive(true);
        }
         else
        {
           tryAgainJobPanel.SetActive(true); 
        }
    }


    //-----------------------
    public void PopulateQuizzesOnVideo(int videoId)
    {
        ClearAllText();
        StartCoroutine(LoadQuestions("video", videoId)); 
    }
    public void PopulateQuizzesOnSubCat(int subcatId)
    {
        ClearAllText();
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
                HomeMainUIController.ShowPopup.Invoke("Oops! Some error in loading questions. Please try again later", () => print("Some error"));
            }
            else
            {
                print(webRequest.downloadHandler.text);     
                GetQuetionResponse response = JsonUtility.FromJson<GetQuetionResponse>(webRequest.downloadHandler.text);
                if(response.status)
                {
                    if(response.data.questions.Count <= 0)
                    {
                        HomeMainUIController.ShowPopup.Invoke("No Questions available on this topic.", () => HomeMainUIController.EventBackClicked.Invoke());    
                    } else
                    {
                            AllAvailableQuestions = response.data.questions;
                            print(AllAvailableQuestions.Count);
                            print(AllAvailableQuestions[0].options.Count);
                            PopulateNextQuestion();
                    }
                }
                else
                {
                    HomeMainUIController.ShowPopup.Invoke(response.message, () => HomeMainUIController.EventBackClicked.Invoke());
                }

            } 
            
        } 
    }

    int queIndex = 0;
    int currectOptionIndex = 0;
    private void PopulateNextQuestion()
    {
        Question nextQuestion = AllAvailableQuestions[queIndex];
        queNumberText.text = nextQuestion.question_id.ToString();
        queBodyText.text = nextQuestion.question;
        optionA_Text.text = nextQuestion.options[0].option.ToString();
        optionB_Text.text = nextQuestion.options[1].option.ToString();
        optionC_Text.text = nextQuestion.options[2].option.ToString();
        optionD_Text.text = nextQuestion.options[3].option.ToString();

        for(int i = 0; i < nextQuestion.options.Count; i++){
            if(nextQuestion.options[i].correct_incorrect == "C")
            {
               currectOptionIndex = i+1; 
            }
        }
        print("Current option is "+currectOptionIndex);
        queIndex += 1;
    }
    public void ButtonGreatJob()
    {
        greateJobPanel.SetActive(false);
        if(AllAvailableQuestions.Count > queIndex)
        {    
            PopulateNextQuestion();
        } 
        else
        {
            HomeMainUIController.ShowPopup.Invoke("Hurray! No more questions.", () =>
                                            { 
                                                queIndex = 0;
                                                ClearAllText();
                                                HomeMainUIController.EventBackClicked.Invoke();
                                            });
        }
    }
    public void ButtonTryAgain()
    {
        tryAgainJobPanel.SetActive(false);
    }

    void ClearAllText()
    {
        queNumberText.text = "";
        queBodyText.text = "";
        optionA_Text.text = "";
        optionB_Text.text = "";
        optionC_Text.text = "";
        optionD_Text.text = "";
    }
    

}