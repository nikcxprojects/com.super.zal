using OneSignalSDK;
using UnityEngine;

public class OneSignalInitializer : MonoBehaviour
{

    private void Start()
    {
        OneSignal.Default.LogLevel = LogLevel.Info;
        OneSignal.Default.AlertLevel = LogLevel.Fatal;

        OneSignal.Default.Initialize("a39b1e74-b7b1-4ca1-bdc2-dc86287b0019");
    }
}