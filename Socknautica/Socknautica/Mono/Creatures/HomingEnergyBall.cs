namespace Socknautica.Mono.Creatures;

internal class HomingEnergyBall : MonoBehaviour
{
    private float mSpeed = 40;
    private float rSpeed = 80;

    private void Update()
    {
        var pPos = Player.main.transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((transform.position - pPos).normalized), Time.deltaTime * rSpeed);
        transform.position += (pPos - transform.position).normalized * mSpeed * Time.deltaTime;
    }
}
