using Behaviors;
using Character.Controllers;
using UnityEngine;

public class BasicFurniture : MonoBehaviour
{
    [SerializeField] private HPModule _hpModule;
    
    private void Start()
    {
        if (_hpModule != null)
        {
            _hpModule.OnDead.AddListener(OnBroken);
            _hpModule.OnDamageReceivedEvent.AddListener(ThrowDamagedAnim);
        }
    }

    private void OnBroken()
    {
        Destroy(gameObject);
    }

    private void ThrowDamagedAnim()
    {
        //Throw particles
    }
}
