using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirearmManager : Singleton<FirearmManager>
{
    public TMPro.TextMeshProUGUI magazineText;
    public ReloadingUI reloading;

    float magazine;
    UIBase ui;

    private void Awake()
    {
        ui = magazineText.GetComponent<UIBase>();
        ui.renderer = magazineText;
        ui.SetValue = () => magazineText.text = magazine.ToString();
    }

    public void UpdateMagazine(int u)
    {
        magazine = u;
        ui.UpdateUI();
    }
}
