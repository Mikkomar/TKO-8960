using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelController : MonoBehaviour
{
    public Dropdown Dropdown;
    private ModelManipulator GestureTracker;
    private Transform ActiveModel;
    // Start is called before the first frame update
    void Start()
    {
        HideChildren();
        this.GestureTracker = GameObject.Find("GestureTracker").GetComponent<ModelManipulator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HideChildren()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the active model for GestureTracker
    /// </summary>
    public void SetActiveModel()
    {
        if (this.Dropdown != null)
        {
            int DropDownValueIndex = Dropdown.value - 1;
            if (DropDownValueIndex != -1)
            {
                for(int i = 0; i < transform.childCount; i++)
                {
                    GameObject Child = transform.GetChild(i).gameObject;
                    if(i == DropDownValueIndex)
                    {
                        ActiveModel = Instantiate(Child).transform;
                        ActiveModel.gameObject.SetActive(true);
                        if(GestureTracker != null)
                        {
                            GestureTracker.ActiveModel = this.ActiveModel;
                        }
                    }
                }
            }
        }
    }
}
