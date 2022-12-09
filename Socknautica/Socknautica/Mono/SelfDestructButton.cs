using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica.Mono;

internal class SelfDestructButton : HandTarget, IHandTarget
{
    private bool clicked;

    public void OnHandClick(GUIHand hand)
    {
        if (clicked) return;
        clicked = true;
        SelfDestructProcess.Begin(gameObject.GetComponentInParent<Rocket>());
    }

    public void OnHandHover(GUIHand hand)
    {
        HandReticle.main.SetInteractText("Self destruct", false, HandReticle.Hand.Left);
    }
}
