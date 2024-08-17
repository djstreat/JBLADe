using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    
    private const float SecondsToIdleTrigger = 2.0f;
    
    private List<IdleResourceObject> _idlerResourceObjects;

    private float _idlerTriggerTime;
    
    private int _stoneCoinCount = 0;

    public TextMeshProUGUI stoneCoinText;
    public Button craftStoneCoinButton;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _idlerTriggerTime = 0.0f;
        _idlerResourceObjects = new List<IdleResourceObject>();
        Debug.Log("Manager IS LOADING.");

        // Set up the button click listener
        if (craftStoneCoinButton != null)
        {
            craftStoneCoinButton.onClick.AddListener(CraftStoneCoin);
        }
        else
        {
            Debug.LogWarning("Craft Stone Coin Button is not assigned in the inspector!");
        }
        var objs = GameObject.FindGameObjectsWithTag("Resource");
        foreach (var resource in objs)
        {
            _idlerResourceObjects.Add(resource.GetComponent<IdleResourceObject>());
        }
        Debug.Log($"Manager has {_idlerResourceObjects.Count} objs.");
        UpdateStoneCoinDisplay();
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

        // Check for Stone Coin crafting button press
        if (Input.GetKeyDown(KeyCode.C))
        {
            CraftStoneCoin();
        }
    }

    private void CraftStoneCoin()
    {
        bool canCraft = true;
        foreach (var resource in _idlerResourceObjects)
        {
            if (resource.GetResourceCount() < 1)
            {
                canCraft = false;
                break;
            }
        }

        if (canCraft)
        {
            foreach (var resource in _idlerResourceObjects)
            {
                resource.ConsumeResource(1);
            }
            _stoneCoinCount++;
            UpdateStoneCoinDisplay();
            Debug.Log($"Stone Coin crafted! Total: {_stoneCoinCount}");
        }
        else
        {
            Debug.Log("Not enough resources to craft a Stone Coin!");
        }
    }

    private void UpdateStoneCoinDisplay()
    {
        if (stoneCoinText != null)
        {
            stoneCoinText.text = $"Stone Coins: {_stoneCoinCount}";
        }
    }
}
