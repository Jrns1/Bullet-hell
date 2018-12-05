using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SpeechBubble : Singleton<SpeechBubble> {

    public float delay;
    public Line[] lines;
    public List<Transform> speakers;

    bool isTalking = false;
    TMPro.TextMeshProUGUI speakerBox;
    TMPro.TextMeshProUGUI textBox;

    StringBuilder sb;
    Animator textAnimator;
    CameraController cam;

    private void Awake()
    {
        sb = new StringBuilder();
        speakerBox = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        textBox = transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
        textAnimator = transform.GetComponent<Animator>();
        cam = Camera.main.GetComponent<CameraController>();
    }

    public IEnumerator StartDialogue()
    {
        if (isTalking)
            yield break;
        isTalking = true;

        textAnimator.enabled = true;
        textAnimator.SetBool("isTalking", true);
        GameManager.Instance.player.GetComponent<PlayerMovement>().isAllowedToMove = false;

        speakerBox.text = lines[0].speaker;
        yield return GameManager.Instance.WaitForAnimation(textAnimator, "Show", (n) => (n >= 1));

        for (int line = 0; line < lines.Length; line++)
        {
            string speakerName = lines[line].speaker;
            if (!speakerName.Equals("\""))
            {
                speakerBox.text = speakerName;
                SetCamTarget(speakerName);
            }

            for (int ch = 0; ch < lines[line].script.Length; ch++)
            {
                sb.Append(lines[line].script[ch]);
                textBox.text = sb.ToString();
                yield return new WaitForSeconds(delay);
            }

            yield return new WaitUntil(() => (Input.GetKeyDown(KeyCode.E)));
            sb = new StringBuilder();
        }

        textAnimator.SetBool("isTalking", false);

        yield return null;
        yield return GameManager.Instance.WaitForAnimation(textAnimator, "Hide", (n) => (n >= 1));

        isTalking = false;
        GameManager.Instance.player.GetComponent<PlayerMovement>().isAllowedToMove = true;
        textBox.text = "";
        speakerBox.text = "";
    }

    void SetCamTarget(string name)
    {
        for (int i = 0; i < speakers.Count; i++)
        {
            if (speakers[i].name.Equals(name))
                cam.target = speakers[i];
        }
    }
}
