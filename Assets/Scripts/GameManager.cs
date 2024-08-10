using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    
    private const float SecondsToIdleTrigger = 2.0f;
    
    private List<IdleResourceObject> _idlerResourceObjects;

    private float _idlerTriggerTime;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _idlerTriggerTime = 0.0f;
        _idlerResourceObjects = new List<IdleResourceObject>();
        Debug.Log("Manager IS LOADING.");
        var objs = GameObject.FindGameObjectsWithTag("Resource");
        foreach (var resource in objs)
        {
            _idlerResourceObjects.Add(resource.GetComponent<IdleResourceObject>());
        }
        Debug.Log($"Manager has {_idlerResourceObjects.Count} objs.");
    }

    // Update is called once per frame
    void Update()
    {
        // Check time to trigger a collection step 
        if (_idlerTriggerTime >= SecondsToIdleTrigger)
        {
            foreach (var resource in _idlerResourceObjects)
            {
                resource.Collect();
            }
            // Reset time checker
            _idlerTriggerTime = 0f;
        }
        
        // Add time to step
        _idlerTriggerTime += Time.deltaTime;
    }
}
