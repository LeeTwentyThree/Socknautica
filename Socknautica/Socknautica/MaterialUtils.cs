using System.Collections;
using UnityEngine;
using UWE;

namespace Socknautica;

/// <summary>
/// a Utility Class for Enhancing Materials and Shaders.
/// </summary>
public static class MaterialUtils
{
    internal static void LoadMaterials()
    {
        CoroutineHost.StartCoroutine(LoadIonCubeMaterial());
        CoroutineHost.StartCoroutine(LoadPrecursorGlassAndFogMaterial());
        CoroutineHost.StartCoroutine(LoadStasisFieldMaterial());
        CoroutineHost.StartCoroutine(LoadAirWaterBarrierMaterial());
        CoroutineHost.StartCoroutine(LoadForcefieldMaterial());
        CoroutineHost.StartCoroutine(LoadPrecursorElevatorMaterial());
        CoroutineHost.StartCoroutine(LoadAuroraRockMaterial());
    }

    /// <summary>
    /// Gets the Ion Cube's Material.
    /// </summary>
    public static Material IonCubeMaterial { get; private set; }

    /// <summary>
    /// Gets the Precursor Glass' Material.
    /// </summary>
    public static Material PrecursorGlassMaterial { get; private set; }

    /// <summary>
    /// Gets the Stasis Rifle's ball Material.
    /// </summary>
    public static Material StasisFieldMaterial { get; private set; }

    /// <summary>
    /// Gets the Precursor Force Field's Material.
    /// </summary>
    public static Material ForceFieldMaterial { get; private set; }

    /// <summary>
    /// Gets the Precursor Elevator VFX's Material.
    /// </summary>
    public static Material PrecursorElevatorMaterial { get; private set; }

    /// <summary>
    /// Gets the Material used in Alien Bases for the transition between water and air.
    /// </summary>
    public static Material AirWaterBarrierMaterial { get; private set; }

    /// <summary>
    /// Gets the triplanar Material used in rocks around the Aurora.
    /// </summary>
    public static Material AuroraRockMaterial { get; private set; }

    /// <summary>
    /// Gets the generic black Material to be used for stacked fading fog planes. Used in RotA's Voidbase.
    /// </summary>
    public static Material FogMaterial { get; private set; }

    private static GameObject _precursorInteriorSky;

    /// <summary>
    /// Returns the Interior sky that is used by the vanilla Precursor bases.
    /// </summary>
    public static GameObject PrecursorInteriorSky
    {
        get
        {
            if (!_precursorInteriorSky)
            {
                PrefabDatabase.TryGetPrefab("4a5670a3-1459-45b9-81b4-44ecc7af5996", out var obj);
                _precursorInteriorSky = obj.GetComponent<SkyApplier>().customSkyPrefab;
            }

            return _precursorInteriorSky;
        }
    }

    private static GameObject _precursorExteriorSky;
            
    /// <summary>
    /// Returns the Exterior sky that is used by the vanilla Precursor bases.
    /// </summary>
    public static GameObject PrecursorExteriorSky
    {
        get
        {
            if (!_precursorExteriorSky)
            {
                PrefabDatabase.TryGetPrefab("812c84dd-0175-4fbc-95d8-ccc397c6ca91", out var obj);
                _precursorExteriorSky = obj.GetComponent<SkyApplier>().customSkyPrefab;
            }

            return _precursorExteriorSky;
        }
    }

    private static GameObject _precursorPrisonAntechamberSky;
        
    /// <summary>
    /// Returns the vanilla Prison Antechamber sky.
    /// </summary>
    public static GameObject PrecursorAntechamberSky
    {
        get
        {
            if (!_precursorPrisonAntechamberSky)
            {
                _precursorPrisonAntechamberSky = WaterBiomeManager.main.biomeSkies[131].gameObject;
            }

            return _precursorPrisonAntechamberSky;
        }
    }

    /// <summary>
    /// The <see cref="Shader"/> that is used for most prefabs in Subnautica.
    /// </summary>
    public static Shader MarmosetUber { get; private set; } = Shader.Find("MarmosetUBER");

