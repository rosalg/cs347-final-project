using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;

public interface ITap
{
    public void SpawnInteractable(SelectEnterEventArgs args);
}
