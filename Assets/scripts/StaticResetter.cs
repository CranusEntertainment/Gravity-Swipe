using UnityEngine;

public static class StaticResetter
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetStatics()
    {
        // SectionTrigger static deðerlerini sýfýrla
        SectionTrigger.ResetStatics();
    }
}
