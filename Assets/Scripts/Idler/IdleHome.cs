using UnityEngine;

namespace Idler
{
    public class IdleHome : MonoBehaviour
    {
        public GameObject homeUIPanel;

        private float timeHovered = 0.0f;
        public float timeToOpenPanel = 0.6f;

        public void ClosePanel()
        {
            if (homeUIPanel) homeUIPanel.SetActive(false);
        }

        public void OpenPanel()
        {
            if (homeUIPanel) homeUIPanel.SetActive(true);
        }

        private void OnMouseOver()
        {
            // While hovering, count time.
            timeHovered += Time.deltaTime;
        
            // Open panel if hovered for enough time.
            if (timeHovered >= timeToOpenPanel) OpenPanel();
        }

        private void OnMouseExit()
        {
            // Reset time on exit.
            timeHovered = 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"Home was triggered.");
            if (other.CompareTag("Worker"))
            {
                WorkerUnit worker = other.GetComponent<WorkerUnit>();
                if (worker.headHome == true) worker.ToggleTarget(); // Switches direction
            }
        }
    }
}
