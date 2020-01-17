using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class HomePanel : MonoBehaviour
{
    [SerializeField] private Text userWelcomeText;

    void Start()
    {
       
        if (Globals.LoginType == 0)
        {
            userWelcomeText.text = string.Format("{0}, {1}", "Hello", Globals.UserLoginDetails.username.ToUpper());
        }
        else if (Globals.LoginType == 1)
        {
            userWelcomeText.text = string.Format("{0}, {1}", "Hello", Globals.fBLoginResponseData.name.ToUpper());
        }
    }

    public void VideoClicked()
    {
        HomeMainUIController.EventVideoClicked.Invoke();
    }
    public void ProfileClicked()
    {
        HomeMainUIController.EventProfileClicked.Invoke();
    }
    public void BackClicked()
    {
        HomeMainUIController.EventBackClicked.Invoke();
    }
}
