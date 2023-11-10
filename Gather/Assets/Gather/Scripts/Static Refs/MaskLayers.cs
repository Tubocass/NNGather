using System.Collections;
using UnityEngine;

namespace gather
{
    public static class MaskLayers 
    {
        public static readonly LayerMask food = 1 << LayerMask.NameToLayer("Resources");
        public static readonly LayerMask units = 1 << LayerMask.NameToLayer("Default");
    }
}