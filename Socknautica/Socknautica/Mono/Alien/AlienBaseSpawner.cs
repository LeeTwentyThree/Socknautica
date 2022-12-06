using ECCLibrary;
using System.Collections;
using System.Collections.Generic;
using UWE;
using UnityEngine;

namespace Socknautica.Mono.Alien
{
    /// <summary>
    /// A massive class to help adding props to custom Alien Bases.
    /// </summary>
    public abstract class AlienBaseSpawner : MonoBehaviour
    {
        public const string box2x2x2 = "2f996953-bd0a-48e2-8aae-57dd6b6a2507";
        public const string box2x1x2 = "96edb813-c7c7-4c44-9bf4-5f1975edeff8";
        public const string box2x1x2_variant1 = "5ecd846d-1629-4d3c-9119-f4f16179a408";
        public const string box2x1x2_variant2 = "fa5e644a-777b-4b54-a92a-0241752b8e06";
        public const string box2x1x2_variant3 = "3c9344a2-4715-4773-9c58-dc0437002278";
        public const string light_small_spotlight = "742b410c-14d4-42c6-ac84-0e2bcaff09c1";
        public const string light_small_pointlight = "473a8c4d-162f-4575-bbef-16c1c97d1e9d";
        public const string light_small_spotlight_2 = "e3742c20-ab0b-4714-929a-cc4eea95cc18";
        public const string light_small_spotlight_animated = "5cc19d2a-24b1-4a9f-b382-1c218346721c";
        public const string light_big_animated = "6a02aa5c-8d4d-4801-aad4-ea61dccddae5";
        public const string light_big_ceiling_animated = "ce3c3053-5022-404e-a165-e31abe495f1b";
        public const string light_volumetric_1 = "8e96c4a2-6130-4f78-aad9-160cb4d42538";
        public const string light_volumetric_2 = "aa6b2ede-a1bf-4f70-980c-9ed2a51375a1";
        public const string light_verybig_novolumetrics = "5631b64f-d0f0-47f5-b7ac-f23215432070";
        public const string light_strip_animated = "88cad316-cebe-4ead-aae2-1ab31cae0de6";
        public const string light_strip_animated_long = "86cde4b3-0b29-4d03-b20c-de303a005298";
        public const string starfish = "d571d3dc-6229-430e-a513-0dcafc2c41f3";
        public const string structure_outpost_1 = "c5512e00-9959-4f57-98ae-9a9962976eaa";
        public const string structure_outpost_2 = "$542aaa41-26df-4dba-b2bc-3fa3aa84b777";
        public const string pedestal_empty1 = "78009225-a9fa-4d21-9580-8719a3368373";
        public const string pedestal_empty2 = "3bbf8830-e34f-43a1-bbb3-743c7e6860ac";
        public const string pedestal_ionCrystal_rectangle = "7e1e5d12-7169-4ff9-abcd-520f11196764";
        public const string pedestal_ionCrystal_square = "ea65ef91-e875-4157-99f9-a8f4f6dc92f8";
        public const string pedestal_ionCrystalPyramid = "2db600ca-25f7-4000-93a5-f8c2a4ec0387";
        /// <summary>
        /// 2x8x2
        /// </summary>
        public const string structure_column = "640f57a6-6436-4132-a9bb-d914f3e19ef5";
        public const string structure_doorway_divider = "db5a85f5-a5fe-43f8-b71e-7b1f0a8636fe";
        /// <summary>
        /// 4x1x4
        /// </summary>
        public const string structure_specialPlatform = "738892ae-64b0-4240-953c-cea1d19ca111";
        public const string structure_specialPlatform2 = "2a836e22-26fc-4853-98c8-fcb1f639f9ad";
        public const string structure_skeletonScanner1 = "4f5905f8-ea50-49e8-b24f-44139c6bddcf";
        public const string structure_skeletonScanner2 = "ebc943e4-200c-4789-92f3-e675cd982dbe";
        public const string structure_skeletonScanner3 = "ac2b0798-e311-4cb1-9074-fae59cd7347a";
        public const string structure_doorwaySmall = "19d017a5-2e59-4c1f-bc44-e642f7d7fbd3";
        public const string structure_cacheTeleporterDeco = "fecb50b0-eb8a-4ca1-b95b-356fc0f4ac32";
        public const string cables_attachToBase = "18aa16f9-d1d8-4ccd-8a10-7ad32a5fd283";
        public const string cables_mid01 = "69cd7462-7cd2-456c-bfff-50903c391737";
        public const string cables_mid02 = "94933bb3-0587-4e8d-a38d-b7ec4c859b1a";
        public const string cables_mid03 = "31f84eba-d435-438c-a58e-f3f7bae8bfbd";
        public const string cables_attachToWall = "a0a9237e-dee3-4efa-81ff-fea3893a6eb7";
        public const string tabletDoor_orange = "83a70a58-f7da-4f18-b9b2-815dc8a7ffb4";
        public const string prop_microscope = "a30d0115-c37e-40ec-a5d9-318819e94f81";
        public const string prop_specimensCase = "da8f10dd-e181-4f28-bf98-9b6de4a9976a";
        public const string prop_claw = "6a01a336-fb46-469a-9f7d-1659e07d11d7";
        public const string prop_tableRectangle = "68c58fba-bc8d-40fc-a137-544af418f953";
        public const string prop_dissectionTank = "44974fcd-c47a-41aa-a279-43eaf234bfa6";
        public const string prop_genericMap = "172d9440-2670-45a3-93c7-104fee6da6bc";
        public const string prop_waterTank = "0b1cf8d8-65da-4b9d-bf86-bfb96ac35ae0";
        public const string prop_wideRelicCase = "1b85a183-2084-42a6-8d85-7e58dd6864bd";
        public const string prop_relicCase = "d0fea4da-39f2-47b4-aece-bb12fe7f9410";
        /// <summary>
        /// Slightly higher than 2m tall, has a 22.5ish degree angle.
        /// </summary>
        public const string prop_tabletPedestal = "814beddb-62cf-4c55-a86d-5da0684932a8";
        public const string genericDataTerminal = "b629c806-d3cd-4ee4-ae99-7b1359b60049";
        public const string supplies_nutrientBlock = "30373750-1292-4034-9797-387cf576d150";
        public const string supplies_smallWater = "22b0ce08-61c9-4442-a83d-ba7fb99f26b0";
        public const string supplies_mediumWater = "f7fb4077-b4d7-443c-b367-349cc1d39cc8";
        public const string supplies_bigWater = "$545c54a8-b23e-41bc-9d7c-af0b729e502f";
        public const string supplies_ionCrystal = "38ebd2e5-9dcc-4d7a-ada4-86a22e01191a";
        public const string supplies_drillableIonCube = "41406e76-4b4c-4072-86f8-f5b8e6523b73";
        public const string supplies_drillableLithium = "846c3df6-ffbf-4206-b591-72f5ba11ed40";
        public const string supplies_drillableTitanium = "9f855246-76c4-438b-8e4d-9cd6d7ce4224";
        public const string supplies_reactorRod = "cfdd714a-55fb-40df-86e5-6acf0d013b34";
        public const string supplies_titaniumIngot = "41919ae1-1471-4841-a524-705feb9c2d20";
        public const string supplies_firstAidKit = "bc70e8c8-f750-4c8e-81c1-4884fe1af34e";
        public const string supplies_whiteTablet = "066e533d-f854-435d-82c6-b28ba59858e0";
        public const string supplies_redTablet = "7d19f47b-6ec6-4a25-9b28-b3fd7f5661b7";
        public const string supplies_purpleTablet = "53ffa3e8-f2f7-43b8-a5c7-946e766aff64";
        public const string supplies_orangeTablet = "58247109-68b9-411f-b90f-63461df9753a";
        public const string supplies_cutefishegg = "b78912bc-0191-4455-a9de-3b708e165393";
        public const string creature_alienRobot = "4fae8fa4-0280-43bd-bcf1-f3cba97eed77";
        public const string creature_spinefishSchool = "2d3ea578-e4fa-4246-8bc9-ed8e66dec781";
        public const string creature_holefishSchool = "a7b70c23-8e57-43e0-ab39-e02a29341376";
        public const string creature_holefish = "495befa0-0e6b-400d-9734-227e5a732f75";
        public const string creature_rockgrub = "8e82dc63-5991-4c63-a12c-2aa39373a7cf";
        public const string creature_warperSpawner = "510a71f0-ab6d-4c6a-aa54-a19b3f1c436c";
        public const string creature_jumper = "fa7fa15c-5515-497f-872d-d6d579a39f60";
        public const string atmosphereVolume_cache = "f5dc3fa5-7ef7-429e-9dc6-2ea0e97b6187";
        public const string ambience_greenLight = "0b359b03-92e4-40df-81ed-aad488a7f13e";
        public const string ambience_blueLight = "30730b57-d076-4692-bc56-c40bf2aedc13";
        public const string ambience_ghostSkeletonLight = "a1f8e7cf-83ae-438c-9197-3321374eca56";
        public const string airlock_1 = "03809334-e82d-40f5-9ccd-920e753887de";
        public const string natural_rockBlade1 = "f0438971-2761-412c-bc42-df80577de473";
        public const string natural_rockBlade2 = "282cdcbc-8670-4f9a-ae1d-9d8a09f9e880";
        public const string natural_coralClumpYellow = "5e8261d5-acce-4ec6-b77c-0f138770d5cb";
        public const string natural_coralClumpYellow_small = "339d91c3-9203-4c8f-8592-14b72ba7d5cc";
        public const string natural_coralClumpYellow_variant = "db86ef34-e1fa-4eb2-aa18-dda5af30cb45";
        public const string natural_boulder_small = "3e581d94-c873-4ad7-a2f4-a35ec6ac3ecb";
        public const string natural_boulder_big = "8d13d081-431e-4ed5-bc99-2b8b9fabe9c2";
        public const string natural_lr_hangingplant1_1 = "87f8fd44-43ca-44df-94c6-28b5962a6524";
        public const string natural_lr_hangingplant1_2 = "31d941ee-b00b-437c-abea-c2d42526ba0a";
        public const string natural_lr_hangingplant1_3 = "6d535e87-a4b1-4044-802b-f99491fe21fd";
        public const string natural_lr_hangingplant2_1 = "59f9f106-e2d4-45cc-9211-2d843d456282";
        public const string natural_lr_hangingplant2_2 = "c5664c82-d9f4-445e-86b1-b943e97e3913";
        public const string natural_lr_hangingplant2_3 = "c40f058c-e73b-4cf5-a4e5-6ce78a73899a";
        public const string natural_plant_fan = "be8b0e66-0cde-428b-be78-9d6bf06eaef4";
        public const string natural_grass_blue = "c87e584c-7e38-4589-b408-8eca51f474c1";
        public const string natural_grass_green = "26940e53-d3eb-4770-ae99-6ce4335445d3";
        public const string natural_grass_yellow = "7ecc9cdd-3afc-4005-bff7-01ba62e95a03";
        public const string natural_coral_yellow1 = "598c95d8-7420-4907-8f70-ba18b4e6adcb";
        public const string natural_coral_yellow2 = "450bf7b5-b6cf-4139-921f-3cb9ea505d5f";
        public const string natural_coral_red = "22bf7b03-8154-410b-a6fb-8ba315f68987";
        public const string natural_coral_purple = "c71f41ce-b586-4e85-896e-d25e8b5b9de0";
        public const string natural_ameboid = "375a4ade-a7d9-401d-9ecf-08e1dce38d6b";
        public const string bone_reaperMandible = "501c0536-7993-4ed6-be77-6287cedd8d02";
        public const string bone_reaperSkull = "50031120-ab7a-4f10-b497-3a97f63b4de1";
        public const string bone_reaperSpine1Rib = "949d8657-1e5c-4418-8948-76b8b712fc57";
        public const string bone_reaperFullRibcage_normal = "358012ab-6be8-412d-85ee-263a733c88ba";
        public const string bone_reaperFullRibcage_chipped = "0b6ea118-1c0b-4039-afdb-2d9b26401ad2";
        public const string bone_reaperHalfRibcage = "e10ff9a1-5f1e-4c4d-bf5f-170dba9e321b";
        public const string bone_curly = "70c0c560-1a47-46ea-9659-30c8072eb792";
        public const string bone_generic1 = "db44e245-1bf5-42b7-9da2-ab7c33e91241";
        public const string bone_generic2 = "33c42808-c360-42b6-954d-5f10d0bffdeb";
        public const string bone_lessCurly = "6be26bed-91eb-42b9-be92-314d3bd028d6";
        public const string bone_lessCurly2 = "7c5425d4-2339-436c-822a-d6b3922b489a";
        public const string bone_lessCurly3 = "e64676d7-0648-4f1e-9ab0-8e37ec877ef9";
        public const string bone_lessCurly4 = "08b4a416-2cdf-4c6b-8772-f58255e525d7";
        public const string bone_lessCurly_chipped = "6bf7e935-6e27-4b93-bc9c-25b7ec95c45e";
        public const string damageprop_smallPanel = "7ec6dc08-6324-4269-93a2-5f3974abd7ec";
        public const string damageprop_destroyedTile = "a523a6be-7358-479f-b07a-71a492e62247";
        public const string damageprop_largeChunk = "a55ec9a0-8962-4388-8afa-6f18fb5ea789";
        public const string damageprop_box = "583f8885-20fd-4c69-aa5a-5fcd7c58804b";
        public const string damageprop_box_double = "199894b7-cfd5-4d38-89e8-2117ce43824c";
        public const string damageprop_box_quadruple = "e42243eb-4f38-42cd-acec-1d38d9b1b120";
        public const string damageprop_box_quadruple2 = "ae06567b-4afd-4aff-9904-e518c1e8e30a";
        /// <summary>
        /// Faces up by default.
        /// </summary>
        public const string vfx_entrance = "8b5e6a02-533c-44cb-9f34-d2773aa82dc4";
        public const string vfx_bubbles = "a5b073a5-4bce-4bcf-8aaf-1e7f57851ba0";
        public const string hugeExteriorCube = "b9df161b-529f-422c-8a9f-f3a7a25e57df";

