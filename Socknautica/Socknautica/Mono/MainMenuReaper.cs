namespace Socknautica.Mono;

internal class MainMenuReaper : MonoBehaviour
{
    private float lifeTime;
    private float startTime;
    private Rigidbody rb;

    private void Start()
    {
        startTime = Time.time;
        lifeTime = Random.Range(17f, 18f);
        Destroy(gameObject, lifeTime);
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Time.time < startTime + lifeTime * 0.4 && transform.position.y < 0f)
        {
            rb.AddForce(Vector3.up * 6000f, ForceMode.Force);
        }
    }
}
