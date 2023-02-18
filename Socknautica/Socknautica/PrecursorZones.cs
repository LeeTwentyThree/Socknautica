using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socknautica;

internal class PrecursorZone
{
    public static readonly PrecursorZone[] zones = new PrecursorZone[] { new PrecursorZone(Main.aquariumPos, 100), new PrecursorZone(Main.arenaTeleporterPos, 400), new PrecursorZone(Main.coordBasePos, 350) };

    public static PrecursorZone GetOccupied(Vector3 point)
    {
        foreach (var zone in zones)
            if (zone.ContainsPoint(point))
                return zone;

        return null;
    }

    public static float GetGlobalClosenessPercent(Vector3 point)
    {
        var occupied = GetOccupied(point);
        if (occupied != null) return occupied.GetClosenessPercent(point);
        if (point.y < -2000)
        {
            return 0.6f;
        }
        return 0f;
    }

    private Vector3 origin;
    private float maxDistance;
    private float maxDistanceSqr;

    public PrecursorZone(Vector3 origin, float maxDistance)
    {
        this.origin = origin;
        this.maxDistance = maxDistance;
        maxDistanceSqr = maxDistance * maxDistance;
    }

    public bool ContainsPoint(Vector3 point)
    {
        return (origin - point).sqrMagnitude <= maxDistanceSqr;
    }

    public float GetClosenessPercent(Vector3 point)
    {
        var dist = Vector3.Distance(point, origin);
        return Mathf.Clamp01(1 - dist / maxDistance);
    }
}