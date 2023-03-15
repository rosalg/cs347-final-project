using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Events;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public enum Stage
    {
        TravelToLungs,
        OxygenCollection,
        TravelToExtremity,
        ReleaseOxygen,
        CollectCarbonDioxide,
        ReturnToLungs,
        ReleaseCarbonDioxide
    }

    public GameObject[] CO2SpawnLocations;
    public GameObject[] O2SpawnLocations;
    public GameObject CO2;
    public GameObject O2;
    public float O2SpawnTime;
    public float CO2SpawnTime;
    public InputActionAsset actions;
    public int O2Needed = 10;
    public int CO2Needed = 10;

    [HideInInspector] public bool _playerInLung;
    [HideInInspector] public bool _playerInExt;

    public static GameManager instance;

    private Coroutine _runningCoroutine;
    private float _speed = 3;
    private Stage _currStage;
    private GameObject _TaskViewer;
    private TMP_Text _TaskText;

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
        _runningCoroutine = null;
        _currStage = 0;
        _TaskViewer = GameObject.Find("LeftHand Controller");
        _TaskText = _TaskViewer.GetComponentInChildren<TMP_Text>();
        _TaskText.text = "Current Red Blood Cell Task: You are in the heart. Travel to the lungs.";

        actions.FindActionMap("XRI LeftHand Interaction").FindAction("Press Primary Button").performed += ctx => { OnPrimaryButtonPress(ctx); };
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

    public void UpdateStage(string newTaskText)
    {
        if (_currStage == Stage.ReleaseCarbonDioxide)
        {
            _currStage = Stage.TravelToLungs;
        } else
        {
            _currStage = (Stage)((int)_currStage + 1);
        }
        _TaskText.text = "Current Red Blood Cell Task: " + newTaskText;
    }

    public void OnPrimaryButtonPress(InputAction.CallbackContext ctx)
    {
        //_TaskViewer.GetComponent<XRInteractorLineVisual>().enabled = !_TaskViewer.GetComponent<LineRenderer>().enabled; 
        _TaskViewer.GetComponentInChildren<Canvas>().enabled = !_TaskViewer.GetComponentInChildren<Canvas>().enabled;
    }

    // Control over state of player location
    public void HandlePlayerTeleport(PlayerTeleporter.BodyPart part)
    {
        if (part == PlayerTeleporter.BodyPart.Heart)
        {
            ResetTravelState();
        } else if (part == PlayerTeleporter.BodyPart.Lungs)
        {
            if (_currStage == Stage.TravelToLungs)
            {
                // Todo: Change to eventually just be a list of every single stage's task text.
                _runningCoroutine = StartCoroutine(SpawnMolecule(O2, O2SpawnTime, O2SpawnLocations[0]));
                UpdateStage("Collect 10 of the oxygen molecules.");
            }
            else if (_currStage == Stage.ReturnToLungs)
            {
                _runningCoroutine = StartCoroutine(SpawnMolecule(O2, O2SpawnTime, O2SpawnLocations[(int)part - 1]));
                UpdateStage("Release 10 carbon dioxide molecules.");
            }
        } else // This means were going to extremity!
        {
            _runningCoroutine = StartCoroutine(SpawnMolecule(CO2, CO2SpawnTime, CO2SpawnLocations[(int)part - 2]));
            if (_currStage == Stage.TravelToExtremity)
            {
                UpdateStage("Release oxygen to keep the extremity alive.");
            }
        }
       
    }

    public void ResetTravelState()
    {
        StopCoroutine(_runningCoroutine);
    }

    public void HandleInventoryUpdate(GameObject inv)
    {

        InventoryManager inventoryObject = inv.GetComponent<InventoryManager>();
        if (inv.name == "O2Inv")
        {
            if (_currStage == Stage.OxygenCollection && inventoryObject.IsFull())
            {
                UpdateStage("Travel to any extremity");
            }
        }
        if (inv.name == "CO2Inv")
        {
            if (_currStage == Stage.CollectCarbonDioxide && inventoryObject.IsFull())
            {
                UpdateStage("Return to the lungs.");
            }
        }
        if (inv.name == "O2Sink")
        {
            if (_currStage == Stage.ReleaseOxygen && inventoryObject.IsDrainHolding(O2Needed))
            {
                UpdateStage("Collect 10 CO2");
            }
        }
        if (inv.name == "CO2Sink")
        {
            if (_currStage == Stage.ReleaseCarbonDioxide && inventoryObject.IsDrainHolding(CO2Needed))
            {
                UpdateStage("Travel to the Lungs to collect O2");
            }
        }
    }

}
