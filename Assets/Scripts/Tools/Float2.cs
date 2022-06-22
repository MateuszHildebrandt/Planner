using System;
using UnityEngine;

namespace Tools
{
    [Serializable]
    public struct Float2
    {
        public float x, y;

        public Float2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Float2(Vector3 vector3)
        {
            return new Float2(vector3.x, vector3.y);
        }

        public static implicit operator Vector3(Float2 float2)
        {
            return new Vector3(float2.x, float2.y, 0);
        }
    }
}
