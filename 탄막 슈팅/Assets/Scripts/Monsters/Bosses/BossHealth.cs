using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonsterHealth {

    protected override void Die()
    {
        TestData.Save<bool>(true, TestData.DeathPath(name));
        gameObject.SetActive(false);
        PathController.Instance.Free(GetComponent<BossBrain>().tilesToShut);
    }

}
