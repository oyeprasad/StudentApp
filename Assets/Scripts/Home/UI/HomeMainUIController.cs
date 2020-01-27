using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;


public enum QueryType
{
    Video,
    Quizzes,
    WorkSheet
}

public enum PasswordPanelState
{
    PASSWORD,
    OLDPASSWORD,
    NEWPASSOWRD,
    CONFIRMNEWPASSWORD 
}

[System.Serializable] public class LogoutResponse:ResponseBase
{
    List<string> data = new List<string>();
}
[System.Serializable] public class ChangePasswordResponse : ResponseBase
{
    List<string> data = new List<string>();    
}
public class HomeMainUIController : MonoBehaviour
{  
    private List<GameObject> navigationPanelsList = new List<GameObject>();
    private Sprite UserImage;

    // Reference to all panels, as gameobject
    public GameObject HomePanelObject, ProfilePanel, ProfilePanelEdit,CategoryPanel, SubCategoryPanel, 
                      VideoPanel, QuizPanel, ChangePasswordPanel, NewPasswordPanel,ConfirmNewPassPanel,HelpPanel, AboutUsPanel, TermsConditionsPanel;
    public GameObject LoaderObject;
    public GameObject SidePanel; 
    public Popup popup;
    public ConfirmationPopup confirmationPopup;

    //Public events
    public static UnityEvent EventBackClicked = new UnityEvent();
    public static UnityEvent EventVideoClicked = new UnityEvent();
    public static UnityEvent EventQuizzesClickedFromHome = new UnityEvent();
    public static UnityEvent EventWorkSheetClickedFromHome = new UnityEvent();
    public static UnityEvent EventProfileClicked = new UnityEvent();

    public static BooleanEvent EventShowHideLoader = new BooleanEvent();
    public static IntStringEvent EventCategoryItemClicked = new IntStringEvent();
    public static IntStringEvent SubCatClicked = new IntStringEvent(); 
    public static StringActionEvent ShowPopup = new StringActionEvent();
    public static IntEvent EventQuizzesClicked = new IntEvent();
    public static IntStringEvent EventPassowrdClicked = new IntStringEvent();
    public static UnityEvent EventMyProfileEditClicked = new UnityEvent();
    public static UnityEvent EventMyProfileSaveClicked = new UnityEvent();
    public static StringEvent EventSubmitHelp = new StringEvent();
    public static StringEvent EventPasswordPanelHide = new StringEvent();

    public static SpriteFloatEvent EventProfilePicChoose = new SpriteFloatEvent();
    public static UnityEvent EventChangePasswordClicked = new UnityEvent();

     
    // End Events\\\


