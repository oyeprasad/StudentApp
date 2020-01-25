using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePasswordPanel : MonoBehaviour
{  
    [SerializeField] private string panelName = ""; 
    void OnEnable()
    {
        HomeMainUIController.EventPasswordPanelHide.Invoke(panelName);
    }
}
