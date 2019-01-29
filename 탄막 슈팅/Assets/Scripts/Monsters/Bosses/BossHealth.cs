using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonsterHealth {

    protected override void Die()
    {
        DataManager.Ins.Save<bool>(true, name);
        gameObject.SetActive(false);
        PathController.Ins.Free(GetComponent<BossBrain>().tilesToShut);
    }

}