    public static QueryType queryType; 
    private string newPassowrdToChange = "1112121"; 
    private PasswordPanelState passwordPanelState = PasswordPanelState.PASSWORD;


     
    void Start()
    {
        HomeMainUIController.EventBackClicked.AddListener(OnBackClicked);
        HomeMainUIController.EventVideoClicked.AddListener(VideoClicked);
        HomeMainUIController.EventQuizzesClickedFromHome.AddListener(QuizzesClickedFromHome); //
        HomeMainUIController.EventWorkSheetClickedFromHome.AddListener(WorkSheetClickedFromHome);
        HomeMainUIController.EventProfileClicked.AddListener(ProfileClicked);
        HomeMainUIController.EventShowHideLoader.AddListener(ShowHideLoader);
        HomeMainUIController.EventCategoryItemClicked.AddListener(CategoryItemClicked);
        HomeMainUIController.SubCatClicked.AddListener(OnSubCatClicked);
        HomeMainUIController.ShowPopup.AddListener(OnShowPopup);
        HomeMainUIController.EventQuizzesClicked.AddListener(QuizzesClicked);
        HomeMainUIController.EventPassowrdClicked.AddListener(PasswordButtonClicked);
        HomeMainUIController.EventMyProfileEditClicked.AddListener(MyProfileEditClicked);
        HomeMainUIController.EventMyProfileSaveClicked.AddListener(MyProfileSaveClicked);
        HomeMainUIController.EventSubmitHelp.AddListener(OnHelpSubmit);
        HomeMainUIController.EventPasswordPanelHide.AddListener(OnPasswordPanelHide);
        HomeMainUIController.EventProfilePicChoose.AddListener(OnProfilePicSelected);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnBackClicked();
        }
    }

    // Click Events
    #region ClickButtonsEvents
    public void OnBackClicked()
    {
        if (navigationPanelsList.Count > 0)
        { 
            string targt = navigationPanelsList[navigationPanelsList.Count - 1].name;
            navigationPanelsList.RemoveAt(navigationPanelsList.Count - 1);
            ActivatePanel(targt);
        } 
        else
        {
            confirmationPopup.gameObject.SetActive(true);
            confirmationPopup.SetUpPanel("EXIT", "Are you sure you want to exit.", () => Application.Quit(), () => confirmationPopup.gameObject.SetActive(false));
       
        }
        // Hide loader if working
        EventShowHideLoader.Invoke(false);
    }

    void ProfileClicked()
    {
        MenuMyAccountClicked();
    }

    void OnLogoutClicked()
    {

    }
    void VideoClicked()
    {
        queryType = QueryType.Video;
        navigationPanelsList.Add(HomePanelObject);
        ActivatePanel(CategoryPanel.name); 
        CategoryPanel.GetComponent<CategoryManager>().PopulateCategoryPanel(Globals.UserLoginDetails.grade);   
    }
    void QuizzesClickedFromHome()
    {
        queryType = QueryType.Quizzes;
        navigationPanelsList.Add(HomePanelObject);
        ActivatePanel(CategoryPanel.name); 
        CategoryPanel.GetComponent<CategoryManager>().PopulateCategoryPanel(Globals.UserLoginDetails.grade);   
    }

    void WorkSheetClickedFromHome()
    {
        queryType = QueryType.WorkSheet;
        navigationPanelsList.Add(HomePanelObject);
        ActivatePanel(CategoryPanel.name); 
        CategoryPanel.GetComponent<CategoryManager>().PopulateCategoryPanel(Globals.UserLoginDetails.grade);  
    }

    void CategoryItemClicked(int catno, string title)
    {
        navigationPanelsList.Add(CategoryPanel);
        ActivatePanel(SubCategoryPanel.name);
        SubCategoryPanel.GetComponent<SubCategoryManager>().Populate(catno, title);
    }
    void OnSubCatClicked(int subCatId, string subCatName)
    {
        print(subCatId+ " Name " +subCatName);
        navigationPanelsList.Add(SubCategoryPanel);
        if(queryType == QueryType.Video)
        {
            navigationPanelsList.Add(VideoPanel);
            ActivatePanel(VideoPanel.name);
            VideoPanel.GetComponent<VideoPanelController>().PopulatePanel(subCatId, subCatName);
        } else if(queryType == QueryType.Quizzes)
        {
            navigationPanelsList.Add(SubCategoryPanel);
            ActivatePanel(QuizPanel.name);
            QuizPanel.GetComponent<QuizController>().PopulateQuizzesOnSubCat(subCatId);
        } 
        else if(queryType == QueryType.WorkSheet)
        {
            StartCoroutine(DownloadWorkSheet(subCatId.ToString()));                  
        }
    }

    void QuizzesClicked(int videoId)
    {
        navigationPanelsList.Add(VideoPanel);
        ActivatePanel(QuizPanel.name);
        QuizPanel.GetComponent<QuizController>().PopulateQuizzesOnVideo(videoId);
    }

    void PasswordButtonClicked(int id, string _password)
    {
        if(id == 0) // Set old password
        { 
            oldPasswordChoosen = _password;
        }
        else if(id == 1) // set new password
        {
            newPasswordChoosen = _password; 
        }
        else if(id == 2) // set confirm password
        {
            confirmNewPasswordChoosen = _password; 
        }
    }
    void MyProfileEditClicked()
    {
        navigationPanelsList.Add(ProfilePanel);
        ActivatePanel(ProfilePanelEdit.name);
        ProfilePanelEdit.GetComponent<ProfileController>().PopulatePanel();
    }
    void MyProfileSaveClicked()
    {
        navigationPanelsList.Clear();
        navigationPanelsList.Add(HomePanelObject);
        ActivatePanel(ProfilePanel.name);
    }
    void OnHelpSubmit(string helpText)
    {
        popup.gameObject.SetActive(true);
        StartCoroutine( HelpSubmitApi(helpText));    
    }
    #endregion ClickButtonsEvents


