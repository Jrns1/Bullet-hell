using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour {


    protected virtual void OnEnable()
    {
        Interactor.interactants.Add(transform);
    }

    protected virtual void OnDisable()
    {
        Interactor.interactants.Remove(transform);
    }

    public virtual void Interact()
    {
        GameManager.Ins.isInteractionAllowed = false;
        Interactor.interactants.Remove(transform);
    }

    protected void CompeleteInteraction()
    {
        GameManager.Ins.isInteractionAllowed = true;
        GameManager.Ins.Delay(() => Interactor.interactants.Add(transform), 2);
    }
}
