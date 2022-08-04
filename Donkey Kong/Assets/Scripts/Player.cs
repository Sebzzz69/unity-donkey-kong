using UnityEngine;

public class Player : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    private Vector2 direction;
    public float moveSpeed = 3f;
    public float jumpStrength = 1f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up * jumpStrength;
        } else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;
        direction.y = Mathf.Max(direction.y, -1f);

        //Changes the direction Mario is facing depending on which way he is walking
        if (direction.x > 0)
        {
            transform.eulerAngles = Vector3.zero;
        } else if (direction.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

    }

    private void FixedUpdate()
    {
        // Moves Mario
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }

}
