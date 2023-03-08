using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] CO2SpawnLocations;
    public GameObject[] O2SpawnLocations;
    [HideInInspector] public bool _playerInLung;
    public GameObject CO2;
    public GameObject O2;

    public static GameManager instance;

    private Coroutine _runningCoroutine;
    private float _speed = 3;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        } else
        {
            instance = this;
        }

        _playerInLung = true;
        _runningCoroutine = null;
    }

    private void Update()
    {
        if (_playerInLung && _runningCoroutine == null)
        {
            _runningCoroutine = StartCoroutine(SpawnO2());
        }

        if (!_playerInLung && _runningCoroutine != null)
        {
            StopCoroutine(_runningCoroutine);
            _runningCoroutine = null;
        }
    }

    IEnumerator SpawnO2()
    {
        while (true)
        {
            GameObject newO2 = Instantiate(O2);
            newO2.GetComponent<Rigidbody>().velocity = new Vector3(Random.value * _speed, -Random.value, Random.value * _speed);
            yield return new WaitForSeconds(10);
        }
    }
}
