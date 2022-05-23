//Author: Simon Canbäck, sica4801
using UnityEngine;

namespace Utility
{
    public static class LayerMaskExtensions
    {
        public static bool IsInLayerMask(GameObject go, LayerMask mask)
        {
            //found online. works by moving 1 to layer's spot in the mask, then using bitwise OR
            //if that flips a 0 to a 1, that means it's not in the mask

            return (mask == (mask | (1 << go.layer))); 
        }
    }
}