using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] CO2SpawnLocations;
    public GameObject[] O2SpawnLocations;
    public GameObject CO2;
    public GameObject O2;
    public float _O2SpawnTime;
    public float _CO2SpawnTime;

    [HideInInspector] public bool _playerInLung;
    [HideInInspector] public bool _playerInExt;

    public static GameManager instance;

    private Coroutine _runningO2Coroutine;
    private Coroutine _runningCO2Coroutine;
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

        _playerInLung = false;
        _playerInExt = false;
        _runningO2Coroutine = null;
        _runningCO2Coroutine = null;
    }

    private void Update()
    {
        if (_playerInLung && _runningO2Coroutine == null)
        {
            _runningO2Coroutine = StartCoroutine(SpawnMolecule(O2, _O2SpawnTime));
        }

        if (!_playerInLung && _runningO2Coroutine != null)
        {
            StopCoroutine(_runningO2Coroutine);
            _runningO2Coroutine = null;
        }

        if (_playerInExt && _runningCO2Coroutine == null)
        {
            _runningCO2Coroutine = StartCoroutine(SpawnMolecule(CO2, _CO2SpawnTime));
        }

        if (!_playerInExt && _runningCO2Coroutine != null)
        {
            StopCoroutine(_runningCO2Coroutine);
            _runningCO2Coroutine = null;
        }
    }

    IEnumerator SpawnMolecule(GameObject moleculeToSpawn, float spawnTime)
    {
        while (true)
        {
            GameObject newO2 = Instantiate(moleculeToSpawn);
            newO2.transform.position = O2SpawnLocations[0].transform.position;
            newO2.GetComponent<Rigidbody>().velocity = new Vector3(Random.value * _speed, -Random.value, Random.value * _speed);
            yield return new WaitForSeconds(spawnTime);
        }
    }

}
