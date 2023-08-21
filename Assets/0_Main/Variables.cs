using System;

public struct Variables
{
        public const string Music = "Music";
        public const string Sound = "Sound";
        public const string Vibration = "Vibration";

        public enum SettingsFieldType
        {
                Music,
                Sound,
                Vibration
        }

        public static string GetNameByType(SettingsFieldType type)
        {
                return type switch
                {
                        SettingsFieldType.Music => Music,
                        SettingsFieldType.Sound => Sound,
                        SettingsFieldType.Vibration => Vibration,
                        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };
        }
}