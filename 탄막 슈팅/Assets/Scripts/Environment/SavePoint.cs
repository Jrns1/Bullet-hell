using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : InteractiveObject {

    public SavePointData savePoint;

    public override void Interact()
    {
        base.Interact();

        Debug.Log("Save");
        GameManager.Ins.savePoint = savePoint;

        CompeleteInteraction();
    }

}
