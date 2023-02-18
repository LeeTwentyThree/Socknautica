namespace Socknautica.Mono;

internal class AtmospheriumVaporization : MonoBehaviour
{
    private static FMODAsset sound = Helpers.GetFmodAsset("event:/creature/lavalizard/spit_hit");

    private void Update()
    {
        if (transform.position.y > -1500 && gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
            Utils.PlayFMODAsset(sound, Player.main.transform.position);
        }
    }
}
