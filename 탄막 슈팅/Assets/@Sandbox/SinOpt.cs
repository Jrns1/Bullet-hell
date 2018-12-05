using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;



public class SinOpt : MonoBehaviour {

    public struct Data
    {
        public Vector2 a;
        public Vector2 b;
        public float dst;

        public Data(Vector2 a, Vector2 b, float dst)
        {
            this.a = a;
            this.b = b;
            this.dst = dst;
        }
    }

    delegate bool TestFunc(Vector2 a, Vector2 b, float dst);

    public int repeat = 30;

    float[] sinList;


    private void Awake()
    {
        sinList = new float[360];
        for (int i = 0; i < 360; i++)
        {
            sinList[i] = Mathf.Sin(Mathf.Deg2Rad * i);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Data[] array = new Data[repeat];
            for (int i = 0; i < repeat; i++)
            {
                array[i] = new Data(Random.insideUnitCircle, Random.insideUnitCircle, Random.Range(0, .5f));
            }

            Repeat(A, array);
            Repeat(B, array);
        }

    }

    void Repeat(TestFunc Test, Data[] list)
    {
        Stopwatch s = Stopwatch.StartNew();
        for (int i = 0; i < repeat; i++)
        {
            Test(list[i].a, list[i].b, list[i].dst);
        }
        UnityEngine.Debug.Log(s.Elapsed);
    }

    private bool A(Vector2 a, Vector2 b, float dst)
    {
        Vector2 diff = a - b;
        return diff.x * diff.x + diff.y * diff.y <= dst * dst;
    }

    private bool B(Vector2 a, Vector2 b, float dst)
    {
        return Vector2.Distance(a, b) <= dst;
    }
}
