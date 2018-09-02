using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonsterHealth {

    protected override void Die()
    {
        Data.Save<bool>(true, Data.DeathPath(name));
        gameObject.SetActive(false);
    }

}
