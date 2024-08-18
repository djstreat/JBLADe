using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.UI;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
public class WorkerUnit : MonoBehaviour
{
    private bool headHome = false;
    public bool active = false;
    public bool dead = false;
    public GameObject homeObject;
    public IdleResourceObject assignedResource;
    public float speed;
    
    // Start is called before the first frame update
    void Start()
    {
        homeObject = GameObject.FindGameObjectWithTag("Home");
        // assignedResource = null;
    }

    private void AssignResource(GameObject resourceObject)
    {
        // Assigns worker to a new resource.
        assignedResource = resourceObject.GetComponent<IdleResourceObject>();
    }

    public void ToggleTarget()
    {
        headHome = !headHome;
    }

    public void MoveToObjective()
    {
        // Worker should always be in motion while they have an objective.
        Vector3 targetPosition = headHome ? homeObject.transform.position : assignedResource.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    

    // Update is called once per frame
    void Update()
    {
        // If unassigned, become inactive.
        active = assignedResource is not null;

        if (active)
        {
            MoveToObjective();
        }
    }
}
