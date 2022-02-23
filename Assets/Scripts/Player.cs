using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    float moveSpeed=5f;

    [SerializeField]
    float jumpforce=5f;

    [SerializeField]
    float jumpforceShort=2f;
    
    Rigidbody2D rb2D;
    SpriteRenderer spr;
    Animator anim;

    [SerializeField]
    float raydistance=5f;

    [SerializeField]
    Color raycolor=Color.red;

    [SerializeField]
    LayerMask groundlayer;

    [SerializeField]
    Vector3 rayOrigin;

    GameInputs gameInputs;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gameInputs = new GameInputs();
    }

    void Start()
    {
        //gameInputs.Gameplay.Jump.performed += _ => Jump();
    }

    // Update is called once per frame
    void OnEnable()
    {
        gameInputs.Enable();
    }

    void OnDisable()
    {
        gameInputs.Disable();
    }

    public void OnJump(InputAction.CallbackContext context){
        switch(context.phase){
            case InputActionPhase.Canceled:
                Jump();
                Debug.Log("Largo");
                break;
            case InputActionPhase.Performed:
                JumpShort();
                Debug.Log("corto");
                break;
        }
    }

    void FixedUpdate()
    {
        rb2D.position += Horizontal * Vector2.right * moveSpeed * Time.fixedDeltaTime;
    }

    void Update()
    {
        //transform.Translate(Axis.x*Vector2.right*moveSpeed*Time.deltaTime);
        spr.flipX = FlipSpriteX;
        /*if(JumpButton && IsGrounding)
        {
            rb2D.AddForce(Vector2.up*jumpforce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }*/
    }

    void LateUpdate()
    {
        anim.SetFloat("AxisX", Mathf.Abs(Horizontal));
        anim.SetBool("ground", IsGrounding);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color=raycolor;
        Gizmos.DrawRay(transform.position + rayOrigin,Vector2.down*raydistance);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("coin"))
        {
            Coin coin = other.GetComponent<Coin>();
            GameManager.instance.GetScore.AddPoints(coin.GetPoints);
            Destroy(other.gameObject);
        }
    }

    void Jump()
    {
        if(IsGrounding)
        {
            rb2D.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }

    void JumpShort()
    {
        if(IsGrounding)
        {
            rb2D.AddForce(Vector2.up * jumpforceShort, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }

    //Vector2 Axis => new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
    //bool JumpButton => Input.GetButtonDown("Jump");
    float Horizontal => gameInputs.Gameplay.Horizontal.ReadValue<float>();
    bool IsGrounding => Physics2D.Raycast(transform.position + rayOrigin, Vector2.down, raydistance, groundlayer);
    bool FlipSpriteX => Horizontal > 0f ? false : Horizontal < 0f ? true : spr.flipX;
}
