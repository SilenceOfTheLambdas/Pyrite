namespace SnapshotShaders.URP
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [System.Serializable]
    [VolumeComponentMenu("Snapshot Shaders Pro/Basic Dither")]
    public sealed class BasicDitherSettings : VolumeComponent, IPostProcessComponent
    {
        public BasicDitherSettings()
        {
            displayName = "Basic Dither";
        }

        [Tooltip("Choose where to insert this pass in URP's render loop.")]
        public RenderPassEventParameter renderPassEvent = new(RenderPassEvent.BeforeRenderingPostProcessing);

        [Tooltip("Is the effect active?")] public BoolParameter enabled = new(false);

        [Tooltip("Noise texture to use for dither thresholding.")]
        public TextureParameter noiseTex = new(null);

        [Tooltip("Size of the noise texture.")]
        public ClampedFloatParameter noiseSize = new(1.0f, 0.1f, 100.0f);

        [Tooltip("Offset used when calculating luminance threshold.")]
        public ClampedFloatParameter thresholdOffset = new(0.0f, -0.5f, 0.5f);

        [Tooltip("Color to use for dark sections of the image.")]
        public ColorParameter darkColor = new(Color.black);

        [Tooltip("Color to use for light sections of the image.")]
        public ColorParameter lightColor = new(Color.white);

        [Tooltip("Use the Scene Color instead of Light Color?")]
        public BoolParameter useSceneColor = new(false);

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