        public const string alterra_abandonedbase2 = "a1e2f322-7080-48ca-8eaf-a05afff8585d";
        public const string alterra_abandonedbase1 = "8f20a08c-c981-4fad-a57b-2de2106b8abf";

        private List<GameObject> spawnedChildren;

        private IEnumerator Start()
        {
            spawnedChildren = new List<GameObject>();
            yield return StartCoroutine(ConstructBase());
            foreach (GameObject obj in spawnedChildren)
            {
                if (obj is null)
                {
                    ECCLibrary.Internal.ECCLog.AddMessage("Spawned child is null");
                    continue;
                }
                obj.transform.parent = null;
                LargeWorldEntity lwe = obj.GetComponent<LargeWorldEntity>();
                if (lwe is null)
                {
                    ECCLibrary.Internal.ECCLog.AddMessage("Spawned child {0} has no LWE", obj.gameObject.name);
                    continue;
                }
                LargeWorld.main.streamer.cellManager.RegisterEntity(lwe);
                SkyApplier skyApplier = obj.GetComponent<SkyApplier>();
                if (skyApplier)
                {
                    skyApplier.ApplySkybox();
                }
            }
            Destroy(gameObject);
        }

        /// <summary>
        /// Override this method to spawn the prefabs;
        /// </summary>
        protected abstract IEnumerator ConstructBase();

