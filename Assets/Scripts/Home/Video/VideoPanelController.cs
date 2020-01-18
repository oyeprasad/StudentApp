using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable] public class VideoResponseData : ResponseBase
{
    public VideoDataList data = new VideoDataList();
}
[System.Serializable] public class VideoData
{
    public int video_id = 0;
    public int sub_category_id = 0;
    public string video_title = "";
    public string video_path = "";
    public string sub_category_name = "";
}

[System.Serializable] public class VideoDataList
{
    public List<VideoData> videos = new List<VideoData>();
}


public class VideoPanelController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text titleText;
    [SerializeField] GameObject playVideoPanel, rePlayVideoPanel; 
    private int videoId = 0;


    public void PopulatePanel(int subCatId, string subCatName)
    {
        titleText.text = subCatName.ToUpper();
        playVideoPanel.SetActive(true);
        rePlayVideoPanel.SetActive(false); 
        HomeMainUIController.EventShowHideLoader.Invoke(true); 
        StartCoroutine(LoadVideoDetails(subCatId, OnCompleteVideoLoad));
    }

    IEnumerator LoadVideoDetails(int _subCatId, System.Action<VideoResponseData> callback)
    {
         string endpoint = WebRequests.Instance.getVideoEndPoint;
        print("request video for subcat id "+_subCatId);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(Globals.BASE_URL + endpoint+_subCatId))
        {
            // Request and wait for the desired page.
            print("URL "+webRequest.url);
            webRequest.SetRequestHeader("Accept", "application/json");//
            webRequest.SetRequestHeader("Authorization", "Bearer "+Globals.UserLoginDetails.access_token);

            yield return webRequest.SendWebRequest();
            while(!webRequest.isDone)
                yield return webRequest;
            if (webRequest.isNetworkError)
            {
              callback(null);
              print("Error! " + webRequest.responseCode);  
            }
            else
            {
                print(webRequest.downloadHandler.text);
                callback(JsonUtility.FromJson<VideoResponseData> (webRequest.downloadHandler.text));
            }
        } 
    }
    private void OnCompleteVideoLoad(VideoResponseData videoData)
    {    
        HomeMainUIController.EventShowHideLoader.Invoke(false); 

    }

    public void LowerButtonClicked(string buttonName)
    {
        switch(buttonName)
        {
            case "quizzes":
                HomeMainUIController.EventQuizzesClicked.Invoke(videoId);
                break;
            case "worksheet":
                print("Clicked for worksheet");
                HomeMainUIController.ShowPopup.Invoke("COMING SOON!", () => print("No action on worksheet clicked"));
                break;
            case "games":
                print("Clicked for games");
                HomeMainUIController.ShowPopup.Invoke("COMING SOON!", () => print("No action on worksheet clicked"));
                break;
            case "help":
                print("Clicked for help");
                HomeMainUIController.ShowPopup.Invoke("COMING SOON!", () => print("No action on worksheet clicked"));
                break;
        }
    }

}
