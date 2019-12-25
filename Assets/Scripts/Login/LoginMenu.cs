using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginMenu : MonoBehaviour
{
    // Static Events
    public static UserRegisterEvent userRegisterEvent = new UserRegisterEvent();

    [SerializeField] private string registerEndPoint;

    [SerializeField] private GameObject LoginPanel, RegisterPanel, ForgotPassordPanel, OtpPanel; 

    private void Start()
    {
        userRegisterEvent.AddListener(OnUserRegister);
    }

    public void ActivatePanel(string panelName)
    {
        LoginPanel.SetActive(string.Equals(LoginPanel.name, panelName));
        RegisterPanel.SetActive(string.Equals(RegisterPanel.name, panelName));
        ForgotPassordPanel.SetActive(string.Equals(ForgotPassordPanel.name, panelName));
        OtpPanel.SetActive(string.Equals(OtpPanel.name, panelName));
    }

    void OnUserRegister(UserRegisterData userData)
    {

    }
}
