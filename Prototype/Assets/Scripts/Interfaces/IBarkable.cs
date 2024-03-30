using UnityEngine;

namespace Behaviors
{
    public interface IBarkable
    {
        void OnBarked(Transform barkerTransform, float barkStrength);
    }
}