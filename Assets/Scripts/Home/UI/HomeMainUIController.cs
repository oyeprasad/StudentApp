using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HomeMainUIController : MonoBehaviour
{  
    private List<GameObject> navigationPanelsList = new List<GameObject>();

    // Reference to all panels, as gameobject
    public GameObject HomePanelObject, ProfilePanel, CategoryPanel, SubCategoryPanel, VideoPanel, QuizPanel;
    public GameObject LoaderObject;
    public Popup popup;

    //Public events
    public static UnityEvent EventBackClicked = new UnityEvent();
    public static UnityEvent EventVideoClicked = new UnityEvent();
    public static UnityEvent EventProfileClicked = new UnityEvent();

    public static BooleanEvent EventShowHideLoader = new BooleanEvent();
    public static IntStringEvent EventCategoryItemClicked = new IntStringEvent();
    public static IntStringEvent SubCatClicked = new IntStringEvent(); 
    public static StringActionEvent ShowPopup = new StringActionEvent();
    public static IntEvent EventQuizzesClicked = new IntEvent();


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

    #endregion ClickButtonsEvents

    public void ActivatePanel(string panelName)
    {
        if (string.IsNullOrEmpty(panelName))
            return;

        HomePanelObject.SetActive(string.Equals(HomePanelObject.name, panelName)); 
        ProfilePanel.SetActive(string.Equals(ProfilePanel.name, panelName));
        CategoryPanel.SetActive(string.Equals(CategoryPanel.name, panelName));
        SubCategoryPanel.SetActive(string.Equals(SubCategoryPanel.name, panelName));
        VideoPanel.SetActive(string.Equals(VideoPanel.name, panelName));
        QuizPanel.SetActive(string.Equals(QuizPanel.name, panelName));
    }

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
    #endregion OtherEvents
}
