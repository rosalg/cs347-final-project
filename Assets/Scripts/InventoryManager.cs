using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryManager : MonoBehaviour, ITap, IDrain
{

    public GameObject inventoryType;
    public bool isDrain;
    public bool isTap;
    public bool isInfiniteDrain;
    public bool isInfiniteTap;
    public int startingInventorySize = 0;
    public int maxInventorySize;

    private int _count;

    private void Start()
    {
        _count = startingInventorySize;
    }

    public void SpawnInteractable(SelectEnterEventArgs args)
    {
        if (!isTap)
            return;

        if (_count > 0 || isInfiniteTap)
        {
            XRInteractionManager XRIM = FindAnyObjectByType<XRInteractionManager>();
            GameObject Interactable = Instantiate(inventoryType);
            XRIM.SelectEnter(args.interactorObject, Interactable.GetComponent<XRGrabInteractable>());
            if (!isInfiniteTap)
            {
                _count -= 1;
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        DrainItem(collision);
    }

    public void DrainItem(Collision collision)
    {
        if (!isDrain)
            return;


        Molecule molecule = collision.gameObject.GetComponent<Molecule>();
        if (molecule != null && molecule.element == inventoryType.GetComponent<Molecule>().element)
        {
            if (molecule.wasReleasedByPlayer && (_count < maxInventorySize || isInfiniteDrain))
            {
                if (!isInfiniteDrain)
                {
                    _count += 1;
                }
                Destroy(collision.gameObject);
            }
        }
    }

}
