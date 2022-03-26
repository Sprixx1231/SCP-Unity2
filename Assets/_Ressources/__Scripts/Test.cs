using UnityEngine;
using Game.Interactable;
public class Test : Interactable
{
    public override void OnInteract()
    {
        print("interacted with "+ gameObject.name);
    }

    public override void OnFocus()
    { 
        print("looking at " + gameObject.name);
    }

    public override void OnLoseFocus()
    { 
        print("stopped looking at " + gameObject.name);
    }
}
