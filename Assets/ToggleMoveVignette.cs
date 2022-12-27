using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleMoveVignette : MonoBehaviour
{
    // Continuous move
    [SerializeField] private GameObject _vignette;
    private List<LocomotionVignetteProvider> _continuousMoveProviders;

    // Teleport move
    //[SerializeField] private GameObject _teleportVignette;
    //private List<LocomotionVignetteProvider> _teleportMoveProviders;

    // Start is called before the first frame update
    void Start()
    {
        _continuousMoveProviders = _vignette.GetComponent<TunnelingVignetteController>().locomotionVignetteProviders;
    }

    public void SetMoveVignetteFromState(bool state)
    {
        foreach (var provider in _continuousMoveProviders)
        {
            if (provider.locomotionProvider.ToString().Contains("ActionBasedContinuousMoveProvider"))
            {
                provider.enabled = state;
            }
        }
    }
}
