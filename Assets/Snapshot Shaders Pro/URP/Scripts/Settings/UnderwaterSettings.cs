namespace SnapshotShaders.URP
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [System.Serializable]
    [VolumeComponentMenu("Snapshot Shaders Pro/Underwater")]
    public sealed class UnderwaterSettings : VolumeComponent, IPostProcessComponent
    {
        public UnderwaterSettings()
        {
            displayName = "Underwater";
        }

        [Tooltip("Choose where to insert this pass in URP's render loop.")]
        public RenderPassEventParameter renderPassEvent = new(RenderPassEvent.BeforeRenderingPostProcessing);

        [Tooltip("Displacement texture for surface waves.")]
        public TextureParameter bumpMap = new(null);

        [Range(0.0f, 10.0f)] [Tooltip("Strength/size of the waves.")]
        public ClampedFloatParameter strength = new(0.0f, 0.0f, 10.0f);

        [Tooltip("Tint of the underwater fog.")]
        public ColorParameter waterFogColor = new(Color.white);

        [Range(0.0f, 1.0f)] [Tooltip("Strength of the underwater fog.")]
        public ClampedFloatParameter fogStrength = new(0.0f, 0.0f, 1.0f);

        [Tooltip("")] public BoolParameter useCaustics = new(false);

        [Tooltip("")] public TextureParameter causticsTexture = new(null);

        public ClampedFloatParameter causticsNoiseSpeed = new(1.0f, 0.0f, 10.0f);

        public ClampedFloatParameter causticsNoiseScale = new(1.0f, 0.0f, 10.0f);

        public ClampedFloatParameter causticsNoiseStrength = new(1.0f, 0.0f, 1.0f);

        public Vector3Parameter causticsScrollVelocity1 = new(new Vector3(0.75f, 0.25f, 0.0f));

        public Vector3Parameter causticsScrollVelocity2 = new(new Vector3(0.75f, 0.25f, 0.0f));

        public Vector2Parameter causticsTiling = new(Vector2.one);

        public ColorParameter causticsTint = new(Color.white, true, true, true);

        public bool IsActive()
        {
            return (strength.value > 0.0f || fogStrength.value > 0.0f) && active;
        }

        public bool IsTileCompatible()
        {
            return false;
        }
    }
}