    /// <summary>
    /// Applies the Necessary Subnautica's shader(MarmosetUBER) to the passed <see cref="GameObject"/>.
    /// </summary>
    /// <param name="prefab">the <see cref="GameObject"/> to apply the shaders to.</param>
    /// <param name="shininess">'_Shininess' value of the shader.</param>
    /// <param name="specularInt">'_SpecularInt' value of the shader.</param>
    /// <param name="glowStrength">'_GlowStrength' and '_GlowStrengthNight' value of the shader.</param>
    public static void ApplySNShaders(GameObject prefab, float shininess = 8f, float specularInt = 1f, float glowStrength = 1f)
    {
        var renderers = prefab.GetComponentsInChildren<Renderer>(true);
        var newShader = MarmosetUber;
        for (var i = 0; i < renderers.Length; i++)
        {
            if(renderers[i] is ParticleSystemRenderer)
            {
                continue;
            }
            for (var j = 0; j < renderers[i].materials.Length; j++)
            {
                var material = renderers[i].materials[j];
                var specularTexture = material.GetTexture("_SpecGlossMap");
                var emissionTexture = material.GetTexture("_EmissionMap");
                var emissionColor = material.GetColor(ShaderPropertyID._EmissionColor);
                material.shader = newShader;

                material.DisableKeyword("_SPECGLOSSMAP");
                material.DisableKeyword("_NORMALMAP");
                if (specularTexture != null)
                {
                    material.SetTexture("_SpecTex", specularTexture);
                    material.SetFloat("_SpecInt", specularInt);
                    material.SetFloat("_Shininess", shininess);
                    material.EnableKeyword("MARMO_SPECMAP");
                    material.SetColor("_SpecColor", new Color(1f, 1f, 1f, 1f));
                    material.SetFloat("_Fresnel", 0.24f);
                    material.SetVector("_SpecTex_ST", new Vector4(1.0f, 1.0f, 0.0f, 0.0f));
                }
                if (material.IsKeywordEnabled("_EMISSION"))
                {
                    material.EnableKeyword("MARMO_EMISSION");
                    material.SetFloat(ShaderPropertyID._EnableGlow, 1f);
                    material.SetTexture(ShaderPropertyID._Illum, emissionTexture);
                    material.SetColor(ShaderPropertyID._GlowColor, emissionColor);
                    material.SetFloat(ShaderPropertyID._GlowStrength, glowStrength);
                    material.SetFloat(ShaderPropertyID._GlowStrengthNight, glowStrength);
                }

                if (material.GetTexture("_BumpMap"))
                {
                    material.EnableKeyword("MARMO_NORMALMAP");
                }

                if(material.name.Contains("Cutout"))
                {
                    material.EnableKeyword("MARMO_ALPHA_CLIP");
                }
                if (material.name.Contains("Transparent"))
                {
                    material.EnableKeyword("_ZWRITE_ON");
                    material.EnableKeyword("WBOIT");
                    material.SetInt("_ZWrite", 0);
                    material.SetInt("_Cutoff", 0);
                    material.SetFloat("_SrcBlend", 1f);
                    material.SetFloat("_DstBlend", 1f);
                    material.SetFloat("_SrcBlend2", 0f);
                    material.SetFloat("_DstBlend2", 10f);
                    material.SetFloat("_AddSrcBlend", 1f);
                    material.SetFloat("_AddDstBlend", 1f);
                    material.SetFloat("_AddSrcBlend2", 0f);
                    material.SetFloat("_AddDstBlend2", 10f);
                    material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack | MaterialGlobalIlluminationFlags.RealtimeEmissive;
                    material.renderQueue = 3101;
                    material.enableInstancing = true;
                }
            }
        }
    }

    /// <summary>
    /// Applies the Necessary Precursor Materials to the passed <see cref="GameObject"/>. Automatically replaces any materials that contains the string "IonShader" with an ion cube material and any material that contains the string "transparent" with a precursor glass material.<br></br>
    /// Should be called AFTER <see cref="ApplySNShaders"/>.
    /// </summary>
    /// <param name="prefab">the <see cref="GameObject"/> to apply the Materials to.</param>
    /// <param name="specint">Specular Strength.</param>
    /// <param name="specularColor">The color of the glow.</param>
    /// <param name="fresnelStrength">The strength of the fresnel (higher values cause the object to only glow around the edges).</param>
    public static void ApplyPrecursorMaterials(GameObject prefab, float specint, PrecursorSpecularColor specularColor = PrecursorSpecularColor.Green, float fresnelStrength = 0.4f)
    {
        foreach (var renderer in prefab.GetComponentsInChildren<Renderer>(true))
        {
            if(renderer is ParticleSystemRenderer)
            {
                continue;
            }
            for (var i = 0; i < renderer.materials.Length; i++)
            {
                var material = renderer.materials[i];
                if (material.name.ToLower().Contains("ionshader"))
                {
                    var sharedMats = renderer.materials;
                    sharedMats[i] = IonCubeMaterial;
                    renderer.materials = sharedMats;
                    continue;
                }
                if (material.name.ToLower().Contains("transparent"))
                {
                    var sharedMats = renderer.materials;
                    sharedMats[i] = PrecursorGlassMaterial;
                    renderer.materials = sharedMats;
                    continue;
                }
                if (material.name.ToLower().Contains("fog"))
                {
                    var sharedMats = renderer.materials;
                    sharedMats[i] = FogMaterial;
                    renderer.materials = sharedMats;
                    continue;
                }
                material.SetColor("_SpecColor", GetSpecularColorForEnum(specularColor));
                material.SetFloat("_SpecInt", specint);
                material.SetFloat("_Fresnel", fresnelStrength);
            }
        }
    }

