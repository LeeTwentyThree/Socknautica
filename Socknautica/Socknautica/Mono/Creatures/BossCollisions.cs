using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Socknautica.Mono.Creatures;

// general class for things related to the gargantuan adult leviathan's body collisions and other interactions it may have with the world, including water currents
internal class BossCollisions : MonoBehaviour
{
    private GameObject _collisionRoot;
    private List<BodyCollider> _bodyColliders;
    private AnimationCurve _thiccnessCurve = new AnimationCurve(new Keyframe(0f, 35), new Keyframe(0.5f, 20), new Keyframe(0.75f, 10), new Keyframe(1f, 5f));
    private Collider[] _headColliders;

    private bool _collidersEnabled = true;

    private void SetupCollisions()
    {
        Transform spineRoot = transform.Find("Multigarg/Armature/Origin/Base/Spine2");
        var spines = new List<Transform>();
        int spineIndex = 3;
        Transform current = spineRoot;
        do
        {
            current = current.Find("Spine" + spineIndex);
            if (current != null) spines.Add(current);
            spineIndex++;
        }
        while (current != null);

        _headColliders = gameObject.GetComponents<Collider>();

        _collisionRoot = new GameObject(gameObject.name + "-BodyCollision");
        var rb = _collisionRoot.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.mass = 1E9F;
        _collisionRoot.transform.parent = transform;

        _bodyColliders = new List<BodyCollider>();
        int colliderIndex = 0;
        for (int i = 1; i < spines.Count; i += 2)
        {
            if (i > spines.Count)
            {
                break;
            }

            float percentDownSpine = (float)i / spines.Count;

            var colliderObject = new GameObject($"Collider-{colliderIndex}");
            colliderObject.transform.parent = _collisionRoot.transform;

            var capsule = colliderObject.gameObject.AddComponent<CapsuleCollider>();
            capsule.radius = _thiccnessCurve.Evaluate(percentDownSpine);
            capsule.height = 160f;
            var bodyCollider = new BodyCollider(capsule.transform, capsule, spines[i]);
            _bodyColliders.Add(bodyCollider);
            UpdateBodyCollider(bodyCollider);
            foreach (var headCollider in _headColliders) // very important so the garg does not become a rocket ship and launch itself into the stratosphere
            {
                Physics.IgnoreCollision(headCollider, capsule);
            }
            colliderIndex += 1;
        }
    }

    private void Start()
    {
        if (true)
        {
            SetupCollisions();
        }
    }

    private void FixedUpdate()
    {
        if (_bodyColliders == null)
        {
            return;
        }
        foreach (var collider in _bodyColliders)
        {
            UpdateBodyCollider(collider);
        }
    }

    private void Update()
    {
        if (_bodyColliders == null)
        {
            return;
        }
        var shouldUseCollision = ShouldUseCollisionThisFrame();
        ToggleColliders(shouldUseCollision);
    }

    private void ToggleColliders(bool newState)
    {
        if (_collidersEnabled == newState)
        {
            return;
        }
        _collidersEnabled = newState;
        foreach (var collider in _bodyColliders)
        {
            collider.colliderComponent.enabled = newState;
        }
    }

    private void UpdateBodyCollider(BodyCollider collider)
    {
        collider.colliderTransform.SetPositionAndRotation(collider.bone.position, collider.bone.rotation);
    }

    private bool ShouldUseCollisionThisFrame()
    {
        return true;
    }

    public class BodyCollider
    {

        public Transform colliderTransform;
        public Collider colliderComponent;
        public Transform bone;

        public BodyCollider(Transform colliderTransform, Collider colliderComponent, Transform bone)
        {
            this.colliderTransform = colliderTransform;
            this.colliderComponent = colliderComponent;
            this.bone = bone;
        }
    }
}