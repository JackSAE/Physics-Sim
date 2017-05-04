using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Ropes
{
    public Vector3 position;
    public Vector3 oldPositions;

    public Ropes(Vector3 position)
    {
        this.position = position;
        this.oldPositions = position;
    }
}

public class Rope : MonoBehaviour {

    public Transform startRope;
    public Transform endRope;

    LineRenderer lr;

    List<Ropes> allDemRopes = new List<Ropes>();

    float ropeLength = 0.5f;
    public int ropeTotalLength = 15;
    public float ropeWidth = 0.3f;
    public float changeRopeFloat = 0.5f;


    void Start()
    {

        lr = GetComponent<LineRenderer>();

        Vector3 ropesSectionsPos = startRope.position;

        for(int i = 0; i < ropeTotalLength; ++i)
        {
            allDemRopes.Add(new Ropes(ropesSectionsPos));
            ropesSectionsPos.y -= ropeLength;
        }
    }


    void Update()
    {
        DisplayTheRope();

        endRope.position = allDemRopes[allDemRopes.Count - 1].position;

        endRope.LookAt( allDemRopes[allDemRopes.Count - 2].position);
    }


    void FixedUpdate()
    {
        RopeSimUpdate();
    }


    void DisplayTheRope()
    {
        

        lr.startWidth = ropeWidth;
        lr.endWidth = ropeWidth / 2;

        Vector3[] pos = new Vector3[allDemRopes.Count];

        for(int i = 0; i < allDemRopes.Count; ++i)
        {
            pos[i] = allDemRopes[i].position;
        }

        lr.numPositions = pos.Length;
        lr.SetPositions(pos);
    }


    void RopeSimUpdate()
    {
        Vector3 gravity = Physics.gravity;

        Ropes firstRope = allDemRopes[0];

        firstRope.position = startRope.position;

        allDemRopes[0] = firstRope;

        for(int i = 1; i < allDemRopes.Count; ++i)
        {
            Ropes currentRope = allDemRopes[i];

            Vector3 ropeVelo = currentRope.position - currentRope.oldPositions;

            currentRope.oldPositions = currentRope.position;

            currentRope.position += ropeVelo;

            currentRope.position += gravity * Time.fixedDeltaTime;

            allDemRopes[i] = currentRope;
        }

        for(int i = 0; i < ropeTotalLength + 5; ++i)
        {
            CheckMaxStretch();
        }
    }


    void CheckMaxStretch()
    {
        for(int i = 0; i < allDemRopes.Count - 1; ++i)
        {
            Ropes topRope = allDemRopes[i];
            Ropes bottomRope = allDemRopes[i + 1];

            float distance = (topRope.position - bottomRope.position).magnitude;

            float distanceError = Mathf.Abs(distance - ropeLength);

            Vector3 changeDirection = Vector3.zero;

            if (distance > ropeLength)
                changeDirection = (topRope.position - bottomRope.position).normalized;
            else
                changeDirection = (bottomRope.position - topRope.position ).normalized;

            Vector3 changeRope = changeDirection * distanceError;

            if(i != 0)
            {
                bottomRope.position += changeRope * changeRopeFloat;

                allDemRopes[i + 1] = bottomRope;

                topRope.position -= changeRope * changeRopeFloat;

                allDemRopes[i] = topRope;
            }
            else
            {
                bottomRope.position += changeRope;
                allDemRopes[i + 1] = bottomRope;
            }
        } 
    }
}

