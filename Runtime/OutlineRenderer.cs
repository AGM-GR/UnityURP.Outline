using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityURP.Outline
{
    [ExecuteAlways]
    public class OutlineRenderer : MonoBehaviour
    {
        [SerializeField] 
        private OutlineMaterial _outlineMaterial;

        [SerializeField] 
        private List<Renderer> _outlineRenderers;

#if UNITY_EDITOR
        private OutlineMaterial _oldOutlineMaterial;
#endif

        private void OnValidate()
        {
            if (_outlineRenderers == null || _outlineRenderers.Count == 0)
                FillRenderersNoParticleSystems();
            
            if (this.enabled && gameObject.activeInHierarchy)
                RefreshOutline();

            if (_oldOutlineMaterial != null && _oldOutlineMaterial != _outlineMaterial)
            {
                BlurredBufferOutlineRendererFeature.RemoveRenderers(_outlineRenderers, _oldOutlineMaterial);
            }
            _oldOutlineMaterial = _outlineMaterial;
        }

        [ContextMenu("Reassign Renderers")]
        public void FillAllRenderers()
        {
            _outlineRenderers = GetComponentsInChildren<Renderer>(true).ToList();
        }

        [ContextMenu("Reassign Renderers without Particles")]
        public void FillRenderersNoParticleSystems()
        {
            var allRenderers = GetComponentsInChildren<Renderer>(true);
            _outlineRenderers = new List<Renderer>(allRenderers.Length);

            foreach (var renderer in allRenderers)
            {
                if (!(renderer is ParticleSystemRenderer))
                {
                    _outlineRenderers.Add(renderer);
                }
            }
        }

        private void OnEnable()
        {
            BlurredBufferOutlineRendererFeature.AddRenderers(_outlineRenderers, _outlineMaterial);
        }

        private void OnDisable()
        {
            BlurredBufferOutlineRendererFeature.RemoveRenderers(_outlineRenderers, _outlineMaterial);
        }

        public void UpdateRenderers(List<Renderer> newRenderers)
        {
            BlurredBufferOutlineRendererFeature.RemoveRenderers(_outlineRenderers, _outlineMaterial);
            _outlineRenderers = newRenderers;
            BlurredBufferOutlineRendererFeature.AddRenderers(_outlineRenderers, _outlineMaterial);
        }

        public void ChangeOutlineMaterial(OutlineMaterial outlineMaterial)
        {
            BlurredBufferOutlineRendererFeature.RemoveRenderers(_outlineRenderers, _outlineMaterial);
            _outlineMaterial = outlineMaterial;
            BlurredBufferOutlineRendererFeature.AddRenderers(_outlineRenderers, _outlineMaterial);
        }

        public void RefreshOutline()
        {
            BlurredBufferOutlineRendererFeature.RemoveRenderers(_outlineRenderers, _outlineMaterial);
            BlurredBufferOutlineRendererFeature.AddRenderers(_outlineRenderers, _outlineMaterial);
        }
    }
}