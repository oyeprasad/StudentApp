using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class PassswordButtons : MonoBehaviour
{
    private Button ClickButton;

    [SerializeField] private string id;
    [SerializeField] private Transform highlight;

    private void Start()
    {
        ClickButton = GetComponent<Button>();
        ClickButton.onClick.AddListener(onClick);
    }

    private void onClick()
    {
        highlight.position = transform.position;
        Login.PasswordClickEvent.Invoke(id);
    }
}
