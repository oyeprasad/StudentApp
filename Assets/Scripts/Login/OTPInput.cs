using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OTPInput : MonoBehaviour
{
    [SerializeField] private OTP otp;
    [SerializeField] private int index;
    InputField input;

    private void OnDisable()
    {
        input.text = string.Empty;
    }
    private void Start()
    {
        input = GetComponent<InputField>(); 
        input.onValueChanged.AddListener(onValueChangeMethod);
    }
    void onValueChangeMethod(string value)
    {
        otp.OnValueChangeEvent.Invoke(index);
    }
}
