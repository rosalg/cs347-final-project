using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molecule : MonoBehaviour
{
    public enum Element
    {
        O2,
        CO2
    };

    public float despawnWaitTime = 5;
    public Element element;

    [HideInInspector] public bool wasReleasedByPlayer;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DespawnThis());
        wasReleasedByPlayer = false;
    }

    IEnumerator DespawnThis()
    {
        yield return new WaitForSeconds(despawnWaitTime);
        Destroy(gameObject);
    }

    public void UpdateRelease(bool WasReleased)
    {
        wasReleasedByPlayer = WasReleased;
    }

}
