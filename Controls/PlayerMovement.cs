using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController cc;

    private float speed = 5f;

    private Vector3 velocity;
    public float gravity = -9.81f;

    private float timer = 0.0f;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            timer += Time.deltaTime;
            if (timer >= 1.5f)
            {
                speed = 20f;
            }

            else { speed = 10.0f; }
        }

        else { speed = 5f; timer = 0.0f; }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        cc.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        cc.Move(velocity * Time.deltaTime);

        if (cc.isGrounded) 
        {
            velocity.y = 0;
        }

    }
}
