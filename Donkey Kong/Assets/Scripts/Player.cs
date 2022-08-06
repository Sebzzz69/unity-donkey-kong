using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite climbSprite;
    private int spriteIndex;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 3f;
    public float jumpStrength = 1f;
    private float spawnPosX = -5.25f;
    private float spawnPosY = -5.424f;

    private bool grounded;
    private bool climbing;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f/12f, 1f/12f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);

        // Loops through objects Mario touches
        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            
            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                //Sets boolean 'grounded' to true
                grounded = hit.transform.position.y < transform.position.y - 0.5f;

                //Ignores collision if Mario hits an object above him while jumping
                Physics2D.IgnoreCollision(collider, results[i], !grounded);
            } else if (hit.layer == LayerMask.NameToLayer("Ladder"))
            {
                climbing = true;
            }
        }
    }

    

    private void Update()
    {
        CheckCollision();

        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;

        }
        else if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
        } else if (grounded && Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up * jumpStrength;
        } else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

        if (grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }
       
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Border"))
        {
            transform.position = new Vector3(spawnPosX, spawnPosY);
            Debug.Log("Mario Fell Out Of The World");
        }
    }

    private void AnimateSprite()
    {
        if (climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }else if (direction.x != 0)
        {
            spriteIndex++;

            if (spriteIndex >= runSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runSprites[spriteIndex];
        }else if (direction.x == 0)
        {
            spriteRenderer.sprite = runSprites[0];
        }
    }

}
