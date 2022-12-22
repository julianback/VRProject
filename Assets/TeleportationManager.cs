using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor rayInteractor;
    [SerializeField] private GameObject reticle;
    [SerializeField] private TeleportationProvider teleportationProvider;

    private InputAction _thumbstick;
    private InputAction _trigger;
    private InputAction _grip;

    private bool _readyToTeleport; // true when the thumbstick has been returned to centre
    private bool _isTeleporting;   // true when the user presses the trigger

    private Transform _reticlePrefab;

    // Start is called before the first frame update
    void Start()
    {
        TurnOffRay();

        _isTeleporting = false;
        _readyToTeleport = true;

        // activate
        _trigger = actionAsset.FindActionMap("XRI LeftHand Interaction").FindAction("Activate");
        _trigger.Enable();
        _trigger.performed += OnTeleportActivate;

        // cancel
        _grip = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Cancel");
        _grip.Enable();
        _grip.performed += OnTeleportCancel;

        // select
        _thumbstick = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        _thumbstick.Enable();

        _reticlePrefab = reticle.transform.Find("Directional Teleport Reticle");
        if (_reticlePrefab == null)
        {
            Debug.LogError("Directional Teleport Reticle not found");
        }

        // push the thumbstick forward to show the line
        // press the trigger to teleport
        // press the grip to cancel
        // release the thumbstick to cancel
        // keep the line out between teleports
        // if cancelled, release the thumbstick to start again

        /*
        TODO
        rotate player to reticle when teleporting
        add invalid teleportation reticle
        make this ^^ rotate when invalid
        add continuous movement
        add continuous turn
        add menu
        add option to select either continuous turn or snap turn
        add option to select either teleportation or continuous movement
        add option for left or right handed controls (right handed we teleport with the left and turn with the right, and vice versa)
        make README.md and explain teleportation and controls
        tags for anchor and free telesport areas
        */
    }

    // Update is called once per frame
    void Update()
    {
        // Only teleport if we are moving the thumbstick, and ready to teleport (that is, haven't previously cancelled
        // teleportation before returning the thumbstick to centre)

        if (!_thumbstick.IsPressed())
        {
            // The thumbstick is at the centre so there is no ray and we are ready to teleport
            _readyToTeleport = true;
            TurnOffRay();
            return;
        }

        if (!_readyToTeleport)
        {
            // We are not ready to teleport (that is, the thumbstick hasn't been returned to centre)
            return;
        }

        // The thumbstick is pressed and we are ready to teleport
        rayInteractor.enabled = true;

        if (!_isTeleporting)
        {
            // The trigger has not been pressed
            return;
        }

        // The trigger has been pressed. Check if the destination is valid
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            // TODO Check if the hit has a teleportation anchor component and if so, rotate to that. otherwise, rotate to the reticle
            // TODO check if this can be moved out of if statement

            Quaternion newRotation = new Quaternion();
            newRotation.eulerAngles = new Vector3(0, _reticlePrefab.rotation.eulerAngles.y, 0);

            TeleportRequest request = new()
            {
                destinationPosition = hit.point,
                destinationRotation = newRotation,
                matchOrientation = MatchOrientation.TargetUpAndForward
            };
            teleportationProvider.QueueTeleportRequest(request);
        }
        _isTeleporting = false;
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        // Run this when the trigger is pressed
        if (_thumbstick.IsPressed() && _readyToTeleport)
        {
            _isTeleporting = true;
        }
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        // Run this when the grip is pressed
        _readyToTeleport = false;
        _isTeleporting = false;
        TurnOffRay();
    }

    void TurnOffRay()
    {
        rayInteractor.enabled = false;
        reticle.SetActive(false);
    }
}
