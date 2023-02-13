using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ECCLibrary;
using ECCLibrary.Internal;

namespace Socknautica.Mono.Creatures
{
    public class BloopVehicleCinematic : MonoBehaviour
    {
        public GameObject throat;

        bool playing;
        Vector3 startPos;
        Quaternion startRot;
        float time;
        bool waitingForPlayerToExit;

        IEnumerator Start()
        {
            playing = true;
            startPos = transform.position;
            startRot = transform.rotation;
            time = 0f;
            bool killPlayer = false;
            if (Player.main.GetVehicle() == gameObject.GetComponent<Vehicle>())
            {
                killPlayer = true;
            }
            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = true;
            if (killPlayer)
            {
                MainCameraControl.main.ShakeCamera(4f, 1.5f, MainCameraControl.ShakeMode.BuildUp, 1.2f);
            }
            yield return new WaitForSeconds(1.5f);
            playing = false;
            GetComponent<LiveMixin>().TakeDamage(9999f);
            if (killPlayer)
            {
                Player.main.liveMixin.Kill(DamageType.Normal);
            }
            waitingForPlayerToExit = true;
        }

        void Update()
        {
            if (playing)
            {
                transform.position = Vector3.Lerp(startPos, throat.transform.position, time);
                transform.rotation = Quaternion.RotateTowards(startRot, throat.transform.rotation, Time.deltaTime * 360f);
                time += Time.deltaTime / 2f;
            }
            if (waitingForPlayerToExit)
            {
                if (Player.main.currentMountedVehicle == null && !Player.main.transform.IsChildOf(gameObject.transform))
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
