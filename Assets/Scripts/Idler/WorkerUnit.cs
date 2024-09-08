using UnityEngine;

namespace Idler
{
    public class WorkerUnit : MonoBehaviour
    {
        private static int _totalWorkers = 0;
        public bool headHome = false;
        public bool active = false;
        public bool dead = false;
        public GameObject homeObject;
        public IdleResourceObject assignedResource;
        public float speed;
        public string workerName;
    
        // Start is called before the first frame update
        void Start()
        {
            homeObject = GameObject.FindGameObjectWithTag("Home");
            // assignedResource = null;
            workerName = $"Worker {_totalWorkers}";
            _totalWorkers++;
        }

        private void AssignResource(GameObject resourceObject)
        {
            // Assigns worker to a new resource.
            assignedResource = resourceObject.GetComponent<IdleResourceObject>();
        }

        public void GoHome()
        {
            headHome = true;
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
}
