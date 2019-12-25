using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OTP : MonoBehaviour
{
    [SerializeField] private InputField otpInput;
    [SerializeField] private Text screenMessage; 

    private void Start()
    {
        LoginMenu.InputFieldEditStart.AddListener(StartEditToInput);
    }

    public void Submit()
    {
        if (string.IsNullOrEmpty(otpInput.text) || !otpInput.GetComponent<ValidateInput>().isValidInput)
        {
            otpInput.GetComponent<ValidateInput>().Validate(otpInput.text);
        }
    }

    void StartEditToInput()
    {
        screenMessage.text = string.Empty;
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
        {
            BackClicked();
        }
    }

    public void BackClicked()
    {
        LoginMenu.BackFromPanelEvent.Invoke("Login");
    }

}
