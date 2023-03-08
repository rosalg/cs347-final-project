using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;

public class PlayerTeleporter : MonoBehaviour
{
    public GameObject _LocationToTeleport;
    public bool _GoingToLung;
    public bool _GoingToExt;

    void OnTriggerEnter()
    {
        GameObject player = GameObject.Find("XROrigin");
        player.gameObject.transform.position = _LocationToTeleport.transform.position;

        if (_GoingToLung)
        {
            GameManager.instance._playerInLung = true;
        } else
        {
            GameManager.instance._playerInLung = false;
        }
        
        if (_GoingToExt)
        {
            GameManager.instance._playerInExt = true;
        } else
        {
            GameManager.instance._playerInExt = false;
        }

    }


}
