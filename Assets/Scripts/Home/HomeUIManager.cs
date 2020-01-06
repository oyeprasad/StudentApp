using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUIManager : MonoBehaviour
{
    [SerializeField] private Text UserWelcomeText;
    // Start is called before the first frame update
    void Start()
    {
        print("Globals.USERNAME "+ Globals.USERNAME);
        UserWelcomeText.text = string.Format("{0}, {1}", "Hello", Globals.USERNAME.ToUpper());
    }
     
}
