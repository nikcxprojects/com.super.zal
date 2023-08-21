using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    
    private static SettingsController instance;
 
    private SettingsController() {}
 
    public static SettingsController getInstance()
    {
        if (instance == null) instance = new SettingsController();
        return instance;
    }
    
    [SerializeField] private GameObject panelPrefab;

    private GameObject _panel;
    
    public void ShowPanel()
    {
        if (!_panel)
        {
            _panel = Instantiate(panelPrefab);
        }
        else
        {
            Destroy(_panel);
            _panel = Instantiate(panelPrefab);
            _panel.GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }

    public void HidePanel(GameObject panel = null)
    {
        if(_panel) Destroy(_panel);
        else if(panel) Destroy(panel);
    }
    
}
