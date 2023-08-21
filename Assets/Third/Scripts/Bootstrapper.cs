using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Execute()
    {
        Object.Instantiate(Resources.Load("Viewer"));
    }
}
