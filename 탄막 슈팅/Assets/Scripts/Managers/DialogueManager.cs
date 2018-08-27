using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class DialogueManager : Singleton<DialogueManager> {

    public TMPro.TextMeshProUGUI text;
    public float delay;
    public string[] lines;

    bool isTalking = false;

    StringBuilder sb;
    Animator textAnimator;

    private void Awake()
    {
        sb = new StringBuilder();
        textAnimator = text.transform.parent.GetComponent<Animator>();
    }

    public IEnumerator StartDialogue()
    {
        if (isTalking)
            yield break;
        textAnimator.enabled = true;
        isTalking = true;
        GameManager.Instance.player.GetComponent<PlayerMovement>().isMoving = false;
        textAnimator.SetBool("isTalking", true);

        for (int line = 0; line < lines.Length; line++)
        {
            for (int ch = 0; ch < lines[line].Length; ch++)
            {
                sb.Append(lines[line][ch]);
                text.text = sb.ToString();
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.E)));
            sb = new StringBuilder();
        }

        isTalking = false;
        GameManager.Instance.player.GetComponent<PlayerMovement>().isMoving = true;
        textAnimator.SetBool("isTalking", false);
    }

}
