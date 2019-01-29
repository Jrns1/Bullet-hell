 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arc : MonoBehaviour
{
    public Transform shadow;

    public float speed = 1;

    public Vector3 final = new Vector3(0, 0, 0);
    public Vector3 initial = new Vector3(0, 0, 0);

    public float h = 1;

    [Range(0, 1)]
    public float s = 0;

    public Vector3 UP = new Vector3(0, 1, 0);

    public float randomness;

    //public Vector3 pc = new Vector3(0, 0, 0);
    //public Vector3 CameraNormal = new Vector3(0, 1 / Mathf.Sqrt(2), -1 / Mathf.Sqrt(2));

    // Use this for initialization
    void Start()
    {
        final = Random.insideUnitCircle * randomness;
        shadow.position = final;
        //CameraNormal.Normalize();
    }


    private void Update()
    {
        transform.position = GameManager.Ins.Arc(initial, final, h, s);

        s = (s + speed * Time.deltaTime);
        if (s > 1)
        {
            GameManager.Ins.Particle("Dust", final);

            final = Random.insideUnitCircle * randomness;
            shadow.position = final;
            s = s % 1;
        }
    }

    /*
    void Update()
    {
        //p1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 fprime = f();
        //Vector3 Vy = proj(UP);
        //Vector3 Vx = Vector3.Cross(Vy, CameraNormal);
        //float x = Vector3.Dot(fprime, Vx);
        //float y = Vector3.Dot(fprime, Vy);
        this.transform.position = new Vector3(fprime.x, fprime.y, this.transform.position.z);

        if (shadow != null)
        {
            fprime.z = 0;
            //x = Vector3.Dot(fprime, Vx);
            //y = Vector3.Dot(fprime, Vy);
            shadow.position = new Vector3(fprime.x, fprime.y - .5f, shadow.position.z);
        }
        s = (s + speed * Time.deltaTime);
        if (s > 1)
        {
            s = s % 1;
        }
    }


    Vector3 f()
    {
        return s * final + (1 - s) * initial + UP * 4 * h * s * (1 - s);
    }

    /*
    Vector3 proj(Vector3 p)
    {
        return p - Vector3.Dot(p, CameraNormal) * CameraNormal;
    }
    */
}