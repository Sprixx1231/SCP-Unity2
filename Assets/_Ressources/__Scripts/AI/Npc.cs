using System;
using UnityEngine;
using Game.Utility;

namespace Game.AI
{
    [System.Serializable]
    public class NpcData
    {
        public SeriVector Pos, Target, Rotation;
        //public npctype type;
        //public npcstate state= npcstate.alive;
        public bool isActive=false;
        public int[] npcvalue = new int[5];
    }

    public class Npc : MonoBehaviour
    {
        [HideInInspector]
        public NpcData data;
        
        public virtual void CreateData()
        {
            var position = transform.position;
            data.Pos = new SeriVector(position.x, position.y, position.z);
        }
    }
}