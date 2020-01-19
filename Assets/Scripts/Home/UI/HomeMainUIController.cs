using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;

[System.Serializable] public class LogoutResponse:ResponseBase
{
    List<string> data = new List<string>();
}
public class HomeMainUIController : MonoBehaviour
{  
    private List<GameObject> navigationPanelsList = new List<GameObject>();

    // Reference to all panels, as gameobject
    public GameObject HomePanelObject, ProfilePanel, ProfilePanelEdit,CategoryPanel, SubCategoryPanel, 
                      VideoPanel, QuizPanel, ChangePasswordPanel, HelpPanel, AboutUsPanel, TermsConditionsPanel;
    public GameObject LoaderObject;
    public GameObject SidePanel; 
    public Popup popup;
    public ConfirmationPopup confirmationPopup;

    //Public events
    public static UnityEvent EventBackClicked = new UnityEvent();
    public static UnityEvent EventVideoClicked = new UnityEvent();
    public static UnityEvent EventProfileClicked = new UnityEvent();

    public static BooleanEvent EventShowHideLoader = new BooleanEvent();
    public static IntStringEvent EventCategoryItemClicked = new IntStringEvent();
    public static IntStringEvent SubCatClicked = new IntStringEvent(); 
    public static StringActionEvent ShowPopup = new StringActionEvent();
    public static IntEvent EventQuizzesClicked = new IntEvent();
    public static StringEvent EventPassowrdClicked = new StringEvent();
    public static UnityEvent EventMyProfileEditClicked = new UnityEvent();
    public static UnityEvent EventMyProfileSaveClicked = new UnityEvent();
    private string newPassowrdToChange = "1112121"; 
    void Start()
    {
        HomeMainUIController.EventBackClicked.AddListener(OnBackClicked);
        HomeMainUIController.EventVideoClicked.AddListener(VideoClicked);
        HomeMainUIController.EventProfileClicked.AddListener(ProfileClicked);
        HomeMainUIController.EventShowHideLoader.AddListener(ShowHideLoader);
        HomeMainUIController.EventCategoryItemClicked.AddListener(CategoryItemClicked);
        HomeMainUIController.SubCatClicked.AddListener(OnSubCatClicked);
        HomeMainUIController.ShowPopup.AddListener(OnShowPopup);
        HomeMainUIController.EventQuizzesClicked.AddListener(QuizzesClicked);
        HomeMainUIController.EventPassowrdClicked.AddListener(PasswordButtonClicked);
        HomeMainUIController.EventMyProfileEditClicked.AddListener(MyProfileEditClicked);
        HomeMainUIController.EventMyProfileSaveClicked.AddListener(MyProfileSaveClicked);
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
    }

    void ProfileClicked()
    {
        navigationPanelsList.Add(HomePanelObject);
        ActivatePanel(ProfilePanel.name);
    }

    void OnLogoutClicked()
    {

    }
    void VideoClicked()
    {
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
        navigationPanelsList.Add(SubCategoryPanel);
        ActivatePanel(VideoPanel.name);
        VideoPanel.GetComponent<VideoPanelController>().PopulatePanel(subCatId, subCatName);
    }

    void QuizzesClicked(int videoId)
    {
        navigationPanelsList.Add(VideoPanel);
        ActivatePanel(QuizPanel.name);
        QuizPanel.GetComponent<QuizController>().PopulateQuizzes(videoId);
    }

    void PasswordButtonClicked(string newPassword)
    {
        newPassowrdToChange = newPassword;
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
    #endregion ClickButtonsEvents

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
        ActivatePanel(ChangePasswordPanel.name);
        SidePanel.SetActive(false);
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
        confirmationPopup.SetUpPanel("LOGOUT", "Are you sure that you want to logout", 
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
        print("Request for change password to "+newPassowrdToChange);
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
            popup.SetPopup(response.message, () => print("logout error"));
        }
    }
    #endregion Logout

    #region ChangePassword

    #endregion ChangePassword
}
