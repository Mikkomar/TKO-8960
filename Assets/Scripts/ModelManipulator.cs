using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManipulator : MonoBehaviour
{
    public bool DebuggerOn;
    public float RotationSpeed;
    public float ZoomSpeed;
    public float ScaleSpeed;
    public Transform ActiveModel;
   
    public Enums.ModelPinchAction PinchAction;

    void Start()
    {
    }

    void Update()
    {
        /* If using only one finger */
        if (Input.touchCount == 1)
        {
            /* Get finger movement */
            Vector3 direction = new Vector3(Input.GetTouch(0).deltaPosition.x, Input.GetTouch(0).deltaPosition.y, 0);
            DebugLine($"Direction: {direction.ToString()}");
            /* Rotate according to movement */
            RotateModel(direction);

        }

        /* If using two fingers */
        if(Input.touchCount == 2)
        {
            /* Get input for both fingers */
            Touch TouchZero = Input.GetTouch(0);
            Touch TouchOne = Input.GetTouch(1);

            Vector2 TouchZeroPosition = TouchZero.position - TouchZero.deltaPosition;
            Vector2 TouchOnePosition = TouchOne.position - TouchOne.deltaPosition;

            /* Calculate how fingers are moving related to each other */
            float TouchMagnitude = (TouchZeroPosition - TouchOnePosition).magnitude;
            float TouchDeltaMagnitude = (TouchZero.position - TouchOne.position).magnitude;

            float MagnitudeDifference = TouchMagnitude - TouchDeltaMagnitude;

            switch (PinchAction)
            {
                case (Enums.ModelPinchAction.Zoom):
                    /* Move model on camera-model axis according to finger movements */
                    MoveModelRelatedToCamera(MagnitudeDifference);
                    break;
                case (Enums.ModelPinchAction.Scale):
                    /* Scale model according to finger movements */
                    ScaleModel(MagnitudeDifference);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Move the Model on the camera-model axis
    /// </summary>
    /// <param name="distance"></param>
    private void MoveModelRelatedToCamera(float distance)
    {
        if(ActiveModel != null)
        {
            ActiveModel.position += Camera.main.transform.forward * distance * ZoomSpeed;
        }
        else
        {
            DebugLine("Active model not found or not set!");
        }
    }
    /// <summary>
    /// Scale the Model parent
    /// </summary>
    /// <param name="modifier"></param>
    private void ScaleModel(float modifier)
    {
        if (ActiveModel != null)
        {
            if (ActiveModel.parent != null)
            {
                /* Checking that the Model scale won't go negative */
                if (ActiveModel.parent.transform.localScale.x - modifier * ScaleSpeed > 0 &&
                    ActiveModel.parent.transform.localScale.y - modifier * ScaleSpeed > 0 &&
                    ActiveModel.parent.transform.localScale.z - modifier * ScaleSpeed > 0)
                {
                    ActiveModel.parent.localScale -= new Vector3(modifier * ScaleSpeed, modifier * ScaleSpeed, modifier * ScaleSpeed);
                }
            }
            else
            {
                DebugLine("Active model missing scalable parent!");
            }
        }
        else
        {
            DebugLine("Active model not found or not set!");
        }
    }

    /// <summary>
    /// Rotates the model
    /// </summary>
    /// <param name="rotationDirection"></param>
    private void RotateModel(Vector3 rotationDirection)
    {
        if (ActiveModel != null)
        {
            /* If the model is upside down, invert rotation */
            if (Vector3.Dot(ActiveModel.transform.up, Vector3.up) >= 0)
            {
                ActiveModel.Rotate(transform.up, -Vector3.Dot(rotationDirection * RotationSpeed, Camera.main.transform.right), Space.World);
            }
            else
            {
                ActiveModel.Rotate(transform.up, -Vector3.Dot(rotationDirection * RotationSpeed, Camera.main.transform.right), Space.World);
            }

            ActiveModel.Rotate(transform.right, Vector3.Dot(rotationDirection*RotationSpeed, Camera.main.transform.up), Space.World);
        }
        else
        {
            DebugLine("Active model not found or not set!");
        }
    }

    /// <summary>
    /// Used for debugging purposes to write lines with the Unity debug function
    /// </summary>
    /// <param name="line"></param>
    private void DebugLine(string line)
    {
        if (this.DebuggerOn)
        {
            Debug.Log(line);
        }
    }
}