    /// <summary>
    /// Fixes any dark shading on any ion cube materials on or in the children of <paramref name="prefab"/> by updating the fakeSSS vector on the shader.
    /// </summary>
    /// <param name="prefab">The object to apply these changes to (includes children as well)</param>
    /// <param name="brightness">The brightness of the ion cube material.</param>
    public static void FixIonCubeMaterials(GameObject prefab, float brightness)
    {
        foreach (var renderer in prefab.GetComponentsInChildren<Renderer>(true))
        {
            if (renderer is ParticleSystemRenderer)
            {
                continue;
            }
            for (var i = 0; i < renderer.materials.Length; i++)
            {
                if (renderer.materials[i].name.ToLower().Contains("precursor_crystal_cube"))
                {
                    renderer.materials[i].SetVector("_FakeSSSparams", new Vector4(brightness, 0f, 0f, 0f));
                }
            }
        }
    }

    /// <summary>
    /// Defines specular colors that are commonly used in precursor materials.
    /// </summary>
    public enum PrecursorSpecularColor
    {
        /// <summary>
        /// Green specular. RGB: 0.25, 0.54, 0.41
        /// </summary>
        Green,
        /// <summary>
        /// Blue specular. RGB: 0.40, 0.69, 0.67
        /// </summary>
        Blue
    }

    private static Color GetSpecularColorForEnum(PrecursorSpecularColor color)
    {
        if(color == PrecursorSpecularColor.Green)
        {
            return precursorSpecularGreen;
        }
        else
        {
            return precursorSpecularBlue;
        }
    }

    private static Color precursorSpecularGreen = new Color(0.25f, 0.54f, 0.41f);
    private static Color precursorSpecularBlue = new Color(0.40f, 0.69f, 0.67f);

    private static IEnumerator LoadIonCubeMaterial()
    {
        if (IonCubeMaterial)
            yield break;

        PrefabDatabase.TryGetPrefab("41406e76-4b4c-4072-86f8-f5b8e6523b73", out var prefab);

        IonCubeMaterial = prefab.GetComponentInChildren<MeshRenderer>().material;
    }

    private static IEnumerator LoadAirWaterBarrierMaterial()
    {
        if (AirWaterBarrierMaterial)
            yield break;
        
        var task = PrefabDatabase.GetPrefabAsync("8b5e6a02-533c-44cb-9f34-d2773aa82dc4");
        yield return task;

        if (task.TryGetPrefab(out var prefab))
        {
            AirWaterBarrierMaterial = prefab.GetComponentInChildren<MeshRenderer>().material;
        }
    }

    private static IEnumerator LoadStasisFieldMaterial()
    {
        if (StasisFieldMaterial)
            yield break;
        
        var task = CraftData.GetPrefabForTechTypeAsync(TechType.StasisRifle);
        yield return task;

        var stasisRifle = task.GetResult();
        StasisFieldMaterial = stasisRifle.GetComponent<StasisRifle>().effectSpherePrefab.GetComponentInChildren<Renderer>().materials[1];
    }

    private static IEnumerator LoadForcefieldMaterial()
    {
        if (ForceFieldMaterial)
            yield break;

        var task = PrefabDatabase.GetPrefabAsync("b7ec7d50-186b-4656-9cc6-7dd503d14d98");
        yield return task;

        if (task.TryGetPrefab(out var prefab))
        {
            ForceFieldMaterial = prefab.GetComponentInChildren<Renderer>().material;
        }
    }

    private static IEnumerator LoadPrecursorElevatorMaterial()
    {
        if (PrecursorElevatorMaterial)
            yield break;

        var task = PrefabDatabase.GetPrefabAsync("51e58608-a80b-4135-9143-add4ce77a42f");
        yield return task;

        if (task.TryGetPrefab(out var prefab))
        {
            PrecursorElevatorMaterial = prefab.transform.Find("FX/x_Gun_Elevator_Tube").gameObject.GetComponentInChildren<Renderer>().material;
        }
    }

    private static IEnumerator LoadAuroraRockMaterial()
    {
        if (AuroraRockMaterial)
            yield break;

        var task = PrefabDatabase.GetPrefabAsync("8d13d081-431e-4ed5-bc99-2b8b9fabe9c2");
        yield return task;

        if (task.TryGetPrefab(out var prefab))
        {
            AuroraRockMaterial = prefab.GetComponentInChildren<Renderer>().material;
        }
    }

    private static IEnumerator LoadPrecursorGlassAndFogMaterial() // precursor glass AND fog material which derives from that
    {
        if (PrecursorGlassMaterial)
            yield break;
        
        var request = PrefabDatabase.GetPrefabAsync("2b43dcb7-93b6-4b21-bd76-c362800bedd1");
        yield return request;

        if (request.TryGetPrefab(out var glassPanel))
        {
            PrecursorGlassMaterial = new Material(glassPanel.GetComponentInChildren<MeshRenderer>().material);
            PrecursorGlassMaterial.SetColor("_Color", new Color(1f, 1f, 1f, 0.7f));
            PrecursorGlassMaterial.SetFloat("_SpecInt", 1f);

            FogMaterial = new Material(PrecursorGlassMaterial);
            FogMaterial.SetColor("_Color", new Color(0f, 0f, 0f, 0.2f));
            FogMaterial.SetColor("_GlowColor", new Color(0f, 0.24f, 0.23f, 1f));
            FogMaterial.SetTexture("_MainTex", null);
            FogMaterial.SetFloat("_SpecInt", 0f);
        }
    }
}