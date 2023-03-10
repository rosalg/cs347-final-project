using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class PlayerTeleporter : MonoBehaviour
{
    public GameObject locationToTeleport;
    public bool goingToLung;
    public bool goingToExt;

    void OnTriggerEnter()
    {
        GameObject player = GameObject.Find("XROrigin");
        player.gameObject.transform.position = locationToTeleport.transform.position;

        if (goingToLung)
        {
            GameManager.instance._playerInLung = true;
        } else
        {
            GameManager.instance._playerInLung = false;
        }
        
        if (goingToExt)
        {
            GameManager.instance._playerInExt = true;
        } else
        {
            GameManager.instance._playerInExt = false;
        }

    }


}
