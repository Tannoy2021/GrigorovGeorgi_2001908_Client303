using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public Vector3 position;
    public MeshRenderer model;
    private Vector3 pointA = Vector3.zero;
    private Vector3 PointB = Vector3.zero;
    private float lerpT;
    // server and client are both set to 60 fps thats why 1 / 60 to find tick time 
    private float timeBetweenTicks = 1.0f / 60.0f;
    public void SetLerpPosition(Vector3 position)
    {
        pointA = PointB;
        PointB = position;
        lerpT = Time.time;
    }
    private void Update()
    {
        this.transform.position = Vector3.Lerp(pointA, PointB, (Time.time - lerpT) / timeBetweenTicks);
    }
    public void Initialize(int _id, Vector3 _position )
    {
        id= _id;
        position= _position;
    }
}
