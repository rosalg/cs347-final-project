using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject[] CO2SpawnLocations;
    public GameObject[] O2SpawnLocations;
    public GameObject CO2;
    public GameObject O2;
    public float O2SpawnTime;
    public float CO2SpawnTime;

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
            _runningO2Coroutine = StartCoroutine(SpawnMolecule(O2, O2SpawnTime, O2SpawnLocations[0]));
        }

        if (!_playerInLung && _runningO2Coroutine != null)
        {
            StopCoroutine(_runningO2Coroutine);
            _runningO2Coroutine = null;
        }

        if (_playerInExt && _runningCO2Coroutine == null)
        {
            _runningCO2Coroutine = StartCoroutine(SpawnMolecule(CO2, CO2SpawnTime, CO2SpawnLocations[0]));
        }

        if (!_playerInExt && _runningCO2Coroutine != null)
        {
            StopCoroutine(_runningCO2Coroutine);
            _runningCO2Coroutine = null;
        }
    }

    IEnumerator SpawnMolecule(GameObject moleculeToSpawn, float spawnTime, GameObject spawnPosition)
    {
        while (true)
        {
            GameObject newMolecule = Instantiate(moleculeToSpawn);
            newMolecule.transform.position = spawnPosition.transform.position;
            newMolecule.GetComponent<Rigidbody>().velocity = new Vector3(Random.value * _speed, -Random.value, Random.value * _speed);
            yield return new WaitForSeconds(spawnTime);
        }
    }

}
