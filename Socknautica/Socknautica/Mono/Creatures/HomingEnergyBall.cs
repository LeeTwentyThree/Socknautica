namespace Socknautica.Mono.Creatures;

internal class HomingEnergyBall : MonoBehaviour
{
    private float mSpeed = 20;
    private float rSpeed = 100;

    private void Update()
    {
        var pPos = Player.main.transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((transform.position - pPos).normalized), Time.deltaTime * rSpeed);
        transform.position += (pPos - transform.position).normalized * mSpeed * Time.deltaTime;
    }
}
