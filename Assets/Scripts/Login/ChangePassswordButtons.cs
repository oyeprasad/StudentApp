using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class ChangePassswordButtons : MonoBehaviour
{
  
    private Button ClickButton; 
    public string id;
    [SerializeField] private Transform highlight;
    [SerializeField] private HomePasswordPanel _parent;

    void OnEnable()
    {  
    }
    private void Start()
    {
        HomeMainUIController.EventChangePasswordClicked.AddListener(OnChangePasswordClicked);
        ClickButton = GetComponent<Button>();
        ClickButton.onClick.AddListener(onClick);
    }

    private void onClick()
    {
        highlight.position = transform.position; 
        _parent.OnClick(this);
    }
 
    private void OnChangePasswordClicked()
    {
        if (id == "1112121")
        {
            highlight.position = transform.position;
            _parent.UpdateCurrentPassword(this);
        } 
    }
}
