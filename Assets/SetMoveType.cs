using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetMoveType : MonoBehaviour
{
    [SerializeField] private LocomotionSystem _locomotionSystem;

    private TeleportationProvider _teleportMove;
    private ActionBasedContinuousMoveProvider _continuousMove;
    private TeleportationManager _teleportationManagerScript;


    // Start is called before the first frame update
    void Start()
    {
        _teleportMove = _locomotionSystem.GetComponent<TeleportationProvider>();
        _continuousMove = _locomotionSystem.GetComponent<ActionBasedContinuousMoveProvider>();
        _teleportationManagerScript = _locomotionSystem.GetComponent<TeleportationManager>();

        // default is teleport
        _teleportMove.enabled = true;
        _teleportationManagerScript.enabled = true;
        _continuousMove.enabled = false;
    }

    public void SetMoveTypeFromIndex(int index)
    {
        if (index == 0)
        {
            // Use teleport
            _teleportMove.enabled = true;
            _teleportationManagerScript.enabled = true;
            _continuousMove.enabled = false;
        }
        else if (index == 1)
        {
            // Use continuous move
            _continuousMove.enabled = true;
            _teleportMove.enabled = false;
            _teleportationManagerScript.enabled = false;
        }
    }
}
