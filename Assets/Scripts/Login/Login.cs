using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OTPInitiator
{
    Login,
    ForgotPassword,
    Registration
}

public class Login : MonoBehaviour
{

    // static events
    public static StringEvent PasswordClickEvent = new StringEvent();
    public static StringEvent OTPSubmitEvent = new StringEvent(); //
    public static IntEvent GradeClickEvent = new IntEvent();
    public static StringEvent FBLoginEvent = new StringEvent();

     
    [SerializeField] private InputField usernameInput, EmailFPInput, RegFullname, RegEmail, RegPhomenumber, ChooseUsernameInput;
    [SerializeField] private Dropdown RegPhoneCode;

     
    [SerializeField] private GameObject LoginPanel, PasswordPanel, ForgotPassowrdPanel, WhoAreYouPanel, VerificationPanel, SignUpPanel, ChooseUserNamePanel, GradePanel,LoaderPanel;
    [SerializeField] private LoginPopup loginPopup;
    [SerializeField] private FBManager fbManager;

     
    [SerializeField] private Text LoginScreenMessage, PasswordScreenMessage, RegisScreenMessage, VerificationScreenMessage, ForgotPasswordScreenMessage, GradeScreenMessage;

    [SerializeField] private WebRequests WebRequestObject;

    private string username, passwordId = "1", emailid, fullname, phonecode, phonenumber;
    private int user_id = 0, grade;

