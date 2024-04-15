using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SignalReceiver))] 

public class DeathCutscene : Interactable
{

    [SerializeField] private GameObject cutsceneToPlay;


    public override void Activate()
    {
        base.Activate();
    }

    public override void Deactivate() 
    { 
        base.Deactivate();
    }
}
