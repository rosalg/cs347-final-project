using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.Events;
using TMPro;
using Unity.XR.CoreUtils;

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

    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponentInParent<XROrigin>())
        {
            GameObject player = GameObject.Find("XROrigin");
            player.gameObject.transform.position = locationToTeleport.transform.position;
            player.gameObject.transform.rotation = locationToTeleport.transform.rotation;
            OnPlayerTeleport.Invoke(Destination);
        }
    }

    [System.Serializable]
    public class UnityTeleportEvent : UnityEvent<BodyPart> { };
}
