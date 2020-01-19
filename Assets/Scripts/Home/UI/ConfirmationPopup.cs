using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConfirmationPopup : MonoBehaviour
{
     [SerializeField] private Text header;
     [SerializeField] private Text body;
     [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

     private System.Action ActionOnYes;
     private System.Action ActionOnNo;
     
     void Start()
     {
      yesButton.onClick.AddListener(() => ActionOnYes.Invoke());
      noButton.onClick.AddListener(() => ActionOnNo.Invoke()); 
     }
      private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
     public void SetUpPanel(string _header, string _body, System.Action OnYes, System.Action OnNo)
     {
         header.text = _header;
         body.text = _body;
         ActionOnYes = OnYes;
         ActionOnNo = OnNo;
     }
}
