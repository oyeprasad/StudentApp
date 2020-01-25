using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class HomePanel : MonoBehaviour
{
    [SerializeField] private Text userWelcomeText; 
    [SerializeField] private Image profilePic;



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
        HomeMainUIController.ShowPopup.Invoke("COMING SOON!", () => print("No action on worksheet clicked"));
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
