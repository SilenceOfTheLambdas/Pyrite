namespace SnapshotShaders.URP
{
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    [VolumeComponentMenu("Snapshot Shaders Pro/Height Fog")]
    public sealed class HeightFogSettings : VolumeComponent, IPostProcessComponent
    {
        public HeightFogSettings()
        {
            displayName = "Height Fog";
        }

        [Tooltip("Choose where to insert this pass in URP's render loop.")]
        public RenderPassEventParameter renderPassEvent = new(RenderPassEvent.AfterRenderingOpaques);

        [Tooltip("How strongly to mix fog colors and original scene colors.")]
        public ClampedFloatParameter strength = new(0.0f, 0.0f, 1.0f);

        [Tooltip("Distance away from the camera at which distance fog starts appearing.")]
        public FloatParameter startDistance = new(20.0f);

        [ColorUsage(false, true)] [Tooltip("Color of the distance fog at the start distance.")]
        public ColorParameter distanceStartColor = new(new Color(0.514151f, 0.6437106f, 1.0f));

        [Tooltip("Distance away from the camera at which distance fog is at full strength.")]
        public FloatParameter endDistance = new(50.0f);

        [ColorUsage(false, true)] [Tooltip("Color of the distance fog at the end distance and beyond.")]
        public ColorParameter distanceEndColor = new(new Color(0.1745283f, 0.3942103f, 1.0f));

        [Tooltip("Controls the color curve at distances between start and end.")]
        public ClampedFloatParameter distanceFalloff = new(1.0f, 0.01f, 10.0f);

        [Tooltip(
            "Height in world space at which height fog starts appearing.\n\nNote: this value should be higher than End Height.")]
        public FloatParameter startHeight = new(5.0f);

        [ColorUsage(false, true)] [Tooltip("Color of the height fog at the start height.")]
        public ColorParameter heightStartColor = new(new Color(0.8313726f, 0.3137255f, 0.2235294f));

        [Tooltip(
            "Height in world space at which height fog is at full strength.\n\nNote: this value should be lower than Start Height.")]
        public FloatParameter endHeight = new(-5.0f);

        [ColorUsage(false, true)] [Tooltip("Color of the height fog at the end height and below.")]
        public ColorParameter heightEndColor = new(new Color(0.3294118f, 0.1254902f, 0.1254902f));

        [Tooltip("Controls the color curve at height values between start and end.")]
        public ClampedFloatParameter heightFalloff = new(1.0f, 0.01f, 10.0f);

        public bool IsActive()
        {
            return strength.value > 0.0f && active;
        }

        public bool IsTileCompatible()
        {
            return false;
        }
    }
}