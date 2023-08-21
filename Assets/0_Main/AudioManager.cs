
using UnityEngine;

public class AudioManager
{

    public static GameObject PlayAudio(AudioClip clip, Variables.SettingsFieldType type = Variables.SettingsFieldType.Sound)
    {
        if(PlayerPrefs.GetInt(Variables.GetNameByType(type)) == 0) return null;
        if (!clip) return null;
        
        var aObj = new GameObject { name = clip.name };
        var audio = aObj.AddComponent<AudioSource>();
        audio.clip = clip;
        audio.Play();
        audio.loop = type == Variables.SettingsFieldType.Music;
        return aObj;
    }

    public static void Vibrate()
    {
        
    }
}
        
