using Game.Player;
using UnityEngine;

namespace Game.Interactable
{
    public class Door : Interactable
    {
        private bool _isOpen = false;
        private bool _canBeInteractedWith = true;
        private Animator _anim;

        private void Start()
        {
            _anim = GetComponent<Animator>();
        }

        public override void OnInteract()
        {
            if (_canBeInteractedWith)
            {
                _isOpen = !_isOpen;
                Vector3 doorTransformDirection = transform.TransformDirection(Vector3.forward);
                Vector3 playerTransformDirection = FPSController.Instance.transform.position - transform.position;
                float dot = Vector3.Dot(doorTransformDirection, playerTransformDirection);
            
                _anim.SetFloat("dot", dot);
                _anim.SetBool("isOpen", _isOpen);
            }
        }

        public override void OnFocus()
        {
            // throw new System.NotImplementedException();
        }

        public override void OnLoseFocus()
        {
            //throw new System.NotImplementedException();
        }
    }
}

