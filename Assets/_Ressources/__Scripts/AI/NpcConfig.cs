using Unity.VisualScripting.FullSerializer;
using UnityEngine;

namespace Game.AI
{
    [CreateAssetMenu(/*fileName = "FILENAME", menuName = "MENUNAME", order = 0*/)]
    public class NpcConfig : ScriptableObject
    {
         public float MaxTime = 1.0f;
         public float MaxDistance = 1.0f;
         public float DieForce = 10.0f;
         public float MaxSightDistance = 5.0f;
    }
}