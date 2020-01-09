using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUIManager : MonoBehaviour
{
    [SerializeField] private Text UserWelcomeText;
    // Start is called before the first frame update

    private List<GameObject> navigationPanelsList = new List<GameObject>();

    // Reference to all panels, as gameobject
    public GameObject HomePanel, ProfilePanel;



    void Start()
    {
        if (Globals.LoginType == 0)
        {
            UserWelcomeText.text = string.Format("{0}, {1}", "Hello", Globals.UserLoginDetails.username.ToUpper());
        }
        else if (Globals.LoginType == 1)
        {
            UserWelcomeText.text = string.Format("{0}, {1}", "Hello", Globals.fBLoginResponseData.name.ToUpper());
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
    }

    public void ProfileClciked()
    {
      //  navigationPanelsList.Add(HomePanel);
        //ActivatePanel(ProfilePanel.name);
    }

    public void OnLogoutClicked()
    {

    }

    #endregion ClickButtonsEvents

    void ActivatePanel(string panelName)
    {
        if (string.IsNullOrEmpty(panelName))
            return;

        HomePanel.SetActive(string.Equals(HomePanel.name, panelName));
        ProfilePanel.SetActive(string.Equals(ProfilePanel.name, panelName));
    }

}
