using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class GradeButtons : MonoBehaviour
{ 
    private Button ClickButton;

    [SerializeField] private string grade;

    private void Start()
    {
        ClickButton = GetComponent<Button>();
        ClickButton.onClick.AddListener(onClick);
    }

    private void onClick()
    {
        Login.GradeClickEvent.Invoke(grade);
    }
}
