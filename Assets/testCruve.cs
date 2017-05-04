using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testCruve : MonoBehaviour {

    public List<GameObject> controlPoints;
    public GameObject target;
    public float progress = 0;

    Vector3 hermite(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t, float tension, float bias)
    {
        Vector3 m0, m1;
        float a0, a1, a2, a3;
        m0 = (p1 - p0) * (1.0f + bias) * (1.0f - tension) / 2.0f;
        m0 += (p2 - p1) * (1.0f - bias) * (1.0f - tension) / 2.0f;
        m1 = (p2 - p1) * (1.0f + bias) * (1.0f - tension) / 2.0f;
        m1 += (p3 - p2) * (1.0f - bias) * (1.0f - tension) / 2.0f;
        a0 = 2.0f * t * t * t - 3.0f * t * t + 1.0f;
        a1 = t * t * t - 2 * t * t + t;
        a2 = t * t * t - t * t;
        a3 = -2 * t * t * t + 3 * t * t;
        return (a0 * p1 + a1 * m0 + a2 * m1 + a3 * p2);
    }

    Vector3 eval(float t)
    {
        Vector3 p = new Vector3();
        int segment = (int)(t * (controlPoints.Count - 1));
        int p0 = segment - 1;
        if (p0 < 0)
            p0 += controlPoints.Count;
        int p1 = (p0 + 1) % controlPoints.Count;
        int p2 = (p0 + 2) % controlPoints.Count;
        int p3 = (p0 + 3) % controlPoints.Count;
        p = hermite(controlPoints[p0].transform.position,
                    controlPoints[p1].transform.position,
                    controlPoints[p2].transform.position,
                    controlPoints[p3].transform.position,
                    (t * (controlPoints.Count - 1)) % 1.0f, 0.0f, 0.5f);
        //Debug.Log(t + "   " + segment + "   " + t / (controlPoints.Count - 1));
        return p;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        progress += Time.deltaTime * 0.2f;
        progress = progress % 1.0f;
        target.transform.position = eval(progress);
        target.transform.LookAt(eval(progress + 0.01f));
        for (int i = 0; i < controlPoints.Count - 1; ++i)
        {
            Debug.DrawLine(controlPoints[i].transform.position, controlPoints[i + 1].transform.position, new Color(1, 1, 1));
        }
        Vector3 lastPoint = controlPoints[0].transform.position;
        for (float t = 0; t <= 1.0; t += 0.01f)
        {
            //Vector3 p = hermite(controlPoints[0].transform.position, controlPoints[1].transform.position, controlPoints[2].transform.position, controlPoints[3].transform.position, t, 0, 0.5f);
            Vector3 p = eval(t);
            Debug.DrawLine(lastPoint, p);
            lastPoint = p;
        }
    }
}
