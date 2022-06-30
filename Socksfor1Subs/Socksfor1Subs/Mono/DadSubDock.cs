using System.Collections;
using UnityEngine;

namespace Socksfor1Subs.Mono
{
    public class DadSubDock : MonoBehaviour
    {
        public DadSubBehaviour sub;
        public Transform dockTransformSmallVehicles;
        public Transform dockTransformTank;

        public Vehicle dockedVehicle;

        private static FMODAsset _dockSound = Helpers.GetFmodAsset("DadDock");
        private static FMODAsset _undockSound = Helpers.GetFmodAsset("DadUndock");

        private float _timeLastDocked;
        private float _timeLastUndocked;

        public bool DockVehicle(Vehicle vehicle)
        {
            if (Time.time < _timeLastUndocked + 3f)
            {
                return false;
            }
            if (dockedVehicle != null)
            {
                return false;
            }
            if (vehicle.docked)
            {
                return false;
            }
            dockedVehicle = vehicle;
            LargeWorldStreamer.main.cellManager.UnregisterEntity(dockedVehicle.gameObject);
            dockedVehicle.transform.parent = sub.transform;
            if (Player.main.currentMountedVehicle == dockedVehicle)
            {
                Player.main.currentMountedVehicle = null;
            }
            dockedVehicle.docked = true;
            dockedVehicle.upgradesInput.SetDocked(Vehicle.DockType.Base);
            dockedVehicle.collisionModel.SetActive(true);
            var dockTransform = PreferredDockTransform();
            dockedVehicle.transform.position = dockTransform.position;
            dockedVehicle.transform.rotation = dockTransform.rotation;
            SkyEnvironmentChanged.Broadcast(dockedVehicle.gameObject, sub);
            sub.PlayWelcomeVoiceLine();
            Utils.PlayFMODAsset(_dockSound, dockTransform.transform.position);
            dockedVehicle.SetPlayerInside(false);
            Player.main.ExitLockedMode();
            _timeLastDocked = Time.time;
            return true;
        }

        private Transform PreferredDockTransform()
        {
            if (dockedVehicle != null && (dockedVehicle is SeaMoth || dockedVehicle is Exosuit))
            {
                return dockTransformSmallVehicles;
            }
            return dockTransformTank;
        }

        private void OnUndockingComplete(Player player)
        {
            player.SetCurrentSub(null);
            if (dockedVehicle != null)
            {
                dockedVehicle.OnUndockingComplete(player);
                SkyEnvironmentChanged.Broadcast(dockedVehicle.gameObject, (Component)null);
            }
            else
            {
                player.inExosuit = false;
                player.inSeamoth = false;
            }
            dockedVehicle = null;
        }

        private void Update()
        {
            if (Time.time > _timeLastDocked + 3f)
            {
                if (Player.main.GetVehicle() == dockedVehicle)
                {
                    UndockCurrent();
                }
            }
            if (dockedVehicle != null)
            {
                dockedVehicle.GetEnergyValues(out float charge, out float capacity);
                bool shouldCharge = charge < capacity;
                if (shouldCharge)
                {
                    if (sub.InfinitePower || (sub.powerRelay.GetPower() >= Time.deltaTime * Balance.DadRechargeUsePowerRate && sub.powerRelay.ConsumeEnergy(Time.deltaTime * Balance.DadRechargeUsePowerRate, out var _)))
                    {
                        dockedVehicle.AddEnergy(Time.deltaTime * Balance.DadRechargeVehiclesRate);
                    }
                }
                if (dockedVehicle is Exosuit)
                {
                    var mainColliderObj = Helpers.FindChild(dockedVehicle.gameObject, "mainCollider");
                    if (mainColliderObj)
                    {
                        mainColliderObj.GetComponent<Collider>().enabled = true;
                    }
                }
            }
        }

        public bool CanUndockCurrent()
        {
            if (dockedVehicle == null)
            {
                return false;
            }
            if (!dockedVehicle.docked)
            {
                return false;
            }
            return true;
        }

        public bool UndockCurrent()
        {
            if (!CanUndockCurrent())
            {
                return false;
            }
            Utils.PlayFMODAsset(_undockSound, dockedVehicle.transform.position);
            dockedVehicle.docked = false;
            dockedVehicle.transform.parent = null;
            LargeWorldStreamer.main.cellManager.RegisterEntity(dockedVehicle.gameObject);
            dockedVehicle.useRigidbody.AddForce(Vector3.down * 15f, ForceMode.VelocityChange);
            dockedVehicle = null;
            OnUndockingComplete(Player.main);
            _timeLastUndocked = Time.time;
            return true;
        }
    }
}
