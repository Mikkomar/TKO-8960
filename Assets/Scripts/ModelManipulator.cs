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

        /* If a countinuous finger press is detected */
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = Input.mousePosition - TouchContinuesAt;
            DebugLine($"Direction: {direction.ToString()}");
            RotateModel(direction);
        }

        TouchContinuesAt = Input.mousePosition;
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
