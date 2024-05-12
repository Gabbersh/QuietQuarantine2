
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 10f;
    private void Update()
    {
        if (IsOwner)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            transform.position += new Vector3(x, 0, z) * speed * Time.deltaTime; 
        }
    }
}
