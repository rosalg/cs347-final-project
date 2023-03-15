using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.Events;
using TMPro;

public class PlayerTeleporter : MonoBehaviour
{
    public enum BodyPart
    {
        Heart,
        Lungs,
        Leg,
        Arm
    }

    public GameObject locationToTeleport;
    public BodyPart Destination;

    [HideInInspector] public UnityTeleportEvent OnPlayerTeleport;


    public void Start()
    {

        OnPlayerTeleport.AddListener(GameManager.instance.HandlePlayerTeleport);
    }

    void OnTriggerEnter()
    {
        GameObject player = GameObject.Find("XROrigin");
        player.gameObject.transform.position = locationToTeleport.transform.position;
        OnPlayerTeleport.Invoke(Destination);

    }

    [System.Serializable]
    public class UnityTeleportEvent : UnityEvent<BodyPart> { };
}
