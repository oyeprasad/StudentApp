using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OTPInput : MonoBehaviour
{
    TMPro.TMP_InputField input;

    private void OnDisable()
    {
        input.text = string.Empty;
    }
    private void Start()
    {
        input = GetComponent<TMPro.TMP_InputField>();
        input.onValueChanged.AddListener((value) =>ValueChanged(value));
        input.onSelect.AddListener((value) => OnSelected(value));
    }
    private void Update()
    {
       //input.textComponent.rectTransform.anchoredPosition = Vector2.zero;
       //transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
    void ValueChanged(string value) {
        if (input.text.Length > 5) {
            input.OnDeselect(new BaseEventData(EventSystem.current));
            Login.OTPSubmitEvent.Invoke(input.text);
        }
    }

    void OnSelected(string value)
    {
        value = "";
        input.text = value;
        transform.GetChild(0).GetChild(1).gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
}
