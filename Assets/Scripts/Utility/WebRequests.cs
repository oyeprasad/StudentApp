using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebRequests : MonoBehaviour
{ 
    public Communications communications;
    [SerializeField] private string RegistrationEndPoint = "";
    [SerializeField] private string VerifyOTPEndpoint = "";
    [SerializeField] private string LoginEndPoint = "";
    [SerializeField] private string ResendOTPEndPoint = "";
    [SerializeField] private string ForgotPasswordEndPoint = "";
    [SerializeField] private string VerifyForgotPasswordEndPoint = "";
    [SerializeField] private string StudentDetailsEndPoint = "";
    [SerializeField] private string FBLoginEndPoint = "";

    Action<ResponseData<UserData>> callback;

    #region Registration
    public void ProcessSignUp(string name, string country_code, string phone, string email, Action<ResponseData<UserData>> _callback)
    { 
        WWWForm wwwform = new WWWForm();
        wwwform.AddField("name", name);
        wwwform.AddField("country_code", country_code);
        wwwform.AddField("phone", phone);
        wwwform.AddField("email", email);

        communications.PostForm(RegistrationEndPoint, wwwform, _callback);
    }

    #endregion Registration
    #region OTP
    public void ProcessOTP(int user_id, string otp, Action<ResponseData<UserData>> _callback)
    {
        callback = _callback;
        WWWForm wwwform = new WWWForm();
        wwwform.AddField("user_id", user_id);
        wwwform.AddField("otp", otp);
        communications.PostForm(VerifyOTPEndpoint, wwwform, _callback);
    }
    #endregion OTP

    #region Login
    public void ProcessLogin(string username, string password, Action<ResponseData<UserData>> _callback)
    {
        WWWForm form = new WWWForm();

        form.AddField("username", username);
        form.AddField("password", password);

        communications.PostForm(LoginEndPoint, form, _callback);
    }
    #endregion Login

    #region ResendOTP
    public void ProcessResendOTP(int user_id, Action<ResponseData<UserData>> _callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id); 
        communications.PostForm(ResendOTPEndPoint, form, _callback);
    }
    #endregion ResendOTP

    #region ForgotPassword
    public void ProcessForgotPassword(string email, Action<ResponseData<UserData>> _callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        communications.PostForm(ForgotPasswordEndPoint, form, _callback);
    }
    #endregion ForgotPassword

    #region StudentDetailsSubmit
    public void ProcessStudentDetailsSubmit(int user_id, string username, string email,string password, int grade, Action<ResponseData<UserData>> _callback)
    {
        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id);
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("grade", grade);
        communications.PostForm(StudentDetailsEndPoint, form, _callback);
    }
    #endregion StudentDetailsSubmit

    #region VerifyForgotPasswordOTP
    public void ProcessForgotPasswordOTP(int user_id, string otp, Action<ResponseData<UserData>> _callback)
    {
        {
            WWWForm form = new WWWForm();
            form.AddField("user_id", user_id);
            form.AddField("otp", otp);
            communications.PostForm(VerifyForgotPasswordEndPoint, form, _callback);
        }
    }
    #endregion VerifyForgotPasswordOTP

    #region FBLogin

    public void ProcessFBLogin(string accessToken, Action<ResponseData<FBLoginResponseData>> _callback)
    {
        print(accessToken);
        WWWForm form = new WWWForm(); 
        form.AddField("accessToken", accessToken);
        communications.PostForm(FBLoginEndPoint, form, _callback);
    }
    #endregion FBLogin


}
