using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{

    public float _DespawnWaitTime = 5;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting despawn counter...");
        StartCoroutine(DespawnThis());    
    }

    IEnumerator DespawnThis()
    {
        yield return new WaitForSeconds(_DespawnWaitTime);
        Destroy(gameObject);
    }
}
