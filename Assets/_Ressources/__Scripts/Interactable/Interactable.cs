using System;
using UnityEngine;

namespace Game.Interactable
{
    public abstract class Interactable : MonoBehaviour
    {
        public virtual void Awake()
        {
            gameObject.layer = 7;
        }

        public abstract void OnInteract();
        public abstract void OnFocus();
        public abstract void OnLoseFocus();
    }

}
