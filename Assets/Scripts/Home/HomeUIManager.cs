using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUIManager : MonoBehaviour
{
    [SerializeField] private Text UserWelcomeText;
    // Start is called before the first frame update
    void Start()
    {
        if (Globals.LoginType == 0)
        {
            UserWelcomeText.text = string.Format("{0}, {1}", "Hello", Globals.UserLoginDetails.username.ToUpper());
        }
        else if (Globals.LoginType == 1)
        {
            UserWelcomeText.text = string.Format("{0}, {1}", "Hello", Globals.fBLoginResponseData.name.ToUpper());
        }
    }
     
}
