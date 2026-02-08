namespace SnapshotShaders.URP
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [System.Serializable]
    [VolumeComponentMenu("Snapshot Shaders Pro/Glitch")]
    public class GlitchSettings : VolumeComponent, IPostProcessComponent
    {
        public GlitchSettings()
        {
            displayName = "Glitch";
        }

        [Tooltip("Choose where to insert this pass in URP's render loop.")]
        public RenderPassEventParameter renderPassEvent = new(RenderPassEvent.BeforeRenderingPostProcessing);

        [Tooltip("Is the effect active?")] public BoolParameter enabled = new(false);

        [Tooltip("Texture which controls the strength of the glitch offset based on y-coordinate.")]
        public TextureParameter offsetTexture = new(null);

        [Tooltip("Glitch effect intensity.")] public ClampedFloatParameter offsetStrength = new(0.1f, 0f, 5.0f);

        [Tooltip("Controls how many times the glitch texture repeats vertically.")]
        public ClampedFloatParameter verticalTiling = new(5.0f, 0.0f, 25.0f);

        public bool IsActive()
        {
            return enabled.value && active;
        }

        public bool IsTileCompatible()
        {
            return false;
        }
    }
}