using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    public GameManager ServerPosition;
    //private Vector3 serverPos;
    //private Vector3 clientPos;
    //private Rigidbody rigidbody;
    //private int id;
    //public Vector3 ServerPos { get => serverPos; set => serverPos = value; }
    //public Vector3 ClientPos { get => clientPos; set => clientPos = value; }
    //public int Id { get=> id; set => id = value; }
    private void FixedUpdate()
    {
        SendInputToServer();
    }
    private void Start()
    {
       // rigidbody= GetComponent<Rigidbody>();
    }
    private void SendInputToServer()
    {
        bool[] _clientInputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.LeftShift)
        };
        Vector2 direction = Vector2.zero;
        if (_clientInputs[0])
        {
            direction.y += 1;
        }
        if (_clientInputs[1])
        {
            direction.y -= 1;
        }
        ClientSend.PlayerMovement(_clientInputs);
      // Move();
    }
    private void Move()
    {
      // Debug.Log($"{serverPos.x} and {serverPos.z} ");
      // Debug.Log($"{transform.position.x} {transform.position.z}");
        //serverPos = ServerPosition.transform.position;
        //rigidbody.MovePosition(new Vector3(Mathf.Lerp(rigidbody.position.x, serverPos.x, 0.5f),
        //Mathf.Lerp(rigidbody.position.y, serverPos.y, 0.5f),
        //Mathf.Lerp(rigidbody.position.z, serverPos.z, 0.5f)));

    }

}
