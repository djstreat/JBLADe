using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private TextMeshProUGUI _workerCountGUI;
    private const float SecondsToIdleTrigger = 2.0f;
    private List<IdleResourceObject> _idlerResourceObjects;
    private float _idlerTriggerTime;

    public List<WorkerUnit> allWorkers;
    public GameObject workerCountObj;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get workers in scene if they exist
        GameObject[] strayWorkers = GameObject.FindGameObjectsWithTag("Worker");
        foreach (var worker in strayWorkers)
        {
            var workerUnit = worker.GetComponent<WorkerUnit>();
            allWorkers.Add(workerUnit);
        }
        Debug.Log($"Found {allWorkers.Count} Worker(s)");
        
        _idlerTriggerTime = 0.0f;
        _idlerResourceObjects = new List<IdleResourceObject>();
        Debug.Log("Manager IS LOADING.");
        var objs = GameObject.FindGameObjectsWithTag("Resource");
        foreach (var resource in objs)
        {
            _idlerResourceObjects.Add(resource.GetComponent<IdleResourceObject>());
        }
        Debug.Log($"Manager has {_idlerResourceObjects.Count} objs.");
        if (workerCountObj != null) _workerCountGUI = workerCountObj.GetComponent<TextMeshProUGUI>();
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
        
        // Track the Worker Count
        _workerCountGUI.text = $"Worker Count: {allWorkers.Count}";
    }

    public WorkerUnit AssignWorker()
    {
        // Get Idle workers
        foreach (var workerUnit in allWorkers)
        {
            if (!workerUnit.active)
            {
                return workerUnit;
            }
        }
        return null;
    }

    public Stack<WorkerUnit> GetAssignedWorkers(IdleResourceObject resource)
    {
        Stack<WorkerUnit> assignedWorkers = new Stack<WorkerUnit>();
        foreach (var worker in allWorkers)
        {
            if (worker.assignedResource == resource) assignedWorkers.Push(worker);
        }

        return assignedWorkers;
    }
}
