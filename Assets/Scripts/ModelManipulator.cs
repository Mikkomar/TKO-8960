using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManipulator : MonoBehaviour
{
    public bool DebuggerOn;
    public float RotationSpeed;
    public Transform ActiveModel;
    private Vector3 TouchContinuesAt;

    // Start is called before the first frame update
    void Start()
    {
        TouchContinuesAt = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        /* If using only one finger */
        if (Input.touchCount == 1)
        {
            /* If a countinuous finger press is detected */
            if (Input.GetMouseButton(0))
            {
                Vector3 direction = Input.mousePosition - TouchContinuesAt;
                DebugLine($"Direction: {direction.ToString()}");
                RotateModel(direction);
            }

            TouchContinuesAt = Input.mousePosition;
        }

        /* If using two fingers */
        if(Input.touchCount == 2)
        {
            /* Get input for both fingers */
            Touch TouchZero = Input.GetTouch(0);
            Touch TouchOne = Input.GetTouch(1);

            Vector2 TouchZeroPosition = TouchZero.position - TouchZero.deltaPosition;
            Vector2 TouchOnePosition = TouchOne.position - TouchOne.deltaPosition;

            float TouchMagnitude = (TouchZeroPosition - TouchOnePosition).magnitude;
            float TouchDeltaMagnitude = (TouchZero.position - TouchOne.position).magnitude;

            float MagnitudeDifference = TouchMagnitude - TouchDeltaMagnitude;

            MoveModelRelatedToCamera(MagnitudeDifference);
        }
    }

    private void MoveModelRelatedToCamera(float distance)
    {
        if(ActiveModel != null)
        {
            ActiveModel.position += Camera.main.transform.forward * distance;
        }
    }

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

    private void DebugLine(string line)
    {
        if (this.DebuggerOn)
        {
            Debug.Log(line);
        }
    }
}
