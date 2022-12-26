using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTurnType : MonoBehaviour
{
    [SerializeField] private ActionBasedSnapTurnProvider snapTurn;
    [SerializeField] private ActionBasedContinuousTurnProvider continuousTurn;
    private ActionBasedSnapTurnProvider snapTurnBackup;
    private ActionBasedContinuousTurnProvider continuousTurnBackup;

    // Start is called before the first frame update
    void Start()
    {
        snapTurnBackup = snapTurn;
        continuousTurnBackup = continuousTurn;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTypeFromIndex(int index)
    {
        if (index == 0)
        {
            // Use continuous turn
            snapTurn.enabled = false;
            continuousTurn.enabled = true;
        }
        else if (index == 1)
        {
            // Use snap turn
            snapTurn.enabled = true;
            continuousTurn.enabled = false;
        }

        //snapTurn = snapTurnBackup;
        //continuousTurn = continuousTurnBackup;
        /*
        * From VR Development Discord:
        * I'm going to throw this in here because it seems like it should be a
        * common thing people want to do with VR in Unity but I've only
        * discovered one other person who had the problem, even on the official
        * Unity forums where a Unity guy got back to me confirming the issue -
        * or maybe it's so occasional, nobody has noticed it yet. So you'll
        * probably want to switch between snap turn/smooth turn and smooth
        * movement and teleportation movement in your VR app at runtime via a
        * menu or something. So you have the teleportation, smooth movement,
        * snap turn and smooth turn providers attached to your XR Origin.
        * Enabling and disabling the smooth turn, snap turn and smooth movement
        * (but not the teleportation provider as it's built differently) -
        * occasionally, Unity will throw a NullReferenceException in the depths
        * of the XR Interaction Toolkit. This is because sometimes - and only
        * sometimes - if you have the same Input Action referenced by more than
        * one movement provider, it will lose the references to the Input
        * Actions if you enable or disable them for different movement providers
        * as the XR Interaction Toolkit doesn't check these references at
        * runtime before attempting to process them. You can wrap your code in
        * as many try/catch blocks as you like, it won't make a difference
        * because it's not your code generating the error. The fix is to save
        * these InputActionProperty's in separate variables in your code and
        * IMMEDIATELY AFTER enabling/disabling the movement provider, reassign
        * them from these variables. I hope this helps somebody else, as it took
        * me a couple of weeks to figure out. Unity has confirmed this is still
        * an issue in the latest XRITK so I hope this prevents some headaches.
        */
    }
}
