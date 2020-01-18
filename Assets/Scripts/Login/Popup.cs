using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private Text Message;
    [SerializeField] private Button OKbtn;

    private Action ActionOnClick;

    private void Start()
    {
        OKbtn.onClick.AddListener(OnOKClicked);
    }
    public void SetPopup(string msg, Action callback)
    {
        gameObject.SetActive(true);
        Message.text = msg;
        ActionOnClick = callback;
    }

    public void OnOKClicked()
    {
        if(ActionOnClick != null)
            ActionOnClick.Invoke();
         
        this.gameObject.SetActive(false);
    }
}
