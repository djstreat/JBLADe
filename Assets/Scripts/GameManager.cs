using System.Collections.Generic;
using Idler;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private TextMeshProUGUI _workerCountGUI;
    private const float SecondsToIdleTrigger = 2.0f;
    private List<IdleResourceObject> _idlerResourceObjects;
    private float _idlerTriggerTime;
    private int _gameSceneIndex;
    
    public GameObject workerPrefab;
    public List<WorkerUnit> allWorkers;
    public GameObject workerCountObj;
    public GameObject homeObj;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Manager IS LOADING.");
        _gameSceneIndex = 1;
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
        var objs = GameObject.FindGameObjectsWithTag("Resource");
        foreach (var resource in objs)
        {
            _idlerResourceObjects.Add(resource.GetComponent<IdleResourceObject>());
        }
        Debug.Log($"Manager has {_idlerResourceObjects.Count} objs.");
        if (workerCountObj != null) _workerCountGUI = workerCountObj.GetComponent<TextMeshProUGUI>();
        if (!homeObj) homeObj = GameObject.FindGameObjectWithTag("Home");
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

    public void CreateWorker()
    {
        // Check if you've bought the workers.
        foreach (IdleResourceObject resource in _idlerResourceObjects)
        {
            if (!resource.CanBuyWorker()) {
                Debug.Log("Not enough money to buy a worker"); // Should make a notification saying you cannot buy a worker.
                return;
            }
        }
        // Once you've confirmed, you can take the resources
        foreach (IdleResourceObject resource in _idlerResourceObjects)
        {
            resource.BuyWorker();
        }
        var newWorker = Instantiate(workerPrefab, homeObj.transform.position, quaternion.identity); // Create worker
        
        allWorkers.Add(newWorker.GetComponent<WorkerUnit>());// Add worker to the list
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

    public void RestartGame()
    {
        SceneManager.LoadScene(_gameSceneIndex);
    }
}