//====
void OnPasswordPanelHide(string panelName)
{
    if(panelName == "confirmpassword")
    {
        passwordPanelState = PasswordPanelState.CONFIRMNEWPASSWORD;
    }
    else if(panelName == "newpassword")
    {
        passwordPanelState = PasswordPanelState.NEWPASSOWRD;

    } else if(panelName == "oldpassword")
    {
        passwordPanelState = PasswordPanelState.OLDPASSWORD;
    }
}
//=======
    public void ActivatePanel(string panelName)
    {
        if (string.IsNullOrEmpty(panelName))
            return;

        HomePanelObject.SetActive(string.Equals(HomePanelObject.name, panelName)); 
        ProfilePanel.SetActive(string.Equals(ProfilePanel.name, panelName));
        ProfilePanelEdit.SetActive(string.Equals(ProfilePanelEdit.name, panelName));
        CategoryPanel.SetActive(string.Equals(CategoryPanel.name, panelName));
        SubCategoryPanel.SetActive(string.Equals(SubCategoryPanel.name, panelName));
        VideoPanel.SetActive(string.Equals(VideoPanel.name, panelName));
        QuizPanel.SetActive(string.Equals(QuizPanel.name, panelName));
        ChangePasswordPanel.SetActive(string.Equals(ChangePasswordPanel.name, panelName));
        NewPasswordPanel.SetActive(string.Equals(NewPasswordPanel.name, panelName));
        ConfirmNewPassPanel.SetActive(string.Equals(ConfirmNewPassPanel.name, panelName));
        HelpPanel.SetActive(string.Equals(HelpPanel.name, panelName));
        AboutUsPanel.SetActive(string.Equals(AboutUsPanel.name, panelName));
        TermsConditionsPanel.SetActive(string.Equals(TermsConditionsPanel.name, panelName));
    }
#region  MenuButtons
    public void MenuButtonClicked()
    {
        SidePanel.SetActive(true);
    }
    public void MenuCrossClicked()
    {
        SidePanel.SetActive(false);
    }
    public void MenuHomeClicked()
    {
        navigationPanelsList.Clear();
        ActivatePanel(HomePanelObject.name);
        SidePanel.SetActive(false);
    }
    public void MenuMyAccountClicked()
    {
        navigationPanelsList.Add(HomePanelObject);
        ActivatePanel(ProfilePanel.name);
        ProfilePanel.GetComponent<ProfileController>().PopulatePanel();
        SidePanel.SetActive(false);
    }
     public void MenuChangePasswordClicked()
    {
        navigationPanelsList.Add(HomePanelObject);
        passwordPanelState = PasswordPanelState.OLDPASSWORD;
        ActivatePanel(ChangePasswordPanel.name);
        SidePanel.SetActive(false);
        EventChangePasswordClicked.Invoke();
    }
    public void MenuHelpClicked()
    {
        navigationPanelsList.Add(HomePanelObject);
        ActivatePanel(HelpPanel.name);
        SidePanel.SetActive(false);
    }
    public void MenuAboutClicked()
    {
        navigationPanelsList.Add(HomePanelObject);
        ActivatePanel(AboutUsPanel.name);
        SidePanel.SetActive(false);
    }
    public void MenuTermsConditionClicked()
    { 
        navigationPanelsList.Add(HomePanelObject);
        ActivatePanel(TermsConditionsPanel.name);
        SidePanel.SetActive(false);

    }
    public void MenuLogoutClicked()
    {
        SidePanel.SetActive(false);
        confirmationPopup.gameObject.SetActive(true);
        confirmationPopup.SetUpPanel("LOGOUT", "Are you sure do you want to logout?", 
                                    () => {print("Logout Yes Clicked");confirmationPopup.gameObject.SetActive(false);StartCoroutine(Logout(OnLogoutComplete)); }, 
                                    () => {print("Logout No Clicked");confirmationPopup.gameObject.SetActive(false); });
    }
