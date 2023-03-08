using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryManager : MonoBehaviour
{
    public GameObject _ToInstantiate;
    private int _count;

    private void Start()
    {
        _count = 1;
    }
    public void SpawnInteractable(SelectEnterEventArgs args)
    {
        if (_count > 0) { 
            XRInteractionManager XRIM = FindAnyObjectByType<XRInteractionManager>();
            GameObject Interactable = Instantiate(_ToInstantiate);
            XRIM.SelectEnter(args.interactorObject, Interactable.GetComponent<XRGrabInteractable>());
            _count -= 1;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Molecule molecule = collision.gameObject.GetComponent<Molecule>();
        if (molecule != null)
        {
            if (molecule._WasReleasedByPlayer)
            {
                _count += 1;
                Destroy(collision.gameObject);
            }
        }
    }


}
