using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPendent
{
    void Skill();
}

public class PendantManager : MonoBehaviour
{
    public GameObject[] pendentPrefabs;

    public IPendent pendent;

    const float holdTime = 1.3f;

    SpriteRenderer[] pendentArray;
    PlayerMovement movement;
    Coroutine holding;


    private void Awake()
    {
        bool[] isUnlocked;
        DataManager.Ins.Load<bool[]>("pendent", out isUnlocked);

        for (int i = 0; i < isUnlocked.Length; i++)
        {
            if (isUnlocked[i])
                Instantiate(pendentPrefabs[i], transform);
        }

        pendentArray = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            holding = GameManager.Ins.Delay(
                () => StartCoroutine(SkillSelection()),
                holdTime);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            if (holding != null)
            {
                StopCoroutine(holding);
                pendent.Skill();
            }
        }
    }

    IEnumerator SkillSelection()
    {
        holding = null;
        GameManager.Ins.isActionInhibited = true;

        foreach (SpriteRenderer pendent in pendentArray)
        {
            Fader.Ins.Fade(pendent, .1f, 0, 1);
        }
        yield return Fader.Ins.FadeCo((x) => transform.localScale = new Vector2(x, x), .1f, .7f, 1f);

        float desiredRotation = transform.eulerAngles.z;
        float degreePerRotate = 360f / pendentArray.Length;
        float velocity = 0f;
        while (Input.GetKey(KeyCode.Space) || velocity > .1f)
        {
            desiredRotation -= Input.GetKeyDown(KeyCode.A) ? degreePerRotate : 0;
            desiredRotation += Input.GetKeyDown(KeyCode.D) ? degreePerRotate : 0;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.SmoothDampAngle(transform.eulerAngles.z, desiredRotation, ref velocity, .1f));

            yield return null;
        }

        pendent = transform.GetChild(Mathf.RoundToInt(transform.eulerAngles.z) / (int)degreePerRotate).GetComponent<IPendent>();

        GameManager.Ins.isActionInhibited = false;

        foreach (SpriteRenderer pendent in pendentArray)
        {
            Fader.Ins.Fade(pendent, .2f, 1, 0);
        }
    }

}
