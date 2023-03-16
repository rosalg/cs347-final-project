using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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

    public Sprite heart_map;
    public Sprite lung_map;
    public Sprite leg_map;
    public Sprite arm_map;

    [HideInInspector] public bool _playerInLung;
    [HideInInspector] public bool _playerInExt;

    public static GameManager instance;

    private Coroutine _runningCoroutine;
    private float _speed = 3;
    private Stage _currStage;
    private GameObject _TaskViewer;
    private TMP_Text _TaskText;

    private GameObject _MapViewer;
    private Image _MapImage;


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
        _TaskText.text = "Current Red Blood Cell Task: Travel to the lungs.";

        _MapViewer = GameObject.Find("RightHand Controller");
        _MapImage = _MapViewer.GetComponentInChildren<Image>();
        _MapImage.sprite = heart_map;

        actions.FindActionMap("XRI LeftHand Interaction").FindAction("Press Primary Button").performed += ctx => { OnPrimaryButtonPress(ctx); };
        actions.FindActionMap("XRI RightHand Interaction").FindAction("Press Primary Button").performed += ctx => { OnPrimaryButtonRightPress(ctx); };
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
            _currStage = Stage.OxygenCollection;
        } else
        {
            _currStage = (Stage)((int)_currStage + 1);
        }
        _TaskText.text = "Current Red Blood Cell Task: " + newTaskText;
    }

    public void UpdateMap(Sprite newMapSprite)
    {
        _MapImage.sprite = newMapSprite;
    }

    public void OnPrimaryButtonPress(InputAction.CallbackContext ctx)
    {
        _TaskViewer.GetComponentInChildren<Canvas>().enabled = !_TaskViewer.GetComponentInChildren<Canvas>().enabled;
    }

    public void OnPrimaryButtonRightPress(InputAction.CallbackContext ctx)
    {
        _MapViewer.GetComponentInChildren<Canvas>().enabled = !_MapViewer.GetComponentInChildren<Canvas>().enabled;
    }

    // Control over state of player location
    public void HandlePlayerTeleport(PlayerTeleporter.BodyPart part)
    {
        if (part == PlayerTeleporter.BodyPart.Heart)
        {
            ResetTravelState();
            UpdateMap(heart_map);
            AudioManager.instance.Play("Heartbeat");
        } else if (part == PlayerTeleporter.BodyPart.Lungs)
        {
            AudioManager.instance.Stop("Heartbeat");
            _runningCoroutine = StartCoroutine(SpawnMolecule(O2, O2SpawnTime, O2SpawnLocations[0]));
            UpdateMap(lung_map);
            if (_currStage == Stage.TravelToLungs)
            {
                // Todo: Change to eventually just be a list of every single stage's task text.
                UpdateStage("Collect " + O2Needed.ToString() + " oxygen molecules from the alevoli.");
            }
            else if (_currStage == Stage.ReturnToLungs)
            {
                UpdateStage("Release " + CO2Needed.ToString() + " carbon dioxide molecules into the alveoli.");
            }
        } else // This means were going to extremity!
        {
            AudioManager.instance.Stop("Heartbeat");
            _runningCoroutine = StartCoroutine(SpawnMolecule(CO2, CO2SpawnTime, CO2SpawnLocations[(int)part - 2]));
            if (part == PlayerTeleporter.BodyPart.Leg)
            {
                UpdateMap(leg_map);

            }
            else if (part == PlayerTeleporter.BodyPart.Arm)
            {
                UpdateMap(arm_map);

            }
            if (_currStage == Stage.TravelToExtremity)
            {
                UpdateStage("Release " + O2Needed + " oxygen to keep the extremity alive.");
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
            if (_currStage == Stage.OxygenCollection && inventoryObject.IsHolding(O2Needed))
            {
                UpdateStage("Travel to any extremity");
            }
        }
        if (inv.name == "CO2Inv")
        {
            if (_currStage == Stage.CollectCarbonDioxide && inventoryObject.IsHolding(CO2Needed))
            {
                UpdateStage("Return to the lungs.");
            }
        }
        if (inv.name == "O2Sink")
        {
            if (_currStage == Stage.ReleaseOxygen && inventoryObject.IsHolding(O2Needed))
            {
                UpdateStage("Collect "+ CO2Needed + " CO2");
                inventoryObject.ResetDrain();
            }
        }
        if (inv.name == "CO2Sink")
        {
            if (_currStage == Stage.ReleaseCarbonDioxide && inventoryObject.IsHolding(CO2Needed))
            {
                UpdateStage("Collect " + O2Needed.ToString() + " oxygen molecules from the alevoli.");
                inventoryObject.ResetDrain();
            }
        }
    }

}
