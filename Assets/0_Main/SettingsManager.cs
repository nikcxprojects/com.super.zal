using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class SettingsManager : MonoBehaviour
{
    [Serializable]
    private class FieldUI
    {
        [SerializeField] private Variables.SettingsFieldType type;
        public Variables.SettingsFieldType Type => type;

        [SerializeField] private Image checkmark;
        public Image Checkmark => checkmark;

        public string PlayerPrefsName => Variables.GetNameByType(type);
        
        public FieldUI(Variables.SettingsFieldType type)
        {
            this.type = type;
        }
    }

    [SerializeField] private FieldUI[] fields = new []
    {
         new FieldUI(Variables.SettingsFieldType.Music), 
         new FieldUI(Variables.SettingsFieldType.Sound), 
         new FieldUI(Variables.SettingsFieldType.Vibration)
    };

    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    private GamesController _gamesController;
    private void Start()
    {
        UpdateUI();
    }
    
    private void UpdateUI()
    {
        foreach (var field in fields) field.Checkmark.color = PlayerPrefs.GetInt(field.PlayerPrefsName) == 1 ?
            activeColor : inactiveColor;
        RestartAudio();
    }

    public void ChangeFieldValue(string playerPrefsName)
    {
        var field = GetByString(playerPrefsName);
        var value = PlayerPrefs.GetInt(field.PlayerPrefsName);
        PlayerPrefs.SetInt(field.PlayerPrefsName, value == 0 ? 1 : 0);
        UpdateUI();
    }

    private FieldUI GetByString(string name)
    {
        return fields.FirstOrDefault(field => field.PlayerPrefsName.Equals(name));
    }
    

    public void ClosePanel()
    {
        SettingsController.getInstance().HidePanel(gameObject);
    }

    private void RestartAudio()
    {
        if (!_gamesController) _gamesController = FindObjectOfType<GamesController>();
        var aObjs = FindObjectsOfType<AudioSource>();
        foreach (var aObj in aObjs) Destroy(aObj);
        _gamesController.ReshowCurrentPanel();
    }
}
