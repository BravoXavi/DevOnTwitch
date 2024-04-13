using Behaviors;
using Global.Layers;
using UnityEngine;

namespace Character.Controllers
{
    public class BarkModule : MonoBehaviour
    {
        [SerializeField] private float _barkRadius = 3.0f;
        [SerializeField] private float _barkStrength = 200.0f;

        public void TriggerBark()
        {
            Bark();
        }
        
        private void Bark()
        {
            var detectedTargets = Physics2D.OverlapCircleAll(transform.position, _barkRadius);
            foreach (var target in detectedTargets)
            {
                if (!LayerUtils.IsInLayer(target.gameObject, LayerUtils.Strings.BARKABLE))
                {
                    continue;
                }

                var barkableItem = target.gameObject.GetComponent<IBarkable>();
                barkableItem?.OnBarked(transform, _barkStrength);
            }
        }
        
        #region DEBUG
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _barkRadius);
        }
        
        #endregion
    }
}