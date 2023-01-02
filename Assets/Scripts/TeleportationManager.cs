using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset _actionAsset;
    [SerializeField] private XRRayInteractor _rayInteractor;
    [SerializeField] private GameObject _reticle;
    [SerializeField] private GameObject _blockedReticle;
    [SerializeField] private TeleportationProvider _teleportationProvider;
    [SerializeField] private Material _teleportBlue;
    [SerializeField] private Material _anchorGreen;
    [SerializeField] private List<Renderer> _reticleRenderers;

    private InputAction _thumbstick;
    private InputAction _trigger;
    private InputAction _grip;

    private bool _readyToTeleport; // true when the thumbstick has been returned to centre
    private bool _isTeleporting;   // true when the user presses the trigger

    private Transform _reticlePrefab;

    /*
     * push the thumbstick forward to show the line
     * press the trigger to teleport
     * press the grip to cancel
     * release the thumbstick to cancel
     * keep the line out between teleports
     * if cancelled, release the thumbstick to start again
    */

    // Start is called before the first frame update
    void Start()
    {
        TurnOffRay();

        _isTeleporting = false;
        _readyToTeleport = true;

        // activate
        _trigger = _actionAsset.FindActionMap("XRI LeftHand Interaction").FindAction("Activate");
        _trigger.Enable();
        _trigger.performed += OnTeleportActivate;

        // cancel
        _grip = _actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Cancel");
        _grip.Enable();
        _grip.performed += OnTeleportCancel;

        // select
        _thumbstick = _actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        _thumbstick.Enable();

        _reticlePrefab = _reticle.transform.Find("Directional Teleport Reticle");
        if (_reticlePrefab == null)
        {
            Debug.LogError("Directional Teleport Reticle not found");
        }
    }

    private void OnDestroy()
    {
        _trigger.performed -= OnTeleportActivate;
        _grip.performed -= OnTeleportCancel;
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
        _rayInteractor.enabled = true;

        // Check if the destination is valid
        if (!_rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            return;
        }

        // Check if the hit is on a teleport area
        if (!hit.collider.CompareTag("TeleportArea") && !hit.collider.CompareTag("TeleportAnchor"))
        {
            _blockedReticle.SetActive(true);
            _blockedReticle.transform.position = hit.point;
            return;
        }
        // We have hit a valid teleport area so don't want the blocked reticle
        _blockedReticle.SetActive(false);

        // If an anchor, change the reticle and rotate it to the new rotation
        if (hit.collider.CompareTag("TeleportAnchor"))
        {
            ChangeReticleColour(_anchorGreen);
            // Align reticle with anchor direction and turn off rotate script
            _reticlePrefab.transform.rotation = hit.collider.transform.parent.gameObject.transform.rotation;
            _reticlePrefab.GetComponent<RotateToThumbstick>().enabled = false;
        }
        else
        {
            // Change it back to blue
            ChangeReticleColour(_teleportBlue);
            _reticlePrefab.GetComponent<RotateToThumbstick>().enabled = true;
        }

        if (!_isTeleporting)
        {
            // The trigger has not been pressed
            return;
        }

        _isTeleporting = false; // We've pressed the button. Either we succeed or fail

        Quaternion newRotation = new Quaternion();
        Vector3 newPosition = new Vector3();
        // Check if we rotate to the reticle or an anchor
        if (hit.collider.CompareTag("TeleportArea"))
        {
            newRotation.eulerAngles = new Vector3(0, _reticlePrefab.rotation.eulerAngles.y, 0);
            newPosition = hit.point;
        }
        else
        {
            // Teleportation anchor
            newRotation.eulerAngles = new Vector3(0, hit.collider.transform.rotation.eulerAngles.y, 0);
            newPosition = hit.collider.transform.parent.gameObject.transform.position; // Find the pivot offset
        }

        TeleportRequest request = new()
        {
            destinationPosition = newPosition,
            destinationRotation = newRotation,
            matchOrientation = MatchOrientation.TargetUpAndForward
        };
        _teleportationProvider.QueueTeleportRequest(request);
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
        _rayInteractor.enabled = false;
        _reticle.SetActive(false);
        _blockedReticle.SetActive(false);
    }

    void ChangeReticleColour(Material mat)
    {
        foreach (var renderer in _reticleRenderers)
        {
            renderer.material = mat;
        }
    }
}
