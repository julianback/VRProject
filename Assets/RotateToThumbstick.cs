using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Allows us to get values for the thumbstick
using UnityEngine.XR.Interaction.Toolkit;

public class RotateToThumbstick : MonoBehaviour
{
    [SerializeField] private InputActionProperty _thumbstick;
    [SerializeField] private XRRayInteractor _rayInteractor;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 thumbstickValue = _thumbstick.action.ReadValue<Vector2>(); // (x,z) values of the thumbstick: (0,1) is right, (1,0) is forward
        float angle = Mathf.Rad2Deg * Mathf.Atan2(thumbstickValue.y, thumbstickValue.x);
        float rayInteractorY = _rayInteractor.transform.rotation.eulerAngles.y; // y-rotation of the reticle
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(0, rayInteractorY + 90 - angle, 0)));
    }
}
