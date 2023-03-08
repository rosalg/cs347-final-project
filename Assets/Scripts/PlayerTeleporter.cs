using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    public GameObject _LocationToTeleport;

    void OnTriggerEnter()
    {
        GameObject player = GameObject.Find("XROrigin");
        player.gameObject.transform.position = _LocationToTeleport.transform.position;
        
    }


}