    private List<GameObject> navigationPanelsList = new List<GameObject>();
    private OTPInitiator oTPInitiator = OTPInitiator.Login;
    #region MonoBehaviourMethods
    void Start()
    {
        Login.PasswordClickEvent.AddListener(PasswordButtonClicked);
        Login.OTPSubmitEvent.AddListener(OTPSubmit);
        Login.GradeClickEvent.AddListener(GradeSubmit);
        Login.FBLoginEvent.AddListener(OnFBLogin);
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackFromPanel();
        }
    }
    #endregion MonoBehaviourMethods


    #region ButtonClickEvents
    public void LoginNextClick()
    {
        if (string.IsNullOrEmpty(usernameInput.text))
        { 
            usernameInput.GetComponent<ValidateInput>().Validate(string.Empty);
            return;
        }
        if (ValidateUserName(usernameInput.text))
        {
            username = usernameInput.text;
            usernameInput.text = string.Empty;
            EmailFPInput.text = string.Empty;
            navigationPanelsList.Add(LoginPanel);  

            ClearInpuFields();
            ActivatePanel(PasswordPanel.name);    
        } 
    }
    public void RegisterClicked()
    {
        oTPInitiator = OTPInitiator.Registration;
        navigationPanelsList.Add(LoginPanel); 
        ClearInpuFields();
        ActivatePanel(WhoAreYouPanel.name);
    }

    public void ResendOtpClicked()
    {
        WebRequestObject.ProcessResendOTP(user_id, ResendOTPCallback);
    }

    private void ResendOTPCallback(ResponseData<UserData> obj)
    {
        if (obj != null)
        {
            if (obj.status)
            {
                VerificationScreenMessage.color = Color.green;
                VerificationScreenMessage.text = obj.message;
            }
            else {

                VerificationScreenMessage.color = Color.red;
                VerificationScreenMessage.text = obj.message;
            }
        }
        else
        {
            VerificationScreenMessage.color = Color.red;
            VerificationScreenMessage.text = "Some error! Please try after some time.";
        }
    }

    public void UsertypeClicked(string usertype)
    {
        if (string.Equals(usertype ,"student"))
        {
            ClearInpuFields();
            navigationPanelsList.Add(WhoAreYouPanel); 
            ActivatePanel(SignUpPanel.name);

            loginPopup.gameObject.SetActive(true);
            loginPopup.SetPopup("DISCLAIMER"+"\n\n"+"Dear Student, please have your parent or and adult help you to create an account", null);
        }
    }

    public void ForgotPasswordClick()
    {
        navigationPanelsList.Add(LoginPanel); 
        ActivatePanel(ForgotPassowrdPanel.name);    
    }
    public void PasswordSubmit()
    {
        // perform login here with username and password available 
        if (oTPInitiator == OTPInitiator.Login)
        {
            LoaderPanel.SetActive(true);
            WebRequestObject.ProcessLogin(username, passwordId, LoginCallback);
        }
        if (oTPInitiator == OTPInitiator.ForgotPassword)
        {
            print("Passowrd submit1");
            Globals.USERNAME = username;
            ClearInpuFields();
            navigationPanelsList.Clear();
             
            Globals.LoadLevel(Globals.HOME_SCENE);
        }
        else if (oTPInitiator == OTPInitiator.Registration)
        {
            //navigationPanelsList.Clear();
            ClearInpuFields();
            ActivatePanel(GradePanel.name); 
        }

    }

    private void LoginCallback(ResponseData<UserData> obj)
    {
        LoaderPanel.SetActive(false);
        if (obj == null)
        {
            PasswordScreenMessage.text = "Some error in login! Please try after some time.";
        }
        if (obj != null)
        {
            if (obj.status)
            {
                ClearInpuFields();
                navigationPanelsList.Clear();
                Globals.UserLoginDetails = obj.data; 
                Globals.LoadLevel(Globals.HOME_SCENE);
            }
            else
            {
                PasswordScreenMessage.text = obj.message;
            }

        }
    }

    public void ForgotPasswordSubmit()
    {
        if (string.IsNullOrEmpty(EmailFPInput.text))
        {
            EmailFPInput.GetComponent<ValidateInput>().Validate(string.Empty);
        }
        else if (EmailFPInput.GetComponent<ValidateInput>().isValidInput)
        {
            emailid = EmailFPInput.text;

            WebRequestObject.ProcessForgotPassword(emailid, ForgotPasswordSubmitCallback);


        }
    }

    private void ForgotPasswordSubmitCallback(ResponseData<UserData> obj)
    {
        if (obj == null)
        {
            ForgotPasswordScreenMessage.text = "Some error! Please try after some time.";
        }
        else
        {
            if (obj.status)
            {
                navigationPanelsList.Add(ForgotPassowrdPanel);
                ClearInpuFields();
                oTPInitiator = OTPInitiator.ForgotPassword;
                ActivatePanel(VerificationPanel.name);
            }
            else
            {
                ForgotPasswordScreenMessage.text = obj.message;
            }
        }
    }

    public void OTPSubmit(string otp_intered)
    { 
         
        ClearInpuFields();
        LoaderPanel.SetActive(true);
         
        if (oTPInitiator == OTPInitiator.ForgotPassword) {

            WebRequestObject.ProcessForgotPasswordOTP(user_id, otp_intered, ForgotPassOTPCallback);

            navigationPanelsList.Add(VerificationPanel);
            ActivatePanel(PasswordPanel.name);
        }
         else if (oTPInitiator == OTPInitiator.Registration) {
            WebRequestObject.ProcessOTP(user_id, otp_intered, OTPSubmitCallback);
        }

    }

    private void ForgotPassOTPCallback(ResponseData<UserData> obj)
    {
        if (obj != null)
        {
            if (obj.status)
            {
                ClearInpuFields();
                navigationPanelsList.Add(VerificationPanel);
                oTPInitiator = OTPInitiator.ForgotPassword;
                ActivatePanel(PasswordPanel.name);
            }
            else
            {
                VerificationScreenMessage.text = obj.message;
            }
        }
    }

    private void OTPSubmitCallback(ResponseData<UserData> obj)
    {
        LoaderPanel.SetActive(false);
 
        if (obj != null)
        {
            if (obj.status)
            {
                navigationPanelsList.Add(VerificationPanel);

                if (oTPInitiator == OTPInitiator.ForgotPassword)
                    ActivatePanel(PasswordPanel.name);
                else if (oTPInitiator == OTPInitiator.Registration)
                    ActivatePanel(ChooseUserNamePanel.name);
            }
            else
            {
                VerificationScreenMessage.text = obj.message;
            }
        }
        else
        {
            VerificationScreenMessage.text = "Some error! Please try after some time.";
        }
    }

    public void SignUpSubmit()
    { 
        if (string.IsNullOrEmpty(RegFullname.text) || !RegFullname.GetComponent<ValidateInput>().isValidInput)
        {
            RegFullname.GetComponent<ValidateInput>().Validate(RegFullname.text);
        }
        else if (string.IsNullOrEmpty(RegEmail.text) || !RegEmail.GetComponent<ValidateInput>().isValidInput)
        {
            RegEmail.GetComponent<ValidateInput>().Validate(RegEmail.text);
        }
        else if (string.IsNullOrEmpty(RegPhomenumber.text) || !RegPhomenumber.GetComponent<ValidateInput>().isValidInput)
        {
            RegPhomenumber.GetComponent<ValidateInput>().Validate(RegPhomenumber.text);
        } else 
        {
            print("Signup Validate true");
            emailid = RegEmail.text;
            fullname = RegFullname.text;
            phonecode = RegPhoneCode.value.ToString();
            phonenumber = RegPhomenumber.text;

            LoaderPanel.gameObject.SetActive(true);
            WebRequestObject.ProcessSignUp(fullname, phonecode, phonenumber, emailid, RegisterCallback); 
        }
         
    }
    void RegisterCallback(ResponseData<UserData> response)
    {
        LoaderPanel.gameObject.SetActive(false);
        if (response == null)
        {
            RegisScreenMessage.text = "Some error! Please try after some time.";
        }
        else
        {
            if (response.status)
            {
                user_id = response.data.user_id;
                navigationPanelsList.Add(SignUpPanel);
                ClearInpuFields();
                oTPInitiator = OTPInitiator.Registration;
                ActivatePanel(VerificationPanel.name);
            }
            else
            {
                RegisScreenMessage.text = response.message;
            }
        }
    }

    public void ChooseUsernameSubmit()
    {
        if (string.IsNullOrEmpty(ChooseUsernameInput.text) || !ChooseUsernameInput.GetComponent<ValidateInput>().isValidInput)
        {
            ChooseUsernameInput.GetComponent<ValidateInput>().Validate(ChooseUsernameInput.text);
        }
        else if (ValidateUserName(ChooseUsernameInput.text))
        {
            username = ChooseUsernameInput.text;
            ClearInpuFields();
            navigationPanelsList.Add(ChooseUserNamePanel);
            ActivatePanel(PasswordPanel.name);
        }
    }

    private void GradeSubmit(int gradeNo)
    {
        if (GradeIsAvailable(gradeNo))
        {
            grade = gradeNo;

            // Api for submit student details
            LoaderPanel.SetActive(true);
            WebRequestObject.ProcessStudentDetailsSubmit(user_id, username, emailid,passwordId, grade, StudentDetailsSubmitCallback); 
        }
        else
        {
            loginPopup.gameObject.SetActive(true);
            loginPopup.SetPopup("COMING SOON! TRY ANOTHER GRADE!", null);
        }
    }

    private void StudentDetailsSubmitCallback(ResponseData<UserData> obj)
    {
        LoaderPanel.SetActive(false);
        if (obj != null)
        {
            if (obj.status)
            {
                print(">>>\n"+JsonUtility.ToJson(obj));
                Globals.UserLoginDetails = obj.data;
                Globals.USERNAME = obj.data.username;
                navigationPanelsList.Clear();
                ClearInpuFields();
                ActivatePanel(LoginPanel.name);
                Globals.LoadLevel(Globals.HOME_SCENE);
            }
            else
            {
                GradeScreenMessage.text = obj.message;
            }
        }
        else
        {
            GradeScreenMessage.text = "Some error! Please try after some time."; 
        }
    }

    public void LoginClicked()
    {
        navigationPanelsList.Clear();
        ActivatePanel(LoginPanel.name);
    }

    private void AfterFBDatarecieved(FBUserData data)
    {
    }

    public void FbLoginClicked()
    {
        fbManager.FBLogin();
    }
    public void GoogleLoginClicked()
    {

    }

    public void PasswordButtonClicked(string _passwordId)
    {
        passwordId = _passwordId;
    }
    #endregion ButtonClickEvents

    #region FBLogin
    void OnFBLogin(string token)
    {
        LoaderPanel.SetActive(true);
        WebRequestObject.ProcessFBLogin(token, FBLoginCallback);
    }

    private void FBLoginCallback(ResponseData<FBLoginResponseData> obj)
    {
        LoaderPanel.SetActive(false);
        if (obj != null)
        {
            if (obj.status)
            {
                Globals.fBLoginResponseData = obj.data;
                Globals.LoginType = 1;
                navigationPanelsList.Clear();
                Globals.LoadLevel(Globals.HOME_SCENE);
            }
            else
            {
                LoginScreenMessage.text = obj.message;
            }
        }
        else
        {
            LoginScreenMessage.text  = "Some error! Please try after some time.";
        }
    }
    #endregion FBLogin



    private void ActivatePanel(string panelName)
    {
        if (string.IsNullOrEmpty(panelName))
        {
            return;
        }

        LoginPanel.SetActive(string.Equals(LoginPanel.name, panelName));
        PasswordPanel.SetActive(string.Equals(PasswordPanel.name, panelName));
        ForgotPassowrdPanel.SetActive(string.Equals(ForgotPassowrdPanel.name, panelName));
        WhoAreYouPanel.SetActive(string.Equals(WhoAreYouPanel.name, panelName));
        VerificationPanel.SetActive(string.Equals(VerificationPanel.name, panelName));
        SignUpPanel.SetActive(string.Equals(SignUpPanel.name, panelName));
        ChooseUserNamePanel.SetActive(string.Equals(ChooseUserNamePanel.name, panelName));
        GradePanel.SetActive(string.Equals(GradePanel.name, panelName));
        LoaderPanel.SetActive(string.Equals(LoaderPanel.name, panelName)); 
    }


    public void BackFromPanel()
    {
        ClearInpuFields();
        if (navigationPanelsList.Count > 0)
        {
            string targt = navigationPanelsList[navigationPanelsList.Count - 1].name;
            navigationPanelsList.RemoveAt(navigationPanelsList.Count - 1) ;
            ActivatePanel(targt);
        } 
        ClearInpuFields();
    }
    bool ValidateUserName(string username)
    {
        return true;
    }

    bool GradeIsAvailable(int _gradeNo)
    {
        return true;
    }
    void ClearInpuFields()
    {
        usernameInput.text = string.Empty;
        EmailFPInput.text = string.Empty;
        RegFullname.text = string.Empty;
        RegEmail.text = string.Empty;
        RegPhomenumber.text = string.Empty;
        ChooseUsernameInput.text = string.Empty;
        RegPhoneCode.RefreshShownValue();

        LoginScreenMessage.text = "";
        PasswordScreenMessage.text = "";
        RegisScreenMessage.text = "";

        if (loginPopup.gameObject.activeInHierarchy)
        {
            loginPopup.gameObject.SetActive(false);
        }
    }
}
