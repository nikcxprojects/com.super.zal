using UnityEngine;

public class SetOrientation : MonoBehaviour
{
    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Landscape;
    }
}
