using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerRunnerFollow : MonoBehaviour
{
    public Transform playerRef;
    public Vector3 offset;

    private void Update()
    {
        //get direction to player
        Vector3 direction = playerRef.position - transform.position;
        //transform.position= new Vector3( playerRef.position.x + offset.x, playerRef.position.y + offset.y, playerRef.position.y + offset.y);
        transform.rotation= Quaternion.LookRotation(direction);
        transform.position = playerRef.position + offset;
    }
}
