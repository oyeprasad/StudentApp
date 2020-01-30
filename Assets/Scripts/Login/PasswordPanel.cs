using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PasswordPanel : MonoBehaviour
{
    
    [SerializeField] private Text title;
    public void Populate(string titleText)
    {
        title.text = titleText;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
