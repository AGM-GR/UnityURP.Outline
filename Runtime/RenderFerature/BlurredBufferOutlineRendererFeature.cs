using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace UnityURP.Outline
{
    public class BlurredBufferOutlineRendererFeature : ScriptableRendererFeature
    {
        private const string DilationShaderName = "Hidden/Dilation";
        private const string OutlineShaderName = "Hidden/Outline Color And Stencil";

        [SerializeField] private RenderPassEvent renderEvent = RenderPassEvent.AfterRenderingTransparents;
        [Space]
        [SerializeField] private Shader dilationShader;
        [SerializeField] private Shader outlineShader;
        [Space]
        [SerializeField, Range(1, 60)] private int spread = 10;
        [SerializeField] private Color outlineColor = Color.cyan;

        private Material dilationMaterial;
        private Material outlineMaterial;
        private BlurredBufferOutlinePass _outlinePass;

        private HashSet<Renderer> _targetRenderers;

        public void AddRenderers(List<Renderer> targetRenderers)
        {
            if (_targetRenderers == null)
                _targetRenderers = new HashSet<Renderer>();

            foreach (Renderer renderer in targetRenderers)
                _targetRenderers.Add(renderer);

            if (_outlinePass != null)
                _outlinePass.Renderers = _targetRenderers.ToArray();
        }

        public void RemoveRenderers(List<Renderer> targetRenderers)
        {
            if (_targetRenderers != null)
            {
                foreach (Renderer renderer in targetRenderers)
                    _targetRenderers.Remove(renderer);

                if (_outlinePass != null)
                    _outlinePass.Renderers = _targetRenderers.ToArray();
            }
        }

        public override void Create()
        {
            name = "Outliner";

            // Pass in constructor variables which don't/shouldn't need to be updated every frame.
            _outlinePass = new BlurredBufferOutlinePass();

            if (dilationShader == null)
                dilationShader = Shader.Find(DilationShaderName);
            if (outlineShader == null)
                outlineShader = Shader.Find(OutlineShaderName);

            if (dilationMaterial == null)
                dilationMaterial = RenderFeatureUtils.CreateMaterial(dilationShader);
            if (outlineMaterial == null)
                outlineMaterial = RenderFeatureUtils.CreateMaterial(outlineShader);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RenderFeatureUtils.Destroy(dilationMaterial);
                RenderFeatureUtils.Destroy(outlineMaterial);
            }
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (_outlinePass == null)
                return;

            if (!dilationMaterial ||
                !outlineMaterial ||
                _targetRenderers == null ||
                _targetRenderers.Count == 0)
            {
                // Don't render the effect if there's nothing to render
                return;
            }

            // Any variables you may want to update every frame should be set here.
            _outlinePass.RenderEvent = renderEvent;
            _outlinePass.DilationMaterial = dilationMaterial;
            dilationMaterial.SetInteger("_Spread", spread);
            _outlinePass.OutlineMaterial = outlineMaterial;
            outlineMaterial.SetColor("_BaseColor", outlineColor);
            _outlinePass.Renderers = _targetRenderers.ToArray();

            renderer.EnqueuePass(_outlinePass);
        }
    }
}