#endregion MenuButtons


    #region OtherEvents
    void ShowHideLoader(bool toShow)
    {
        LoaderObject.SetActive(toShow);
    }

    void OnShowPopup(string message, System.Action onClickOk)
    {
       popup.gameObject.SetActive(true);
       popup.SetPopup(message, onClickOk); 
    }

    public void ChangePasswordSubmit()
    { 
        print("change password");
        if(passwordPanelState == PasswordPanelState.OLDPASSWORD)
        {
            navigationPanelsList.Add(ChangePasswordPanel);
            //passwordPanelState = PasswordPanelState.NEWPASSOWRD;
            ActivatePanel(NewPasswordPanel.name);
        } 
        else if(passwordPanelState == PasswordPanelState.NEWPASSOWRD)
        {
            navigationPanelsList.Add(NewPasswordPanel);
            print("confirmNewPasswordChoosen "+confirmNewPasswordChoosen);
            //passwordPanelState = PasswordPanelState.CONFIRMNEWPASSWORD;
            ActivatePanel(ConfirmNewPassPanel.name);
        } 
        else if(passwordPanelState == PasswordPanelState.CONFIRMNEWPASSWORD)
        {
            //Check whether new password and confirm new password is same
            print("confirmNewPasswordChoosen "+confirmNewPasswordChoosen);

            if(newPasswordChoosen.Equals(confirmNewPasswordChoosen))
            {
               // passwordPanelState = PasswordPanelState.PASSWORD;
                StartCoroutine(ProcessChanegPassword());
            } else{
                popup.gameObject.SetActive(true);
                popup.SetPopup("New password and Confirm password do not match", () => print("password does not match"));
            }

            // change users password
        }

    }

    void OnProfilePicSelected(Sprite image, float aspetRatio)
    {
        
    }

    #endregion OtherEvents

