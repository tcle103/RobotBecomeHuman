using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorblindFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class ColorblindSettings
    {
        [Tooltip("Hidden/Wilberforce/Colorblind")]
        public Shader shader;

        [Tooltip("0=Normal, 1=Protanopia, 2=Deuteranopia, 3=Tritanopia")]
        public int Type = 0;
    }

    public ColorblindSettings settings = new ColorblindSettings();

    class ColorblindPass : ScriptableRenderPass
    {
        readonly string _profilerTag = "ColorblindPass";
        Material _material;
        int _typePropID;
        int _tempRT;

        RenderTargetIdentifier _cameraTarget;
        int _type;

        public ColorblindPass(Material mat)
        {
            _material = mat;
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
            _typePropID = Shader.PropertyToID("type");
            _tempRT = Shader.PropertyToID("_TempColorblindRT");
        }

        public void Setup(RenderTargetIdentifier cameraTarget, int type)
        {
            _cameraTarget = cameraTarget;
            _type = type;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (_material == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get(_profilerTag);
            _material.SetInt(_typePropID, _type);

            var desc = renderingData.cameraData.cameraTargetDescriptor;
            cmd.GetTemporaryRT(_tempRT, desc, FilterMode.Bilinear);
            cmd.Blit(_cameraTarget, _tempRT);
            
            cmd.Blit(_tempRT, _cameraTarget, _material);
            cmd.ReleaseTemporaryRT(_tempRT);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    ColorblindPass _pass;
    Material _material;

    public override void Create()
    {
        if (settings.shader == null)
        {
            Debug.LogError("[ColorblindFeature] No shader assigned!");
            return;
        }

        _material = CoreUtils.CreateEngineMaterial(settings.shader);
        _pass = new ColorblindPass(_material);
    }
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (_material == null)
            return;

        renderer.EnqueuePass(_pass);
    }
    
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (_material == null)
            return;

        _pass.Setup(renderer.cameraColorTarget, settings.Type);
    }
    
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        CoreUtils.Destroy(_material);
    }
}