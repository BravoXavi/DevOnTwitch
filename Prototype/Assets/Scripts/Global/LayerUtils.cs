using UnityEngine;

namespace Global.Layers
{
    public static class LayerUtils
    {
        public static class Strings
        {
            public static readonly string GROUND = "Ground";
            public static readonly string BARKABLE = "Barkable";
        }
        
        public static bool IsInLayer(GameObject go, string layer) => 
            go.layer == LayerMask.NameToLayer(layer);
    }
}