        protected IEnumerator SpawnPrefab(string classId, Vector3 localPosition, IOut<GameObject> spawned = null)
        {
            yield return SpawnPrefab(classId, localPosition, Vector3.zero, spawned);
        }

        protected IEnumerator SpawnPrefab(string classId, Vector3 localPosition, Vector3 localRotation, IOut<GameObject> spawned = null)
        {
            yield return SpawnPrefab(classId, localPosition, localRotation, Vector3.one, spawned);
        }

        protected IEnumerator SpawnPrefab(string classId, Vector3 localPosition, Vector3 localRotation, Vector3 scale, IOut<GameObject> spawned = null)
        {
            IPrefabRequest request = PrefabDatabase.GetPrefabAsync(classId);
            yield return request;
            if (request.TryGetPrefab(out GameObject prefab))
            {
                GameObject spawnedObject = Instantiate(prefab, transform);
                spawnedObject.transform.localPosition = localPosition;
                spawnedObject.transform.localEulerAngles = localRotation;
                spawnedObject.transform.localScale = scale;
                spawnedObject.SetActive(true);
                spawnedChildren.Add(spawnedObject);
                if (spawned != null) spawned.Set(spawnedObject);
            }
        }

        protected IEnumerator SpawnPrefabDelayed(string classId, Vector3 localPosition, Vector3 localRotation, Vector3 scale, IOut<GameObject> spawned = null, float delay = 0f, bool insideBase = false)
        {
            yield return new WaitForSecondsRealtime(delay);
            yield return SpawnPrefab(classId, localPosition, localRotation, scale, spawned);
        }

