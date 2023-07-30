namespace Socknautica.Mono.Creatures;

internal class BossCorpseHitFloor : MonoBehaviour
{
    private void Update()
    {
        if (transform.position.y < -1960)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            Destroy(this);
        }
    }
}
