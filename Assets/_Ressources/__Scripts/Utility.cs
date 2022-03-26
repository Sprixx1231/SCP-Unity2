using UnityEngine;

namespace Utility
{
    public class Utility : MonoBehaviour
    {
        public static Vector3 GetRandomDirection()
        {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }
}