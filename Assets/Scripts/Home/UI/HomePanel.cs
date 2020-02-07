using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;

public class HomePanel : MonoBehaviour
{
    [SerializeField] private Text userWelcomeText; 
    [SerializeField] private Image profilePic;

    void Awake()
    {
        
    }

    void Start()
    {
       
       
        if (Globals.LoginType == 0)
        {
            userWelcomeText.text = string.Format("{0}, {1}", "HELLO", Globals.UserLoginDetails.username.ToUpper());
        }
        else if (Globals.LoginType == 1)
        {
            userWelcomeText.text = string.Format("{0}, {1}", "HELLO", Globals.fBLoginResponseData.name.ToUpper());
        }

        HomeMainUIController.EventProfilePicChoose.AddListener(ProfilePicChoosen); 

print("Globals.UserLoginDetails.profile_pic "+Globals.UserLoginDetails.profile_pic  );
         if(!string.IsNullOrEmpty(Globals.UserLoginDetails.profile_pic))
        {
            if(GSC.Instance.UserProfilePic != null)
            {
                ProfilePicChoosen(GSC.Instance.UserProfilePic,GSC.Instance.userpicAspectRatio);
            }
           // StartCoroutine(DownloadProfilePic());
        }
    }

    IEnumerator DownloadProfilePic()
    { 
        print("Downloading unser image");
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(Globals.UserLoginDetails.profile_pic);   
        yield return www.SendWebRequest();
        while(!www.isDone)
        yield return null;

        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else { 
        print("Downloaded unser image");
            Texture2D myTexture = (Texture2D)((DownloadHandlerTexture)www.downloadHandler).texture;
          print("Image is " + myTexture);
            Sprite sprite =  Sprite.Create(myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            HomeMainUIController.EventProfilePicChoose.Invoke(sprite, (myTexture.width * 1.0f)/(myTexture.height * 1.0f));
        }

        
         
    }

    void ProfilePicChoosen(Sprite img, float _aspectRatio)
    {    
        profilePic.sprite = img;
        profilePic.GetComponent<AspectRatioFitter>().aspectRatio = _aspectRatio;
    }

    public void VideoClicked()
    {
        HomeMainUIController.EventVideoClicked.Invoke();
    }
    public void QuizzesClicked()
    {
        HomeMainUIController.EventQuizzesClickedFromHome.Invoke();
    }
    public void ProfileClicked()
    {
        HomeMainUIController.EventProfileClicked.Invoke();
    }
    public void GamesClicked()
    {
        HomeMainUIController.ShowPopup.Invoke("COMING SOON!", () => print("No action on worksheet clicked"));
    }
    public void WorkSheetClicked()
    {
        HomeMainUIController.EventWorkSheetClickedFromHome.Invoke();
    }
    public void PrizesClicked()
    {
        HomeMainUIController.ShowPopup.Invoke("COMING SOON!", () => print("No action on worksheet clicked"));
    }
    public void MessagesClicked()
    {
        HomeMainUIController.ShowPopup.Invoke("COMING SOON!", () => print("No action on worksheet clicked"));
    }
    public void BackClicked()
    {
        HomeMainUIController.EventBackClicked.Invoke();
    }
}
