using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleTurnVignette : MonoBehaviour
{
    [SerializeField] private GameObject _vignette;
    private List<LocomotionVignetteProvider> _locomotionMoveProviders;

    // Start is called before the first frame update
    void Start()
    {
        _locomotionMoveProviders = _vignette.GetComponent<TunnelingVignetteController>().locomotionVignetteProviders;
    }

    public void SetTurnVignetteFromState(bool state)
    {
        foreach (var provider in _locomotionMoveProviders)
        {
            if (provider.locomotionProvider.ToString().Contains("ActionBasedContinuousTurnProvider"))
            {
                provider.enabled = state;
            }
            else if (provider.locomotionProvider.ToString().Contains("ActionBasedSnapTurnProvider"))
            {
                provider.enabled = state;
            }
        }
    }
}
