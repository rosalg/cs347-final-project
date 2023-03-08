using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InventoryManager : MonoBehaviour
{
    public GameObject _ToInstantiate;
    private int _count;
    public bool _IsO2Inv;

    private void Start()
    {
        _count = 0;
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
        if (molecule != null && ((molecule._IsO2 && _IsO2Inv) || (!molecule._IsO2 && !_IsO2Inv)))
        {
            if (molecule._WasReleasedByPlayer && _count < 10)
            {
                _count += 1;
                Destroy(collision.gameObject);
            }
        }
    }


}