#region Logout
    IEnumerator Logout(System.Action<LogoutResponse> callback)
    {
        yield return null;
        HomeMainUIController.EventShowHideLoader.Invoke(true);
        string endpoint = WebRequests.Instance.LogoutEndPoint;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(Globals.BASE_URL + endpoint))
        {
            // Request and wait for the desired page.
            webRequest.SetRequestHeader("Accept", "application/json");//
            webRequest.SetRequestHeader("Authorization", "Bearer "+Globals.UserLoginDetails.access_token);

            yield return webRequest.SendWebRequest();
            while(!webRequest.isDone)
                yield return webRequest;
            if (webRequest.isNetworkError)
            {
              callback(null);  
            }
            else
            {
                print(webRequest.downloadHandler.text);
                callback(JsonUtility.FromJson<LogoutResponse>(webRequest.downloadHandler.text));
            } 
        }
    }

    void OnLogoutComplete(LogoutResponse response)
    {
        HomeMainUIController.EventShowHideLoader.Invoke(false);
        if(response.status)
        {
            PlayerPrefs.SetInt(Globals.PLAYERKEY_LOGINSTATUS, Globals.LOGGED_OUT);
            Globals.LoadLevel(Globals.LOGIN_SCENE);
        } 
        else
        {
            if(response.code == 401)
            {
                popup.SetPopup("Session Expired! Please login again.", () =>  {
                    PlayerPrefs.SetInt(Globals.PLAYERKEY_LOGINSTATUS, Globals.LOGGED_OUT);
                    Globals.LoadLevel(Globals.LOGIN_SCENE);
                });
            }
            else{
                popup.gameObject.SetActive(true);
                popup.SetPopup(response.message, () => print("logout error"));
            }
        }
    }
    #endregion Logout

    #region ChangePassword
     private string oldPasswordChoosen, newPasswordChoosen, confirmNewPasswordChoosen;

     IEnumerator ProcessChanegPassword()
     { 
        WWWForm form = new WWWForm();

        form.AddField("user_id", Globals.UserLoginDetails.user_id);
        form.AddField("old_password", oldPasswordChoosen);
        form.AddField("password", newPasswordChoosen);
        form.AddField("confirm_password", confirmNewPasswordChoosen);

        print("User Id for change password "+Globals.UserLoginDetails.user_id);
         using (UnityWebRequest www =  UnityWebRequest.Post(Globals.BASE_URL + WebRequests.Instance.ChangePasswordEndPoint, form))
        {
            HomeMainUIController.EventShowHideLoader.Invoke(true);
            www.method = "PUT";

            www.SetRequestHeader("Accept", "application/json");//
            www.SetRequestHeader("Authorization", "Bearer "+Globals.UserLoginDetails.access_token);
            print("Auth token \n"+"Bearer "+Globals.UserLoginDetails.access_token);
            yield return www.SendWebRequest();
            while (!www.isDone)
                yield return null;

            HomeMainUIController.EventShowHideLoader.Invoke(false);
            if (www.isNetworkError || www.isHttpError)
            {  
                popup.gameObject.SetActive(true);
                popup.SetPopup("Some error, Try again later.", () => popup.gameObject.SetActive(false));
            }
            else
            {
                print(www.downloadHandler.text); 
                ChangePasswordResponse response = JsonUtility.FromJson<ChangePasswordResponse>(www.downloadHandler.text);
                if(response.status)
                {
                    navigationPanelsList.Clear();
                    ActivatePanel(HomePanelObject.name);
                } 
                popup.gameObject.SetActive(true);
                popup.SetPopup(response.message, () => popup.gameObject.SetActive(false));
            }
        } 
     }

    #endregion ChangePassword
    IEnumerator HelpSubmitApi(string _message)
    { 
         WWWForm form = new WWWForm();

        form.AddField("user_id", Globals.UserLoginDetails.user_id);
        form.AddField("message", _message); 

        print("Uer Id for asking Help "+Globals.UserLoginDetails.user_id);
         using (UnityWebRequest www =  UnityWebRequest.Post(Globals.BASE_URL + WebRequests.Instance.sendMessageEndPoint, form))
        {
            HomeMainUIController.EventShowHideLoader.Invoke(true); 

            www.SetRequestHeader("Accept", "application/json");//
            www.SetRequestHeader("Authorization", "Bearer "+Globals.UserLoginDetails.access_token);

            yield return www.SendWebRequest();
            while (!www.isDone)
                yield return null;

            HomeMainUIController.EventShowHideLoader.Invoke(false);
            if (www.isNetworkError || www.isHttpError)
            {  
                popup.gameObject.SetActive(true);
                popup.SetPopup("Some error, Try again later.", () => popup.gameObject.SetActive(false));
            }
            else
            {
                print(www.downloadHandler.text); 
                ResponseBase response = JsonUtility.FromJson<ResponseBase>(www.downloadHandler.text);
                popup.SetPopup(response.message, () => {
                                                        popup.gameObject.SetActive(false);
                                                        
                                                        });
            }
        }  
    }

    IEnumerator DownloadWorkSheet(string _subCatId)
    {
        string endpoint = WebRequests.Instance.getWorkSheetEndPoint;
        EventShowHideLoader.Invoke(true);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(Globals.BASE_URL + endpoint+_subCatId))
        {
            print("Requested for worksheet : "+Globals.BASE_URL + endpoint+_subCatId);
            // Request and wait for the desired page.
            webRequest.SetRequestHeader("Accept", "application/json");//
            webRequest.SetRequestHeader("Authorization", "Bearer "+Globals.UserLoginDetails.access_token);

            yield return webRequest.SendWebRequest();
            while(!webRequest.isDone)
                yield return webRequest;
            
            EventShowHideLoader.Invoke(false);
            if (webRequest.isNetworkError)
            { 

                  popup.SetPopup("Some error in download.", () => {
                                                        popup.gameObject.SetActive(false);
                                                        
                                                        });
            }
            else
            {
                print(webRequest.downloadHandler.text);
                WorkSheetResponse response = JsonUtility.FromJson<WorkSheetResponse>(webRequest.downloadHandler.text);
                if(response.status)
                {
                    if(response.data.worksheets.Count > 0)
                    {
                        foreach (WorkSheetData item in response.data.worksheets)
                        {
                            Application.OpenURL(item.worksheet_document);
                        }
                    } else
                    {
                      popup.SetPopup(" No worksheet found.", () => {
                                                        popup.gameObject.SetActive(false);
                                                        });  
                    }
                }
                else
                {
                    popup.SetPopup(response.message, () => {
                                                        popup.gameObject.SetActive(false);
                                                        
                                                        });

                } 
            } 
        }
    } 

}
