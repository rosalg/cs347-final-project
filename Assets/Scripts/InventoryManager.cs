using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using TMPro;

public class InventoryManager : MonoBehaviour, ITap, IDrain
{

    public GameObject inventoryType;
    public bool isDrain;
    public bool isTap;
    public bool isInfiniteDrain;
    public bool isInfiniteTap;
    public int startingInventorySize = 0;
    public int maxInventorySize;
    public TMP_Text UIElement;
    public UnityGameObjectEvent OnNewItemSpawned;
    public UnityGameObjectEvent OnNewItemDrained;

    private int _count;

    private void Start()
    {
        if (UIElement != null)
        {
            OnNewItemSpawned.AddListener(UpdateUIElement);
            OnNewItemDrained.AddListener(UpdateUIElement);
        }
        _count = startingInventorySize;

        if (UIElement != null)
            UIElement.text = inventoryType.name + ": " + _count;

        OnNewItemSpawned.AddListener(GameManager.instance.HandleInventoryUpdate);
        OnNewItemDrained.AddListener(GameManager.instance.HandleInventoryUpdate);
    }

    // Add an invocation to say hey a new thing was spawned from the player's inventory.
    public void SpawnInteractable(SelectEnterEventArgs args)
    {
        if (!isTap)
            return;

        if (_count > 0 || isInfiniteTap)
        {
            XRInteractionManager XRIM = FindAnyObjectByType<XRInteractionManager>();
            GameObject Interactable = Instantiate(inventoryType);
            XRIM.SelectEnter(args.interactorObject, Interactable.GetComponent<XRGrabInteractable>());
            if (_count > 0)
            {
                _count -= 1;
            }
            OnNewItemSpawned?.Invoke(this.gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        DrainItem(collision);
    }


    // Add an invocation to say hey a new thing was put into the player's inventory.
    public void DrainItem(Collision collision)
    {
        if (!isDrain)
            return;


        Molecule molecule = collision.gameObject.GetComponent<Molecule>();
        if (molecule != null && molecule.element == inventoryType.GetComponent<Molecule>().element)
        {
            if (molecule.wasReleasedByPlayer && (_count < maxInventorySize || isInfiniteDrain))
            {
                _count += 1;
                Destroy(collision.gameObject);
                OnNewItemDrained?.Invoke(this.gameObject);
            }
        }
    }

    public void UpdateUIElement(GameObject inv)
    {
        UIElement.text = inventoryType.name + ": " + _count.ToString();    
    }

    public bool IsFull()
    {
        return _count == maxInventorySize;
    }

    public bool IsDrainHolding(int amount)
    {
        return _count >= amount;
    }

    [System.Serializable]
    public class UnityGameObjectEvent : UnityEvent<GameObject> { };
}
