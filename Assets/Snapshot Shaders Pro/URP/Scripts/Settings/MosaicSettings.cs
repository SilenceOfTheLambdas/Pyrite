namespace SnapshotShaders.URP
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [System.Serializable]
    [VolumeComponentMenu("Snapshot Shaders Pro/Mosaic")]
    public sealed class MosaicSettings : VolumeComponent, IPostProcessComponent
    {
        public MosaicSettings()
        {
            displayName = "Mosaic";
        }

        [Tooltip("Choose where to insert this pass in URP's render loop.")]
        public RenderPassEventParameter renderPassEvent = new(RenderPassEvent.BeforeRenderingPostProcessing);

        [Tooltip("Is the effect active?")] public BoolParameter enabled = new(false);

        [Tooltip("Texture to overlay onto each mosaic tile.")]
        public TextureParameter overlayTexture = new(null);

        [Tooltip("Colour of texture overlay.")]
        public ColorParameter overlayColor = new(Color.white);

        [Range(5, 500)] [Tooltip("Number of tiles on the x-axis.")]
        public ClampedIntParameter xTileCount = new(100, 5, 500);

        [Tooltip("Use sharper point filtering when downsampling?")]
        public BoolParameter usePointFiltering = new(true);

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