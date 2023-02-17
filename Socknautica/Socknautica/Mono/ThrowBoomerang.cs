namespace Socknautica.Mono;

internal class ThrowBoomerang : PlayerTool
{
    public override string animToolName => "boomerang";

    public override string GetCustomUseText()
    {
        return LanguageCache.GetButtonFormat("UseBoomerangBoomerang", GameInput.Button.LeftHand);
    }

	public override void OnToolUseAnim(GUIHand guiHand)
	{
		if (guiHand.GetTool() == this)
		{
			pickupable.Drop(GetDropPosition(), MainCameraControl.main.transform.forward * pushForce, true);
		}
	}

	public Vector3 GetDropPosition()
	{
		Vector3 result = MainCameraControl.main.transform.forward * 1.07f + MainCameraControl.main.transform.position;
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

	public float pushForce = 80;
}
