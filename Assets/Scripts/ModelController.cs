using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelController : MonoBehaviour
{
    public Dropdown Dropdown;
    public Dropdown MaterialDropdown;
    public Button PlaceButton;
    public Transform ScaleParent;
    private ModelManipulator GestureTracker;

    public Material MaterialClothBlue;
    public Material MaterialClothGreen;
    public Material MaterialWood;
    public Material MaterialDarkWood;

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
        CheckMaterialDropdownVisibility();
        MakeModelMoveWithCamera();
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
    /// Makes the active Model move with the camera
    /// </summary>
    private void MakeModelMoveWithCamera()
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        if (GestureTracker.ActiveModel != null && GestureTracker.ActiveModel.parent != null)
        {
            TrackableHit HitInfo;
            /* Checks where the camera-plane axis intersects with the plane and creates a hitting point */
            if (Frame.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out HitInfo))
            {
                GestureTracker.ActiveModel.parent.position = HitInfo.Pose.position;
                //GestureTracker.ActiveModel.parent.rotation = HitInfo.Pose.rotation;
            }
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
    /// Changes MaterialDropdown visibility depending whether or not there is an active Model
    /// </summary>
    private void CheckMaterialDropdownVisibility()
    {
        if (MaterialDropdown != null && GestureTracker != null)
        {
            MaterialDropdown.gameObject.SetActive(GestureTracker.ActiveModel != null);
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
                                Destroy(GestureTracker.ActiveModel.parent.gameObject);
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
    /// Sets the material for the active Model
    /// </summary>
    public void SetMaterial()
    {
        if (this.MaterialDropdown != null && this.GestureTracker.ActiveModel != null)
        {
            int DropDownValueIndex = MaterialDropdown.value - 1;
            /* If selected index not "Select Material" */
            if (DropDownValueIndex != -1)
            {
                switch (DropDownValueIndex)
                {
                    case (0):
                        if (this.MaterialClothBlue != null)
                        {
                            SetMaterialForModel(this.MaterialClothBlue);
                        }
                        break;
                    case (1):
                        if (this.MaterialClothGreen != null)
                        {
                            SetMaterialForModel(this.MaterialClothGreen);
                        }
                        break;
                    case (2):
                        if (this.MaterialWood != null)
                        {
                            SetMaterialForModel(this.MaterialWood);
                        }
                        break;
                    case (3):
                        if (this.MaterialDarkWood != null)
                        {
                            SetMaterialForModel(this.MaterialDarkWood);
                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Changes the material for the active Model and its children's renderer
    /// </summary>
    /// <param name="material"></param>
    private void SetMaterialForModel(Material material)
    {
        Renderer renderer = this.GestureTracker.ActiveModel.GetComponent<Renderer>();
        if(renderer != null)
        {
            renderer.material = material;
        }

        foreach (var childrendrer in this.GestureTracker.ActiveModel.GetComponentsInChildren<Renderer>())
        {
            childrendrer.material = material;
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
