using System.Collections;
using UWE;

namespace Socknautica.Mono.Alien;

// for terminals
public class DataTerminalUnlockTech : MonoBehaviour
{
    public TechType[] techsToUnlock;

    public float delay;

    public void OnStoryHandTarget()
    {
        Invoke(nameof(Unlock), delay);
    }

    void Unlock()
    {
        CoroutineHost.StartCoroutine(DelayedUnlock());
    }

    IEnumerator DelayedUnlock()
    {
        for (int i = 0; i < techsToUnlock.Length; i++)
        {
            KnownTech.Add(techsToUnlock[i]);
            yield return new WaitForSeconds(7f);
        }
    }
}