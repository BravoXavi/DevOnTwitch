using UnityEngine;

public interface IBarkable
{
    void OnBarked(Transform barkerTransform, float barkStrength);
}
