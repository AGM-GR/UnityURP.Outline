using System;
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
        
        private static Dictionary<OutlineMaterial, HashSet<Renderer>> _targetRenderers;
        private static bool _targetRenderersModifyed;

        [SerializeField] private RenderPassEvent renderEvent = RenderPassEvent.AfterRenderingTransparents;
        [Space]
        [SerializeField] private Shader _dilationShader;
        [SerializeField] private Shader _outlineShader;

        private Dictionary<OutlineMaterial, BlurredBufferOutlinePassData> _outlinePasses = new();

        

        [Serializable]
        protected class BlurredBufferOutlinePassData : IDisposable
        {
            public Material dilationMaterial;
            public Material outlineMaterial;
            public BlurredBufferOutlinePass outlinePass;

            public BlurredBufferOutlinePassData(Shader dilationShader, Shader outlineShader)
            {
                outlinePass = new BlurredBufferOutlinePass();
                dilationMaterial = RenderFeatureUtils.CreateMaterial(dilationShader);
                outlineMaterial = RenderFeatureUtils.CreateMaterial(outlineShader);
            }

            public void Dispose()
            {
                RenderFeatureUtils.Destroy(dilationMaterial);
                RenderFeatureUtils.Destroy(outlineMaterial);
            }
        }


        public static void AddRenderers(List<Renderer> targetRenderers, OutlineMaterial outlineMaterial)
        {
            if (outlineMaterial == null) return;

            if (_targetRenderers == null)
                _targetRenderers = new ();

            if (!_targetRenderers.ContainsKey(outlineMaterial))
                _targetRenderers.Add(outlineMaterial, new HashSet<Renderer>());

            foreach (Renderer renderer in targetRenderers)
                _targetRenderers[outlineMaterial].Add(renderer);

            _targetRenderersModifyed = true;
        }

        public static void RemoveRenderers(List<Renderer> targetRenderers, OutlineMaterial outlineMaterial)
        {
            if (_targetRenderers != null)
            {
                if (!_targetRenderers.ContainsKey(outlineMaterial)) return;

                foreach (Renderer renderer in targetRenderers)
                    _targetRenderers[outlineMaterial].Remove(renderer);

                _targetRenderersModifyed = true;
            }
        }

        public override void Create()
        {
            name = "Outliner";

            // Pass in constructor variables which don't/shouldn't need to be updated every frame.
            if (_dilationShader == null)
                _dilationShader = Shader.Find(DilationShaderName);
            if (_outlineShader == null)
                _outlineShader = Shader.Find(OutlineShaderName);
        }

        protected override void Dispose(bool disposing)
        {   
            if (disposing)
            {
                foreach (BlurredBufferOutlinePassData passData in _outlinePasses.Values)
                {
                    passData.Dispose();
                }

                _outlinePasses.Clear();
            }
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (_targetRenderers == null ||
                _targetRenderers.Count == 0)
            {
                // Don't render the effect if there's nothing to render
                return;
            }

            foreach (OutlineMaterial outlineMaterial in _targetRenderers.Keys)
            {
                if (!_outlinePasses.ContainsKey(outlineMaterial))
                {
                    _outlinePasses.Add(outlineMaterial, new BlurredBufferOutlinePassData(_dilationShader, _outlineShader));
                }
            }

            if (_outlinePasses == null || _outlinePasses.Count <= 0)
                return;


            // Any variables you may want to update every frame should be set here.
            foreach (OutlineMaterial material in _outlinePasses.Keys)
            {
                _outlinePasses[material].outlinePass.RenderEvent = renderEvent;
                _outlinePasses[material].outlinePass.DilationMaterial = _outlinePasses[material].dilationMaterial;
                _outlinePasses[material].dilationMaterial.SetInteger("_Spread", material.Spread);
                _outlinePasses[material].outlinePass.OutlineMaterial = _outlinePasses[material].outlineMaterial;
                _outlinePasses[material].outlineMaterial.SetColor("_BaseColor", material.OutlineColor);

                _outlinePasses[material].outlinePass.Renderers = _targetRenderers[material].ToArray();

                renderer.EnqueuePass(_outlinePasses[material].outlinePass);
            }
        }
    }
}