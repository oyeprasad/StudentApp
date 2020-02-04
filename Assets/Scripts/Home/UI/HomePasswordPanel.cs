using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePasswordPanel : MonoBehaviour
{    
    [SerializeField] private int panelNumber;
    [SerializeField] private ChangePassswordButtons currentlySelectedPassword;
    [SerializeField] private List<ChangePassswordButtons> AllButtonsList;
     
     void Start()
     { 
     }
    void OnEnable()
    {
        print("currentlySelectedPassword "+currentlySelectedPassword.name);
        print("OnEnable "+panelNumber+" > "+currentlySelectedPassword.id);
        OnClick(currentlySelectedPassword);
    }
    public void OnClick(ChangePassswordButtons button)
    { 
         currentlySelectedPassword = button;
         HomeMainUIController.EventPassowrdClicked.Invoke(panelNumber, button.id);
    }
    public void UpdateCurrentPassword(ChangePassswordButtons newPasswordButton)
    {
        currentlySelectedPassword = newPasswordButton;
        OnClick(currentlySelectedPassword);
    }
}
