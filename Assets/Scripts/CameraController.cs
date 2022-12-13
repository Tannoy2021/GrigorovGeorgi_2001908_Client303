using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerManager player;
    public float sensitivity = 80f;
    private float rotation;
    private void Start()
    {
       rotation = player.transform.eulerAngles.y;
    }
    private void Update()
    {
        Look();
    }
    private void Look()
    {
        float _rotation = Input.GetAxis("Mouse X");
        rotation += _rotation * sensitivity * Time.deltaTime;
        player.transform.rotation = Quaternion.Euler(0f, rotation, 0f);
    }
}
