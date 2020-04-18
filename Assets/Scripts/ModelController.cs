using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelController : MonoBehaviour
{
    public Dropdown Dropdown;
    public Button PlaceButton;
    public Transform ScaleParent;
    private ModelManipulator GestureTracker;
    // Start is called before the first frame update
    void Start()
    {
        HideChildren();
        var parentClass = GameObject.Find("GestureTracker");
        if (parentClass != null)
        {
            this.GestureTracker = parentClass.GetComponent<ModelManipulator>();
        }
    }

    void Update()
    {
        CheckPlaceButtonVisibility();
    }

    /// <summary>
    /// Hides clonable models on Start
    /// </summary>
    private void HideChildren()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Changes PlaceButton visibility depending whether or not there is an active Model
    /// </summary>
    private void CheckPlaceButtonVisibility()
    {
        if (PlaceButton != null && GestureTracker != null)
        {
            PlaceButton.gameObject.SetActive(GestureTracker.ActiveModel != null);
        }
    }

    /// <summary>
    /// Sets the active model for GestureTracker
    /// </summary>
    public void SetActiveModel()
    {
        if (this.Dropdown != null)
        {
            /* Gets the selected dropdown list index, not counting the title option in index 0 */
            int DropDownValueIndex = Dropdown.value - 1;
            /* If selected index not "Select Model" */
            if (DropDownValueIndex != -1)
            {
                /* Selects the right clonable child Model based on the selected DDL index */
                for(int i = 0; i < transform.childCount; i++)
                {
                    GameObject Child = transform.GetChild(i).gameObject;
                    /* If child has the same value as the selected DDL index, it's set as the active Model and cloned */
                    if(i == DropDownValueIndex)
                    {
                        if(GestureTracker != null)
                        {
                            /* If there is no active Model, the selected object is cloned, set as the active Model and made visible*/
                            if (GestureTracker.ActiveModel == null && ScaleParent != null)
                            {
                                GestureTracker.ActiveModel = Instantiate(Child).transform;
                                GestureTracker.ActiveModel.gameObject.SetActive(true);
                                /* The Model needs to be assigned a parent with an offset, so the Model will be scaled "upwards" with the parent */
                                var TempParent = Instantiate(ScaleParent.gameObject);
                                TempParent.transform.position = new Vector3(GestureTracker.ActiveModel.transform.position.x, GestureTracker.ActiveModel.transform.position.y - 0.5f, GestureTracker.ActiveModel.transform.position.z);
                                TempParent.transform.rotation = GestureTracker.ActiveModel.transform.rotation;
                                TempParent.transform.localScale = GestureTracker.ActiveModel.transform.localScale;
                                TempParent.SetActive(true);
                                GestureTracker.ActiveModel.SetParent(TempParent.transform);
                            }
                            /* If there exists an active Model, it is replaced and deleted */
                            else
                            {
                                var TempModel = Instantiate(Child).transform;
                                var TempParent = Instantiate(ScaleParent.gameObject);
                                TempParent.transform.position = new Vector3(GestureTracker.ActiveModel.transform.position.x, GestureTracker.ActiveModel.transform.position.y - 0.5f, GestureTracker.ActiveModel.transform.position.z);
                                TempParent.transform.rotation = GestureTracker.ActiveModel.transform.rotation;
                                TempParent.transform.localScale = GestureTracker.ActiveModel.transform.localScale;
                                TempModel.transform.position = GestureTracker.ActiveModel.transform.position;
                                TempModel.transform.rotation = GestureTracker.ActiveModel.transform.rotation;
                                TempModel.transform.localScale = GestureTracker.ActiveModel.transform.localScale;
                                Destroy(GestureTracker.ActiveModel.gameObject);
                                GestureTracker.ActiveModel = TempModel;
                                GestureTracker.ActiveModel.gameObject.SetActive(true);
                                TempParent.SetActive(true);
                                GestureTracker.ActiveModel.SetParent(TempParent.transform);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Places the active Model by setting it non-interactable
    /// </summary>
    public void PlaceObject()
    {
        this.GestureTracker.ActiveModel = null;
    }
}