        protected IEnumerator SpawnPrefabGlobally(string classId, Vector3 worldPosition, IOut<GameObject> spawned = null)
        {
            IPrefabRequest request = PrefabDatabase.GetPrefabAsync(classId);
            yield return request;
            if (request.TryGetPrefab(out GameObject prefab))
            {
                GameObject spawnedObject = GameObject.Instantiate(prefab);
                spawnedObject.transform.position = worldPosition;
                spawnedObject.SetActive(true);
                LargeWorld.main.streamer.cellManager.RegisterEntity(spawnedObject.GetComponent<LargeWorldEntity>());
                if (spawned != null) spawned.Set(spawnedObject);
            }
        }

        protected IEnumerator SpawnPrefabGlobally(string classId, Vector3 worldPosition, Vector3 worldRotation, Vector3 scale, IOut<GameObject> spawned = null)
        {
            IPrefabRequest request = PrefabDatabase.GetPrefabAsync(classId);
            yield return request;
            if (request.TryGetPrefab(out GameObject prefab))
            {
                GameObject spawnedObject = GameObject.Instantiate(prefab);
                spawnedObject.transform.position = worldPosition;
                spawnedObject.transform.eulerAngles = worldRotation;
                spawnedObject.transform.localScale = scale;
                spawnedObject.SetActive(true);
                LargeWorld.main.streamer.cellManager.RegisterEntity(spawnedObject);
                if (spawned != null) spawned.Set(spawnedObject);
            }
        }

