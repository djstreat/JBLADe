using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class IdleResourceObject : MonoBehaviour
{
    private TextMeshProUGUI _resourceTextBox; // Textbox component
    private List<GameObject> _workers; // list of workers
    private Sprite _buildingSprite;
    [SerializeField] private int resourceCount;
    
    public GameObject textboxGO; // Textbox object
    public string resourceName;
    public int workersCount;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Initializing {resourceName} Resource");
        _resourceTextBox = textboxGO.GetComponent<TextMeshProUGUI>();
    }

    public void Collect()
    {
        resourceCount += workersCount;
    }
    
    // Update is called once per frame
    void Update()
    {
        _resourceTextBox.text = $"{resourceName}: {resourceCount}";
    }
}
