using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabSelect : MonoBehaviour
{

    private EventSystem eventSystem;
    public InputField[] ALLInputFields;
    private int InputFCount =0;

    // Use this for initialization
    void Start()
    {
        this.eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(ALLInputFields[InputFCount].gameObject);
        for (int i = 0; i < ALLInputFields.Length; i++)
        {
            ALLInputFields[i].onValueChanged.AddListener(Editing);
        }
    }

    private void Editing(string currentText)
    {
        if (!string.IsNullOrEmpty(currentText))
        {
            GameObject gobj = eventSystem.currentSelectedGameObject.gameObject;
            gobj.GetComponent<InputField>().text = currentText;
            Debug.Log("Inside ");
            MethodA();
        }
        
    }

    public void MethodA()
    {
        InputFCount++;
        Debug.Log("<color=red>MYLOg</color>");
        Selectable next = null;
        Selectable current = null;

        // Figure out if we have a valid current selected gameobject
        if (eventSystem.currentSelectedGameObject != null)
        {
            Debug.Log("1");
            // Unity doesn't seem to "deselect" an object that is made inactive
            if (eventSystem.currentSelectedGameObject.activeInHierarchy)
            {
                Debug.Log("2");
                current = eventSystem.currentSelectedGameObject.GetComponent<Selectable>();
            }
        }

        if (current != null)
        {
            Debug.Log("3");
            // When SHIFT is held along with tab, go backwards instead of forwards
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Debug.Log("4");
                next = current.FindSelectableOnLeft();
                if (next == null)
                {
                    Debug.Log("5");
                    next = current.FindSelectableOnUp();
                }
            }
            else
            {
                Debug.Log("6");
                next = current.FindSelectableOnRight();
                if (next == null)
                {
                    Debug.Log("7");
                    next = current.FindSelectableOnDown();
                }
            }
        }
        else
        {
            Debug.Log("8");
            // If there is no current selected gameobject, select the first one
            if (Selectable.allSelectableCount > 0)
            {
                Debug.Log("9");
                next = Selectable.allSelectablesArray[0];
            }
        }

        if (next != null)
        {
            Debug.Log("10");
            ALLInputFields[InputFCount].Select();
            Debug.Log("" + next.gameObject.name);
        }
    }

}
