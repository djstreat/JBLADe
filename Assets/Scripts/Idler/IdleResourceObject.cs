using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class IdleResourceObject : MonoBehaviour
{
    private GameManager _manager;
    private TextMeshProUGUI _resourceTextBox; // Textbox component
    private TextMeshProUGUI _numWorkersTextBox; // Worker Text component
    private Sprite _buildingSprite;
    [SerializeField] private int resourceCount;

    public GameObject textboxGO; // Textbox object
    public GameObject workerTextGO;
    public string resourceName;
    public Stack<WorkerUnit> workers;
    public int resourceQuality;
    
    // Start is called before the first frame update
    void Start()
    {
        var managerObj = GameObject.FindGameObjectWithTag("Manager");
        _manager = managerObj.GetComponent<GameManager>();
        _resourceTextBox = textboxGO.GetComponent<TextMeshProUGUI>();
        _numWorkersTextBox = workerTextGO.GetComponent<TextMeshProUGUI>();
        workers = _manager.GetAssignedWorkers(this);
        Debug.Log($"Initialized {resourceName} Resource");
    }

    public void Collect()
    {
        resourceCount += workers.Count * resourceQuality;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Update Text
        _resourceTextBox.text = $"{resourceName}: {resourceCount}";
        _numWorkersTextBox.text = $"{workers.Count}";
    }

    public void IncreaseWorkers()
    {
        WorkerUnit newWorker = _manager.AssignWorker();
        if (newWorker == null)
        {
            Debug.Log("No workers to assign.");
        }
        else
        {
            newWorker.active = true;
            newWorker.assignedResource = this;
            workers.Push(newWorker);
            Debug.Log("Assigned worker.");
        }
    }

    public void ReduceWorkers()
    {
        if (workers.Count <= 0) return;
        WorkerUnit tempWorker = workers.Pop();
        tempWorker.active = false;
        tempWorker.GoHome();
        tempWorker.assignedResource = null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log($"{resourceName} was triggered.");
        if (other.CompareTag("Worker"))
        {
            WorkerUnit worker = other.GetComponent<WorkerUnit>();
            worker.ToggleTarget(); // Switches direction
        }
    }
}
