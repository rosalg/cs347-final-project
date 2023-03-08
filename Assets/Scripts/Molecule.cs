using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molecule : MonoBehaviour
{

    public float _DespawnWaitTime = 5;
    [HideInInspector] public bool _WasReleasedByPlayer;
    public bool _IsO2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DespawnThis());
        _WasReleasedByPlayer = false;
    }

    IEnumerator DespawnThis()
    {
        yield return new WaitForSeconds(_DespawnWaitTime);
        Destroy(gameObject);
    }

    public void UpdateRelease(bool WasReleased)
    {
        _WasReleasedByPlayer = WasReleased;
    }
}
