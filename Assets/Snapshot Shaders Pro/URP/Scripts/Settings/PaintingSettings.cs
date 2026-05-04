namespace SnapshotShaders.URP
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [System.Serializable, VolumeComponentMenu("Snapshot Shaders Pro/Painting"), DisplayInfo(name = "Painting")]
    public sealed class PaintingSettings : VolumeComponent, IPostProcessComponent
    {
#if !UNITY_6000_3_OR_NEWER
        public PaintingSettings()
        {
            displayName = "Painting";
        }
#endif

        [Tooltip("Choose where to insert this pass in URP's render loop.")]
        public RenderPassEventParameter renderPassEvent = new RenderPassEventParameter(RenderPassEvent.BeforeRenderingPostProcessing);

        [Tooltip("Oil Painting effect radius.")]
        public ClampedIntParameter kernelSize = new ClampedIntParameter(1, 1, 51);

        public bool IsActive()
        {
            return kernelSize.value > 1 && active;
        }

        public bool IsTileCompatible()
        {
            return false;
        }
    }
}
