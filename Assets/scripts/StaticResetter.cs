using UnityEngine;

public static class StaticResetter
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetStatics()
    {
        // SectionTrigger static de�erlerini s�f�rla
        SectionTrigger.ResetStatics();
    }
}
