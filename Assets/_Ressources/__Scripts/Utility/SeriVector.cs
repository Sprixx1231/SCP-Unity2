using System;
using UnityEngine;

namespace Game.Utility

{
    [System.Serializable]
        public class SeriVector
        {
            public float x, y, z;

            public SeriVector(float _x, float _y, float _z)
            {
                x = _x;
                y = _y;
                z = _z;

            }

            public static SeriVector FromVector3(Vector3 og)
            {
                return new SeriVector(og.x, og.y, og.z);
            }

            public Vector3 ToVector3()
            {
                return (new Vector3(x, y, z));
            }
        }
}