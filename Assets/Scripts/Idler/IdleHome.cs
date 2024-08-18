using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleHome : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Home was triggered.");
        if (other.CompareTag("Worker"))
        {
            WorkerUnit worker = other.GetComponent<WorkerUnit>();
            worker.ToggleTarget(); // Switches direction
        }
    }
}
