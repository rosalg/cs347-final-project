using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR.Interaction;
using UnityEngine.Events;
using TMPro;

public class PlayerTeleporter : MonoBehaviour
{
    public GameObject locationToTeleport;
    public bool goingToLung;
    public bool goingToExt;

    [HideInInspector] public UnityEvent OnEnterLungs;
    [HideInInspector] public UnityEvent OnEnterExt;


    public void Start()
    {

        OnEnterLungs.AddListener(GameManager.instance.PlayerEnteredLungs);
        OnEnterExt.AddListener(GameManager.instance.PlayerEnteredExt);
    }

    void OnTriggerEnter()
    {
        GameObject player = GameObject.Find("XROrigin");
        player.gameObject.transform.position = locationToTeleport.transform.position;

        if (goingToLung)
        {
            OnEnterLungs.Invoke();
        } else if (goingToExt)
        {
            OnEnterExt.Invoke();
        } else
        {
            GameManager.instance.ResetTravelState();
        }

    }


}
