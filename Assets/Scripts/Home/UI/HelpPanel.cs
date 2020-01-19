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
        HomeMainUIController.EventSubmitHelp.Invoke(HelpInputText.text);
    }
}
