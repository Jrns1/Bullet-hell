using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : InteractiveObject {

    public SavePointData savePoint;

    protected override void Interact()
    {
        Debug.Log("Save");
        GameManager.Instance.savePoint = savePoint;
    }

}
