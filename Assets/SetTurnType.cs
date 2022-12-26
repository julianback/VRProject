using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTurnType : MonoBehaviour
{
    [SerializeField] private ActionBasedSnapTurnProvider _snapTurn;
    [SerializeField] private ActionBasedContinuousTurnProvider _continuousTurn;

    // Start is called before the first frame update
    void Start()
    {
        _continuousTurn.enabled = true; // default
        _snapTurn.enabled = false;
    }

    public void SetTypeFromIndex(int index)
    {
        if (index == 0)
        {
            // Use continuous turn
            _continuousTurn.enabled = true;
            _snapTurn.enabled = false;
        }
        else if (index == 1)
        {
            // Use snap turn
            _snapTurn.enabled = true;
            _continuousTurn.enabled = false;
        }
        // https://forum.unity.com/threads/xr-interaction-toolkit-throwing-nullreferenceexception.1359538/
    }
}
