namespace SnapshotShaders.URP
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [System.Serializable]
    [VolumeComponentMenu("Snapshot Shaders Pro/World Scan")]
    public sealed class WorldScanSettings : VolumeComponent, IPostProcessComponent
    {
        public WorldScanSettings()
        {
            displayName = "World Scan";
        }

        [Tooltip("Choose where to insert this pass in URP's render loop.")]
        public RenderPassEventParameter renderPassEvent = new(RenderPassEvent.BeforeRenderingPostProcessing);

        [Tooltip("The world space origin point of the scan.")]
        public NoInterpVector3Parameter scanOrigin = new(Vector3.zero);

        [Tooltip("How far, in Unity units, the scan has travelled from the origin.")]
        public NoInterpFloatParameter scanDistance = new(0.0f);

        [Tooltip("The distance, in Unity units, the scan texture gets applied over.")]
        public NoInterpFloatParameter scanWidth = new(1.0f);

        [Tooltip("An x-by-1 ramp texture representing the scan color.")]
        public TextureParameter overlayRampTex = new(null);

        [Tooltip("An additional HDR color tint applied to the scan.")]
        public ColorParameter overlayColor = new(Color.white, true, true, true);

        public bool IsActive()
        {
            return overlayRampTex.value != null && active;
        }

        public bool IsTileCompatible()
        {
            return true;
        }
    }
}