        protected IEnumerator SpawnPrefabGlobally(string classId, Vector3 worldPosition, Vector3 direction, bool directionIsRight, float scaleFactor = 1f, IOut<GameObject> spawned = null)
        {
            IPrefabRequest request = PrefabDatabase.GetPrefabAsync(classId);
            yield return request;
            if (request.TryGetPrefab(out GameObject prefab))
            {
                GameObject spawnedObject = GameObject.Instantiate(prefab);
                spawnedObject.transform.position = worldPosition;
                if (directionIsRight)
                {
                    spawnedObject.transform.right = direction;
                }
                else
                {
                    spawnedObject.transform.forward = direction;
                }
                spawnedObject.transform.localScale = Vector3.one * scaleFactor;
                spawnedObject.SetActive(true);
                LargeWorld.main.streamer.cellManager.RegisterEntity(spawnedObject.GetComponent<LargeWorldEntity>());
                if (spawned != null) spawned.Set(spawnedObject);
            }
        }

        protected IEnumerator SpawnPrefabsArray(string classId, float spacing, Vector3 size, Vector3 individualScale, Vector3 offset = default, Vector3 individualEulers = default)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    for (int z = 0; z < size.z; z++)
                    {
                        Vector3 rawPosition = new Vector3(x, y, z);
                        Vector3 spacedPosition = Vector3.Scale(rawPosition, spacing * individualScale);
                        Vector3 positionWithOffset = spacedPosition - (Vector3.Scale(size, (spacing * individualScale) / 2f)) + offset;
                        yield return StartCoroutine(SpawnPrefab(classId, positionWithOffset, individualEulers, individualScale));
                    }
                }
            }
        }

        protected IEnumerator SpawnRelicInCase(Vector3 localPosition, string relicClassId, Vector3 relicOffset, Vector3 localRotation = default, Vector3 relicScale = default, Vector3 relicRotation = default, bool longRelicCase = false, Vector3 caseScale = default)
        {
            yield return StartCoroutine(SpawnPrefab(longRelicCase ? prop_wideRelicCase : prop_relicCase, localPosition, localRotation, caseScale == default ? Vector3.one : caseScale));
            if (string.IsNullOrEmpty(relicClassId))
                yield break;
            if (relicScale == default)
            {
                yield return StartCoroutine(SpawnPrefab(relicClassId, localPosition + relicOffset, localRotation + relicRotation));
            }
            else
            {
                yield return StartCoroutine(SpawnPrefab(relicClassId, localPosition + relicOffset, localRotation + relicRotation, relicScale));
            }
        }

        /// <summary>
        /// All in terms of world positions.
        /// </summary>
        /// <param name="baseAttachPosition"></param>
        /// <param name="baseAttachForward"></param>
        /// <param name="terrainPosition"></param>
        /// <param name="terrainAttachForward"></param>
        /// <param name="offsetDirection"></param>
        /// <param name="quadraticMagnitude"></param>
        protected IEnumerator GenerateCable(Vector3 baseAttachPosition, Vector3 baseAttachForward, Vector3 terrainPosition, Vector3 terrainAttachForward, Vector3 offsetDirection, float quadraticMagnitude, float scale = 1f, bool hasBase = true, bool hasClaw = true)
        {
            List<CableSegment> segments = GetCableSegments(baseAttachPosition, baseAttachForward, terrainPosition, terrainAttachForward, offsetDirection, quadraticMagnitude, scale, hasBase, hasClaw);
            foreach (CableSegment segment in segments)
            {
                yield return SpawnPrefabGlobally(segment.classId, segment.position, segment.forward, true, scale);
            }
        }

        private const float midCableSpacing = 1.05f;
        private List<CableSegment> GetCableSegments(Vector3 basePosition, Vector3 baseAttachForward, Vector3 terrainPosition, Vector3 terrainAttachForward, Vector3 offsetDirection, float quadraticMagnitude, float scale, bool hasBase, bool hasClaw)
        {
            List<CableSegment> segments = new List<CableSegment>();
            if (hasBase)
            {
                segments.Add(new CableSegment(cables_attachToBase, basePosition, baseAttachForward));
            }
            if (hasClaw)
            {
                segments.Add(new CableSegment(cables_attachToWall, terrainPosition, terrainAttachForward));
            }
            int maxSegments = Mathf.RoundToInt(Vector3.Distance(basePosition, terrainPosition) / (midCableSpacing * scale));
            for (int i = 0; i < maxSegments; i++)
            {
                float percent = (float)i / (float)maxSegments;
                if (percent == 0f)
                {
                    continue;
                }
                Vector3 position = GetPositionOfCable(basePosition, terrainPosition, percent, offsetDirection, quadraticMagnitude);

                Vector3 forwardAfter = baseAttachForward;
                float nextPercent = Mathf.Clamp01((i + 1) / maxSegments);
                if (nextPercent == 1f)
                {
                    forwardAfter = terrainAttachForward;
                }
                else
                {
                    Vector3 nextSegmentPosition = GetPositionOfCable(basePosition, terrainPosition, nextPercent, offsetDirection, quadraticMagnitude);
                    forwardAfter = (position - nextSegmentPosition).normalized;
                }

                segments.Add(new CableSegment(GetMiddleCableRandom(i), position, forwardAfter));
            }
            return segments;
        }

        private Vector3 GetPositionOfCable(Vector3 basePos, Vector3 terrainPos, float x, Vector3 offsetDir, float quadrMagnitude)
        {
            return Vector3.Lerp(basePos, terrainPos, x) + EvaluateQuadraticOffset(offsetDir, quadrMagnitude, x);
        }

        private Vector3 EvaluateQuadraticOffset(Vector3 dir, float mag, float x)
        {
            float localMagnitude = -mag * x * (x - 1);
            return dir * localMagnitude;
        }

        public struct CableSegment
        {
            public string classId;
            public Vector3 position;
            public Vector3 forward;

            public CableSegment(string classId, Vector3 position, Vector3 forward)
            {
                this.classId = classId;
                this.position = position;
                this.forward = forward;
            }
        }

        private static string GetMiddleCableRandom(int index)
        {
            int value = index % 3;
            if (value == 2)
            {
                return cables_mid01;
            }
            else if (value == 1)
            {
                return cables_mid02;
            }
            else
            {
                return cables_mid03;
            }
        }

        public IEnumerator GenerateAtmospheres(GameObject placeholderHolder, string parentName, string atmosVolClassId)
        {
            GameObject parent = GameObjectExtensions.SearchChild(placeholderHolder, parentName);
            foreach (Transform child in parent.transform)
            {
                yield return StartCoroutine(SpawnPrefabGlobally(atmosVolClassId, child.transform.position, child.transform.eulerAngles, child.transform.lossyScale));
            }
        }
    }
}