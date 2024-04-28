using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleCollision : MonoBehaviour
{
    public AiAgentTry3 aiAgent;
    #region collision

    public bool isCollidingWithPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ai is colliding with player");

            aiAgent.canSeePlayer = true;
            aiAgent.lastPlayerSeenPosition = aiAgent.playerRefFlastPos;
            aiAgent.suspisionTimer = aiAgent.suspisionTime;

            isCollidingWithPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("ai is not colliding with player");
            
            isCollidingWithPlayer = false;
        }
    }

    #endregion
}
