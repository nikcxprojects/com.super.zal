using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioConfig : MonoBehaviour
{

    [Serializable]
    private struct AudioObject
    {
        [SerializeField] private AudioClip clip;
        [SerializeField] private Variables.SettingsFieldType type;
    }

    [SerializeField] private AudioObject[] audio;
    
    public void GetByName(){}
}
