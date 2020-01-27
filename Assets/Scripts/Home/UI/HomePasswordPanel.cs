using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePasswordPanel : MonoBehaviour
{  
    [SerializeField] private string panelName = ""; 
    [SerializeField] private string passwordSelected;

    void OnEnable()
    {
        HomeMainUIController.EventPasswordPanelHide.Invoke(panelName);
        PasswordClicked(passwordSelected);
    }

    public void SubmitClicked()
    {
         
    }
    public void PasswordClicked(string _password)
    {
        passwordSelected = _password;
        if(panelName == "oldpassword")
        {
            HomeMainUIController.EventPassowrdClicked.Invoke(0, passwordSelected);
        } else if(panelName == "newpassword")
        {
            HomeMainUIController.EventPassowrdClicked.Invoke(1, passwordSelected);

        } else if(panelName == "confirmpassword")
        {
            HomeMainUIController.EventPassowrdClicked.Invoke(2, passwordSelected);
        }
         
    }
}
