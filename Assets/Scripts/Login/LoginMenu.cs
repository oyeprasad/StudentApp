﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.Events;

public class LoginMenu : MonoBehaviour
{
    //  Events
    public static UserRegisterEvent userRegisterEvent = new UserRegisterEvent();
    public static StringEvent OTPSubmitEvent = new StringEvent();
    public static UserLoginEvent LoginButtonClickEvent = new UserLoginEvent();
    public static UnityEvent RegisterButtonClickEvent = new UnityEvent();
    public static StringEvent ForgotPasswordSubmit = new StringEvent();
    public static UnityEvent FbButtonClickEvent = new UnityEvent();
    public static UnityEvent GoogleButtonClickEvent = new UnityEvent();
    public static UnityEvent ForgotPasswordButtonClickEvent = new UnityEvent();
    public static UnityEvent InputFieldEditStart = new UnityEvent();
    public static StringEvent BackFromPanelEvent = new StringEvent();

    public static UnityEvent LoginNextClickEvent = new UnityEvent();

    [SerializeField] private string registerEndPoint, regOTPEndPoint, forgotPasswordEndPoint, loginEndPoint;

    [SerializeField] private GameObject LoginPanel, RegisterPanel, ForgotPassordPanel, OtpPanel, LoadingScreen, UsernamePanel, PasswordPanel; 

    private void Start()
    {
        LoginMenu.userRegisterEvent.AddListener(OnUserRegister);
        LoginMenu.OTPSubmitEvent.AddListener(OnSubmitOtp);
        LoginMenu.LoginButtonClickEvent.AddListener(OnLoginClicked);
        LoginMenu.RegisterButtonClickEvent.AddListener(RegisterButtonClicked);
        LoginMenu.ForgotPasswordButtonClickEvent.AddListener(ForgotPasswordButtonClicked);
        LoginMenu.ForgotPasswordSubmit.AddListener(OnForgotPasswordSubmit);
        LoginMenu.BackFromPanelEvent.AddListener(BackFromPanel);

        //Modified
        LoginMenu.LoginNextClickEvent.AddListener(LoginNextClick);
    }

    void LoginNextClick()
    {
        ActivatePanel(PasswordPanel.name);
    }

    public void ActivatePanel(string panelName)
    {
        print("Activate panel "+panelName);
        //LoginPanel.SetActive(string.Equals(LoginPanel.name, panelName));
        RegisterPanel.SetActive(string.Equals(RegisterPanel.name, panelName));
        ForgotPassordPanel.SetActive(string.Equals(ForgotPassordPanel.name, panelName));
        OtpPanel.SetActive(string.Equals(OtpPanel.name, panelName));
        LoadingScreen.SetActive(string.Equals(LoadingScreen.name, panelName));
        PasswordPanel.SetActive(string.Equals(PasswordPanel.name, panelName));
    }

    void BackFromPanel(string panelName)
    {
        ActivatePanel(panelName);
    }
    public void SuccessfullRegistered()
    {
        Globals.LoadLevel(Globals.MAIN_SCENE);
    }

    #region Login



    private void OnLoginClicked(UserLoginData userLoginData)
    {
        /*
        ActivatePanel(LoadingScreen.name);
        WWWForm form = new WWWForm();
        form.AddField("user_name", userLoginData.Username);
        form.AddField("password", userLoginData.Password); 

        Communications<LoginResponseData> regCommunication = new Communications<LoginResponseData>();
        regCommunication.PostForm(loginEndPoint, form, LoginCallback);
        */

        // for now no communication direct go to next
        LoginCallback(null);

    }

    void LoginCallback(LoginResponseData loginResponseData)
    {
        Globals.LoadLevel(Globals.MAIN_SCENE);
    }
    #endregion Login

    #region Register
    void RegisterButtonClicked()
    {
        ActivatePanel(RegisterPanel.name);
    }

    void OnUserRegister(UserRegisterData userData)
    {
        /*
        ActivatePanel(LoadingScreen.name);
        WWWForm form = new WWWForm();
        form.AddField("first_name", userData.FirstName);
        form.AddField("last_name", userData.LastName);
        form.AddField("email", userData.Email);

        Communications<RegisterResponseData> regCommunication = new Communications<RegisterResponseData>();
        regCommunication.PostForm(registerEndPoint, form, RegCallback);
        */

        // for now no communication direct go to next
        RegCallback(null);
    }

    private void RegCallback(RegisterResponseData obj)
    {
        // implement obj and check if response is correct, ignore this step for now.
        print("Activate otp");
        ActivatePanel(OtpPanel.name);
    }
    #endregion Register

    #region OTP

    private void OnSubmitOtp(string otp)
    {
        /*
        ActivatePanel(LoadingScreen.name);
        WWWForm form = new WWWForm();
        form.AddField("otp", otp);

        Communications<OTPResponseData> regCommunication = new Communications<OTPResponseData>();
        regCommunication.PostForm(regOTPEndPoint, form, OtpSubmitCallback);
        */
        OtpSubmitCallback(null);
    }

    private void OtpSubmitCallback(OTPResponseData obj)
    {
         
    }
    #endregion OTP

    #region ForgotPassword

    void ForgotPasswordButtonClicked()
    {
        ActivatePanel(ForgotPassordPanel.name);
    }

    void OnForgotPasswordSubmit(string email_id)
    {/*
        ActivatePanel(LoadingScreen.name);
        WWWForm form = new WWWForm();
        form.AddField("email", email_id);  

        Communications<ForgotPasswordResponseData> regCommunication = new Communications<ForgotPasswordResponseData>();
        regCommunication.PostForm(forgotPasswordEndPoint, form, ForgotpasswordCallback);
        
        */
        // for now no communication direct go to next
        ForgotpasswordCallback(null);
    }

    private void ForgotpasswordCallback(ForgotPasswordResponseData obj)
    {
        ActivatePanel(LoginPanel.name);
    }
    #endregion ForgotPassword
}
