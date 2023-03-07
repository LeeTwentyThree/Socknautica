namespace Socknautica.Mono;

internal class UnlockTrophyOnGameStart : MonoBehaviour
{
    private void Update()
    {
        if (uGUI.isLoading) return;
        if (SaveWithoutSaving.GetBossDefeated() && !KnownTech.Contains(Main.statue.TechType))
        {
            KnownTech.Add(Main.statue.TechType);
        }
        Destroy(this);
    }
}
