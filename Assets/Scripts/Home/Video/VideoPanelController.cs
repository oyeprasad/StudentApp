using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.Events;
 
[System.Serializable] public class VideoResponseData : ResponseBase
{
    public VideoDataList data = new VideoDataList();
}
[System.Serializable] public class VideoData
{
    public int video_id = 0;
    public int sub_category_id = 0;
    public string video_type = "local_disk";
    public string video_title = "";
    public string video_path = "";
    public string video_thumbnail = ""; 
    public string sub_category_name = "";
}

[System.Serializable] public class VideoDataList
{
    public List<VideoData> videos = new List<VideoData>();
}


public class VideoPanelController : MonoBehaviour
{ 
    public static UnityEvent EventVideoFinish = new UnityEvent();
    [SerializeField] private Text titleText;
    [SerializeField] GameObject playVideoPanel, rePlayVideoPanel; 
    [SerializeField] private Slider videoDownloadProgress;
    [SerializeField] private Text progressText;
    private int videoId = 0;
    private int _subCatId;
    private List<string> videosLocalPath = new List<string>();

    private VideoData currentvideoData;
    void Start()
    {
        EventVideoFinish.AddListener(OnVideoFinished);
    } 
    public void PlayNewVideo(VideoData data)
    {
        print("Play new video at "+data.video_path);
         titleText.text = data.video_title;
        currentvideoData = data;
        videoId = data.video_id;

        rePlayVideoPanel.SetActive(false);
        playVideoPanel.SetActive(true);
        
        if(!string.Equals(data.video_type, "local_disk"))
        {
            YoutubePlayer.YoutubePlayer youtubePlayer = new YoutubePlayer.YoutubePlayer();
            youtubePlayer.GetVideoResolvedUrl(data.video_path, OnYoutubeUrlResolved);
        } 
        else
        {
            print("Download Video");
         StartCoroutine(DownloadVideo(currentvideoData.video_path, currentvideoData.video_id+"_video.mp4",OnVideoDownloaded));   
          //  playVideoPanel.GetComponent<MainVideoPlayer>().PlayNewVideo(data.video_path);
        }
    } 
   
    public void LowerButtonClicked(string buttonName)
    {
        switch(buttonName)
        {
            case "quizzes":
                HomeMainUIController.EventQuizzesClicked.Invoke(currentvideoData.video_id);
                playVideoPanel.GetComponent<MainVideoPlayer>()._isPlaying = true;
                break;
            case "worksheet":
                print("Clicked for worksheet");
                 HomeMainUIController.EventWorkSheetClicked.Invoke(_subCatId);
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

    /*void ResolveYoutubeUrl(string YoutubeUrl, string videoname)
    {
        print("Youtube URL to resolve "+YoutubeUrl);
        YoutubePlayer.YoutubePlayer youtubePlayer = new YoutubePlayer.YoutubePlayer();
        
        print("video name "+videoname);
        youtubePlayer.GetVideoResolvedUrl(YoutubeUrl, videoname,OnYoutubeUrlResolved);
    }*/

    void OnYoutubeUrlResolved(string resolvedUrl)
    {  
        //print("Youtube video resolved "+resolvedUrl);
        //playVideoPanel.GetComponent<MainVideoPlayer>().PlayNewVideo(resolvedUrl);
        StartCoroutine(DownloadVideo(resolvedUrl, currentvideoData.video_id+"_video.mp4",OnVideoDownloaded));
    }

    IEnumerator DownloadVideo(string path, string videoname, System.Action<string> callback)
    {  
        print("Trying video download at "+path);
        string destinationPath = Path.Combine(Application.persistentDataPath, videoname);
        if(File.Exists(Path.Combine(Application.persistentDataPath, videoname)))
        {
            print("Already Downlaoded");
            callback(destinationPath);
        }
        UnityWebRequest www = UnityWebRequest.Get(path);
        print("Downloading");
        StartCoroutine(ShowProgress(www));
        yield return www.SendWebRequest();
        
        if(www.isNetworkError || www.isHttpError) {
            print(www.responseCode);
            print(www.error);
            callback(string.Empty);
        } else {
            print("Writing...  "+videoname);
            File.WriteAllBytes(destinationPath, www.downloadHandler.data);
            callback(destinationPath);
        }
    }
    
    IEnumerator ShowProgress(UnityWebRequest www)
    {
        while(!www.isDone)
        {
            yield return new WaitForSeconds(0.1f);
            progressText.text = ""+Mathf.CeilToInt(www.downloadProgress * 100)  +" %";
            videoDownloadProgress.value = www.downloadProgress;
        }
    }

    void OnVideoDownloaded(string path)
    {
        print("videosLocalPath downloaded at "+ path);
        playVideoPanel.GetComponent<MainVideoPlayer>().PlayNewVideo(path);
    }

    void OnVideoFinished()
    {
         rePlayVideoPanel.SetActive(true);
         playVideoPanel.SetActive(false);
    }
    public void ReplayClicked()
    {
        rePlayVideoPanel.SetActive(false);
        playVideoPanel.SetActive(true);
        playVideoPanel.GetComponent<MainVideoPlayer>().Replay();
    }
    public void QuizClicked()
    {
        HomeMainUIController.EventQuizzesClicked.Invoke(currentvideoData.video_id);
    }
 

}
