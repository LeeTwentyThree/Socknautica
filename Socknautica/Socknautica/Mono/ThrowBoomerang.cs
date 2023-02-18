namespace Socknautica.Mono;

internal class ThrowBoomerang : PlayerTool
{
    public override string animToolName => "boomerang";

	public bool lava;

	private float pushForce = 32;
	private float returnForce = 70;
	private float contactDamage = 750;
	private float maxTravelTime = 2f;

	private bool wasThrown;
	private bool returning;
	private float lastThrowTime;

	private Transform modelTransform;
	private Rigidbody rb;

	private static FMODAsset throwSound = Helpers.GetFmodAsset("event:/tools/extinguisher/shoot");
	private static FMODAsset hitSound = Helpers.GetFmodAsset("event:/sub/common/fishsplat");
	private static FMODAsset explodeSound = Helpers.GetFmodAsset("event:/creature/lavalizard/spit_hit");

	private GameObject explosionFx;
	private GameObject explosionFxBig;

	private bool Weaponized
    {
        get
        {
			return wasThrown && !returning;
        }
    }

	private bool JustThrew
    {
        get
        {
			return Time.time < lastThrowTime + 0.2f;
		}
    }

	private void Start()
    {
		modelTransform = transform.GetChild(0);
		rb = GetComponent<Rigidbody>();
		if (lava)
        {
			var ranged = CraftData.GetPrefabForTechType(TechType.SeaDragon).GetComponent<RangedAttackLastTarget>();
			explosionFx = ranged.attackTypes[0].ammoPrefab;
			explosionFxBig = ranged.attackTypes[1].ammoPrefab;
        }
	}

    public override string GetCustomUseText()
    {
        return LanguageCache.GetButtonFormat("UseBoomerangBoomerang", GameInput.Button.LeftHand);
    }

	public override void OnToolUseAnim(GUIHand guiHand)
	{
		if (guiHand.GetTool() == this)
		{
			pickupable.Drop(GetDropPosition(), Camera.current.transform.forward * pushForce, true);
			if (!Inventory.main.Contains(pickupable))
            {
				OnThrow();
			}
		}
	}

	private void OnThrow()
    {
		wasThrown = true;
		returning = false;
		lastThrowTime = Time.time;
		modelTransform.localEulerAngles = Vector3.zero;
		Utils.PlayFMODAsset(throwSound, Player.main.transform.position);
	}

	private void OnReturn()
    {
		wasThrown = false;
		returning = false;
		Inventory.main.Pickup(pickupable);
		modelTransform.localEulerAngles = Vector3.zero;
	}

	public Vector3 GetDropPosition()
	{
		Vector3 result = MainCameraControl.main.transform.forward * 1.5f + MainCameraControl.main.transform.position;
		RaycastHit raycastHit;
		if (Physics.Raycast(new Ray(MainCameraControl.main.transform.position, MainCameraControl.main.transform.forward), out raycastHit, 1.07f, -1, QueryTriggerInteraction.Ignore))
		{
			result = raycastHit.point + raycastHit.normal * 0.2f;
		}
		return result;
	}

	public override bool OnRightHandDown()
	{
		return Inventory.CanDropItemHere(pickupable, true);
	}

	private void OnCollisionEnter(Collision col)
    {
		if (JustThrew)
		{
			return;
		}
		if (!Weaponized) return;
		if (lava)
		{
			returning = true;
			Explode();
		}
		var hitGo = col.gameObject;
		var root = UWE.Utils.GetEntityRoot(hitGo);
		if (root == null) return;
		var lm = root.GetComponent<LiveMixin>();
		if (lm == null) return;
		lm.TakeDamage(contactDamage);
		Utils.PlayFMODAsset(hitSound, Player.main.transform.position);
		returning = true;
	}

	private void Explode()
    {
		bool big = Vector3.Distance(transform.position, Player.main.transform.position) > 6;
		var spawned = Instantiate(big ? explosionFxBig : explosionFx, transform.position, Quaternion.identity);
		if (big) spawned.GetComponent<LavaMeteor>().explodeRadius = 5;
		Utils.PlayFMODAsset(explodeSound, Player.main.transform.position);
		/*var fish = CraftData.GetPrefabForTechType(TechType.LavaBoomerang);
		for (int i = 0; i < 6; i++)
        {
			Instantiate(fish, transform.position + Random.onUnitSphere * 3, Random.rotation);
        }*/
    }

	private void Update()
    {
		if (Weaponized)
        {
			modelTransform.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0), Space.Self);

			if (!JustThrew && (Time.time > lastThrowTime + maxTravelTime || rb.velocity.sqrMagnitude < 0.4f))
            {
				returning = true;
			}
		}
		if (returning)
        {
			if ((Player.main.transform.position - transform.position).sqrMagnitude < 4)
            {
				OnReturn();
            }
		}
	}

	private void FixedUpdate()
	{
		if (returning)
		{
			rb.AddForce((Player.main.transform.position - transform.position).normalized * returnForce);
		}
	}
}
