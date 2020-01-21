using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebRequests : MonoBehaviour
{
    public static WebRequests Instance;
    private Communications communications;
    [SerializeField] private string RegistrationEndPoint = "";
    [SerializeField] private string VerifyOTPEndpoint = "";
    [SerializeField] private string LoginEndPoint = "";
    [SerializeField] private string ResendOTPEndPoint = "";
    [SerializeField] private string ForgotPasswordEndPoint = "";
    [SerializeField] private string VerifyForgotPasswordEndPoint = "";
    [SerializeField] private string StudentDetailsEndPoint = "";
    [SerializeField] private string FBLoginEndPoint = "";
    [SerializeField] private string ForgotPassNewPassEndPoint = "";
    [SerializeField] public string LogoutEndPoint = "";
    [SerializeField] public string ChangePasswordEndPoint = ""; 


    [SerializeField] public string categoryEndPoint = ""; //
    [SerializeField] public string subCategoryEndPoint = "";
    [SerializeField] public string getVideoEndPoint = "";
    [SerializeField] public string editProfileEndPoint = "";

    Action<ResponseData<UserData>> callback;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        communications = GetComponent<Communications>();
    }


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

    public void ForgotPasswordNewPasswordSubmit(int user_id, string password, string confirm_password, Action<ResponseData<UserData>> _callback)
    {
        print("password "+ password);
        print("confirm_password " + confirm_password);
        print("user_id " + user_id);
        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id);
        form.AddField("password", password);
        form.AddField("confirm_password", confirm_password);
        communications.PostForm(ForgotPassNewPassEndPoint, form, _callback);

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
        communications.PostForm(StudentDetailsEndPoint, form, _callback, WebRequestMethod.PUT);
    }
    #endregion StudentDetailsSubmit

    #region VerifyForgotPasswordOTP
    public void ProcessForgotPasswordOTP(int user_id, string otp, Action<ResponseData<UserData>> _callback)
    {
        WWWForm form = new WWWForm();
		print("user id "+user_id);
        form.AddField("user_id", user_id);
        form.AddField("otp", otp);
        communications.PostForm(VerifyForgotPasswordEndPoint, form, _callback);
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


    #region Logout
    public void ProcessLogout(int user_id, string password, string confirm_password, Action<ResponseData<UserData>> _callback)
    {
        print("password " + password);
        print("confirm_password " + confirm_password);
        WWWForm form = new WWWForm();
        form.AddField("user_id", user_id);
        form.AddField("password", password);
        form.AddField("confirm_password", confirm_password);
        communications.PostForm(LogoutEndPoint, form, _callback);

    }
    #endregion Logout



    //=============================================================================================================

    public void ProcessCategoryRequest(int _gradeNo)
    {

    }

}
