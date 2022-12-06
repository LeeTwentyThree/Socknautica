using UWE;
using System.Collections;

namespace Socknautica.Mono.Alien;

// for terminals
public class DataTerminalAnalyzeTech : MonoBehaviour
{
    public TechType[] techsToAnalyze;

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
        for (int i = 0; i < techsToAnalyze.Length; i++)
        {
            KnownTech.Add(techsToAnalyze[i]);
            yield return new WaitForSeconds(7f);
        }
    }
}