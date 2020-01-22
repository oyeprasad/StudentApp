using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HelpPanel : MonoBehaviour
{
    [SerializeField] private InputField HelpInputText;
    void OnEnable()
    {
        HelpInputText.text = "";
    }

    public void OnSubmit()
    {
        if(string.IsNullOrEmpty(HelpInputText.text))
        {
            HomeMainUIController.ShowPopup.Invoke("Please describe your problem.", () => print(""));
        } 
        else
        {
            HomeMainUIController.EventSubmitHelp.Invoke(HelpInputText.text);
            HelpInputText.text = "";
        }
    }

   
}
