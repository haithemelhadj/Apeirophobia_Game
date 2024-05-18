using UnityEngine;

public class SetGroundedEvent : MonoBehaviour
{
    public PlayerMovementTest1 playerScript;
    public void OnLanding()
    {
        playerScript.playerState = PlayerMovementTest1.Pstate.standing;
        playerScript.CameraSystem(playerScript.playerState);
        Debug.Log("ground set");
    }
}
