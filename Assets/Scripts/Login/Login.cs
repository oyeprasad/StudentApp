using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum OTPInitiator
{
    ForgotPassword,
    Registration
}

public class Login : MonoBehaviour
{

    // static events
    public static StringEvent PasswordClickEvent = new StringEvent();
    public static StringEvent OTPSubmitEvent = new StringEvent(); //
    public static StringEvent GradeClickEvent = new StringEvent();

    [SerializeField] private InputField usernameInput, EmailFPInput, RegFullname, RegEmail, RegPhomenumber, ChooseUsernameInput;
    [SerializeField] private Dropdown RegPhoneCode;

    [SerializeField] private GameObject LoginPanel, PasswordPanel, ForgotPassowrdPanel, WhoAreYouPanel, VerificationPanel, SignUpPanel, ChooseUserNamePanel, GradePanel,LoaderPanel;
    [SerializeField] private LoginPopup loginPopup;
    [SerializeField] private FBManager fbManager;

    private string username, passwordId = "1", emailid, fullname, phonecode, phonenumber, grade; 

    private List<GameObject> navigationPanelsList = new List<GameObject>();
    private OTPInitiator oTPInitiator = OTPInitiator.ForgotPassword;
    #region MonoBehaviourMethods
    void Start()
    {
        Login.PasswordClickEvent.AddListener(PasswordButtonClicked);
        Login.OTPSubmitEvent.AddListener(OTPSubmit);
        Login.GradeClickEvent.AddListener(GradeSubmit);
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
        navigationPanelsList.Add(LoginPanel); 
        ClearInpuFields();
        ActivatePanel(WhoAreYouPanel.name);
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

        if (oTPInitiator == OTPInitiator.ForgotPassword)
        {
            Globals.USERNAME = username;
            ClearInpuFields();
            navigationPanelsList.Clear();
             
            Globals.LoadLevel(Globals.HOME_SCENE);
        }
        else if (oTPInitiator == OTPInitiator.Registration)
        {
            navigationPanelsList.Clear();
            ClearInpuFields();
            ActivatePanel(GradePanel.name);
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
            navigationPanelsList.Add(ForgotPassowrdPanel); 
            ClearInpuFields();
            oTPInitiator = OTPInitiator.ForgotPassword;
            ActivatePanel(VerificationPanel.name);
        }
    }

    public void OTPSubmit(string _username)
    {
        username = _username; 
        ClearInpuFields();
        navigationPanelsList.Add(VerificationPanel);

        if (oTPInitiator == OTPInitiator.ForgotPassword)
            ActivatePanel(PasswordPanel.name);
        else if (oTPInitiator == OTPInitiator.Registration)
            ActivatePanel(ChooseUserNamePanel.name);
         
    }
    public void SignUpSubmit()
    {//RegFullname, RegEmail, RegPhomenumber
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
            navigationPanelsList.Add(SignUpPanel);
            ClearInpuFields();
            oTPInitiator = OTPInitiator.Registration;
            ActivatePanel(VerificationPanel.name);

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

    private void GradeSubmit(string gradeNo)
    {
        if (GradeIsAvailable(gradeNo))
        {
            grade = gradeNo;
            navigationPanelsList.Clear();
            ClearInpuFields();
            ActivatePanel(LoginPanel.name);
            Globals.LoadLevel(Globals.HOME_SCENE);
        }
        else
        {
            loginPopup.gameObject.SetActive(true);
            loginPopup.SetPopup("COMING SOON! TRY ANOTHER GRADE!", null);
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
        fbManager.FBLogin(AfterFBDatarecieved);
    }
    public void GoogleLoginClicked()
    {

    }

    public void PasswordButtonClicked(string _passwordId)
    {
        passwordId = _passwordId;
    }
    #endregion ButtonClickEvents

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
    bool GradeIsAvailable(string _gradeNo)
    {
        return false;
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

        if (loginPopup.gameObject.activeInHierarchy)
        {
            loginPopup.gameObject.SetActive(false);
        }
    }
}
