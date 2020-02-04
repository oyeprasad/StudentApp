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

    [SerializeField] private Color colorRed;
    [SerializeField] private Color colorGreen;
     // References of Input fields taken from inspector
    [SerializeField] private InputField usernameInput, EmailFPInput, RegFullname, RegEmail, RegPhomenumber, ChooseUsernameInput;
    [SerializeField] private Dropdown RegPhoneCode;

    // References of the Different panels taken from Inspector 
    [SerializeField] private GameObject LoginPanel, PasswordPanel, ForgotPassowrdPanel, WhoAreYouPanel, VerificationPanel, SignUpPanel, ChooseUserNamePanel, GradePanel,LoaderPanel;

    // Reference ofloginPopup and FB manager
    [SerializeField] private Popup loginPopup;
    [SerializeField] private ConfirmationPopup confirmationPopup;
    [SerializeField] private FBManager fbManager;

    // References of the screen messages that apears on different screens 
    [SerializeField] private Text LoginScreenMessage, PasswordScreenMessage, RegisScreenMessage, VerificationScreenMessage, ForgotPasswordScreenMessage, GradeScreenMessage;

    //Reference of Webrequest Panel
    [SerializeField] private WebRequests WebRequestObject;

    // User Basic info that intered or populated 
    private string username, passwordId = "1112121", emailid, fullname, phonecode, phonenumber;
    private int user_id = 0, grade;


    //List of panels that user visits seuentially 
    private List<GameObject> navigationPanelsList = new List<GameObject>();

    // Purpose of the OTP 
    private OTPInitiator oTPInitiator = OTPInitiator.Login;


    #region MonoBehaviourMethods
    void Start()
    {
        Login.PasswordClickEvent.AddListener(PasswordButtonClicked);
        Login.OTPSubmitEvent.AddListener(OTPSubmit);
        Login.GradeClickEvent.AddListener(GradeSubmit);
        Login.FBLoginEvent.AddListener(OnFBLogin);
        WebRequestObject = WebRequests.Instance;
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
            usernameInput.Select();
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
            PasswordPanel.GetComponent<PasswordPanel>().Populate("WHAT'S YOUR PASSWORD?");    
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
        LoaderPanel.SetActive(true);
        print("ResendOtpClicked user id "+user_id);
        WebRequestObject.ProcessResendOTP(user_id, ResendOTPCallback);
    }

    private void ResendOTPCallback(ResponseData<UserData> obj)
    {
        LoaderPanel.SetActive(false);
        if (obj != null)
        {
            if (obj.status)
            {
                VerificationScreenMessage.color = colorGreen;
                VerificationScreenMessage.text = obj.message;
            }
            else {
                print("Resend otp response : "+obj.message);
                VerificationScreenMessage.color = colorRed;
                VerificationScreenMessage.text = obj.message;
            }
        }
        else
        {
            VerificationScreenMessage.color = colorRed;
            VerificationScreenMessage.text = "Some error! Please try after some time.";
        }
    }

    public void UsertypeClicked(string usertype)
    {
        if (string.Equals(usertype ,"student"))
        {
            loginPopup.SetPopup("DISCLAIMER" + "\n\n" + "Dear Student, please have your parent or an adult help you to create an account!", () => {
                ClearInpuFields();
                navigationPanelsList.Add(WhoAreYouPanel);
                ActivatePanel(SignUpPanel.name);
            }); 

          }
          else if (string.Equals(usertype, "parent"))
          {
              loginPopup.SetPopup("Under construction.", () => { 
                });
          }
          else if (string.Equals(usertype, "teacher"))
          {
              loginPopup.SetPopup("Under construction.", () => { 
                });
          }
          else if (string.Equals(usertype, "schoolleader"))
          {
              loginPopup.SetPopup("Under construction.", () => { 
                });
          }
    }

    public void ForgotPasswordClick()
    {
        oTPInitiator = OTPInitiator.ForgotPassword;
        navigationPanelsList.Add(LoginPanel); 
        ActivatePanel(ForgotPassowrdPanel.name);    
    }
    public void PasswordSubmit()
    {
        // perform login here with username and password available 
        print("oTPInitiator "+oTPInitiator);
        if (oTPInitiator == OTPInitiator.Login)
        {

            LoaderPanel.SetActive(true);
            WebRequestObject.ProcessLogin(username, passwordId, LoginCallback);
        }
        if (oTPInitiator == OTPInitiator.ForgotPassword)
        { 
            LoaderPanel.SetActive(true);
            WebRequestObject.ForgotPasswordNewPasswordSubmit(user_id, passwordId, passwordId, PasswordSubmitCallback);

        }
        else if (oTPInitiator == OTPInitiator.Registration)
        {
            //navigationPanelsList.Clear();
            ClearInpuFields();
            ActivatePanel(GradePanel.name); 
        }

    }

    private void PasswordSubmitCallback(ResponseData<UserData> obj)
    {
        LoaderPanel.SetActive(false);
        if (oTPInitiator == OTPInitiator.ForgotPassword)
        {
            if (obj != null)
            {
                if (obj.status)
                { 
                    oTPInitiator = OTPInitiator.Login;
                    ClearInpuFields();
                    navigationPanelsList.Clear();
					loginPopup.SetPopup(obj.message, () =>
						{
							ActivatePanel(LoginPanel.name);
						});
                }
                else
                {
                    PasswordScreenMessage.text = obj.message; 
                }
            } else
            {
                PasswordScreenMessage.text = "Some error! Please try after some time.";
            }
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
                Globals.SaveUserData(obj.data);
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
            LoaderPanel.SetActive(true);
            WebRequestObject.ProcessForgotPassword(emailid, ForgotPasswordSubmitCallback);  
        }
    }

    private void ForgotPasswordSubmitCallback(ResponseData<UserData> obj)
    {
        LoaderPanel.SetActive(false);
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
                user_id = obj.data.user_id;

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
            print("user_id "+ user_id);
            print("otp_intered " + otp_intered);

            WebRequestObject.ProcessForgotPasswordOTP(user_id, otp_intered, ForgotPassOTPCallback);

            navigationPanelsList.Add(VerificationPanel);
            
        }
         else if (oTPInitiator == OTPInitiator.Registration) {
            print("user_id reg "+ user_id);
            print("otp_intered reg " + otp_intered);
            WebRequestObject.ProcessOTP(user_id, otp_intered, OTPSubmitCallback);
        }

    }

    private void ForgotPassOTPCallback(ResponseData<UserData> obj)
    {
        LoaderPanel.SetActive(false);
        if (obj != null)
        {
            if (obj.status)
            {
                ClearInpuFields();
                navigationPanelsList.Add(VerificationPanel); 
                ActivatePanel(PasswordPanel.name);
                PasswordPanel.GetComponent<PasswordPanel>().Populate("CHOOSE YOUR PASSWORD");
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
                {
                    ActivatePanel(PasswordPanel.name);
                    PasswordPanel.GetComponent<PasswordPanel>().Populate("CHOOSE YOUR PASSWORD");
                }
                else if (oTPInitiator == OTPInitiator.Registration)
                {

                    ActivatePanel(ChooseUserNamePanel.name);
                }
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

    public void OnValueChangeRegPhone(string _value)
    {
        // Validate phone number length
        switch (RegPhoneCode.value)
        {

        }
    }
    public void SignUpSubmit()
    { 
        if (string.IsNullOrEmpty(RegFullname.text) || !RegFullname.GetComponent<ValidateInput>().isValidInput)
        {
            RegFullname.GetComponent<ValidateInput>().Validate(RegFullname.text);
            RegFullname.Select();
        }
        else if (string.IsNullOrEmpty(RegEmail.text) || !RegEmail.GetComponent<ValidateInput>().isValidInput)
        {
            RegEmail.GetComponent<ValidateInput>().Validate(RegEmail.text);
            RegEmail.Select();
        }
        else if (string.IsNullOrEmpty(RegPhomenumber.text) || !RegPhomenumber.GetComponent<ValidateInput>().isValidInput)
        {
            RegPhomenumber.GetComponent<ValidateInput>().Validate(RegPhomenumber.text);
            RegPhomenumber.Select();
        } else 
        {
            print("Signup Validate true");
            emailid = RegEmail.text;
            fullname = RegFullname.text;
            phonecode = RegPhoneCode.options[RegPhoneCode.value].text;
            phonenumber = RegPhomenumber.text;

            oTPInitiator = OTPInitiator.Registration;
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
                print("User id after register data "+user_id);
                navigationPanelsList.Add(SignUpPanel);
                ClearInpuFields();
                oTPInitiator = OTPInitiator.Registration;
                ActivatePanel(VerificationPanel.name);
            }
            else
            {
                if (response.data.otp_verified == 0)
                {
                    user_id = response.data.user_id;
                    navigationPanelsList.Add(SignUpPanel);
                    ClearInpuFields();

                    loginPopup.SetPopup(response.message, () => {
                        WebRequestObject.ProcessResendOTP(user_id, (ResponseData<UserData> data) => print("OTP RESEND"));    
                        ActivatePanel(VerificationPanel.name); });
                }
                else
                {
                    RegisScreenMessage.text = response.message;
                }
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
            
            LoaderPanel.SetActive(true);
            WebRequests.Instance.ProcessCheckUsernameAvailable(ChooseUsernameInput.text, callbackUsernameAvailable);
 
        }
    } 
    void callbackUsernameAvailable(ResponseBase response)
    {
            LoaderPanel.SetActive(false);
        if(response.status)
        {
            ClearInpuFields();
            navigationPanelsList.Add(ChooseUserNamePanel);
            ActivatePanel(PasswordPanel.name);
            PasswordPanel.GetComponent<PasswordPanel>().Populate("CHOOSE YOUR PASSWORD");
        }
        else
        {
            loginPopup.gameObject.SetActive(true);
            loginPopup.SetPopup(response.message, null);
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
                Globals.SaveUserData(obj.data);
                Globals.USERNAME = obj.data.username;
                navigationPanelsList.Clear();
                ClearInpuFields();
                ActivatePanel(LoginPanel.name);
                loginPopup.SetPopup(obj.message, () => Globals.LoadLevel(Globals.HOME_SCENE));
                

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
        oTPInitiator = OTPInitiator.Login;
        navigationPanelsList.Clear();
        ActivatePanel(LoginPanel.name);
    }

    private void AfterFBDatarecieved(FBUserData data)
    {
    }

    public void FbLoginClicked()
    {
         loginPopup.SetPopup("Under construction.", () => { 
            }); 
        //fbManager.FBLogin();
    }
    public void GoogleLoginClicked()
    {
        loginPopup.SetPopup("Under construction.", () => { 
            });
    }

    public void PasswordButtonClicked(string _passwordId)
    {
        passwordId = _passwordId;
        print("You choosen password "+ passwordId);
    }
    #endregion ButtonClickEvents

    #region FBLogin
    void OnFBLogin(string token)
    {
        LoaderPanel.SetActive(true);
        WebRequestObject.ProcessFBLogin(token, FBLoginCallback  );
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
 

        if(string.Equals(LoginPanel.name, panelName)){
            LoginPanel.SetActive(true);            
            oTPInitiator = OTPInitiator.Login;
        }
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
        else
        {
            print("Ask whether want to exit");
            confirmationPopup.gameObject.SetActive(true);
            confirmationPopup.SetUpPanel("EXIT", "Are you sure you want to exit.", () => Application.Quit(), () => confirmationPopup.gameObject.SetActive(false));
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
        // Clear All input fields
        usernameInput.text = string.Empty;
        EmailFPInput.text = string.Empty;
        RegFullname.text = string.Empty;
        RegEmail.text = string.Empty;
        RegPhomenumber.text = string.Empty;
        ChooseUsernameInput.text = string.Empty;

        //Clear Dropdown for phone code
        RegPhoneCode.RefreshShownValue();

        // Clear all ScrrenMessages;
        print("Clear");
        LoginScreenMessage.text = "";
        PasswordScreenMessage.text = "";
        RegisScreenMessage.text = "";
        VerificationScreenMessage.text = "";
        ForgotPasswordScreenMessage.text = "";
        GradeScreenMessage.text = "";

        if (loginPopup.gameObject.activeInHierarchy)
        {
            loginPopup.gameObject.SetActive(false);
        }
    }


    //----------DownloadPassword icons
    void DownloadPassword()
    {
        
    }


    //-----------------------------------
}
