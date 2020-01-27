using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class ChangePassswordButtons : MonoBehaviour
{
 
    private Button ClickButton; 

    [SerializeField] private string id;
    [SerializeField] private Transform highlight;

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
     //   HomeMainUIController.EventPassowrdClicked.Invoke(id);
    }

    private void OnChangePasswordClicked()
    {
        if (id == "1112121")
        {
            highlight.position = transform.position;
        }
    }
}
