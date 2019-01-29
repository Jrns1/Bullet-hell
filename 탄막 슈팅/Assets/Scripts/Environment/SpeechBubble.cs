using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SpeechBubble : MonoBehaviour {

    public float delay;

    public TMPro.TextMeshProUGUI textBox;

    public IEnumerator Type(string str)
    {
        StringBuilder sb = new StringBuilder();

        for (int ch = 0; ch < str.Length; ch++)
        {
                sb.Append(str[ch]);
                textBox.text = sb.ToString();
                yield return new WaitForSeconds(delay);
        }
    }

    public IEnumerator Dispose()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Hide");
        yield return GameManager.Ins.WaitForAnimation(animator, "Hide", (t) => (t >= 1));
        ObjectPool.Ins.PushToPool(gameObject);
    }
}
