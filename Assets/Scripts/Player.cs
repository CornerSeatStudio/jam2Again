using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour {

    public AudioClip audJump;
    public AudioClip audSwing;
    public int initHealth = 3;

    [Header("Jump Settings")]
    public float jumpForce = 100f;
    public float groundCheckThreshold = .1f;
    public int jumpCount = 1;

    [Header("Move Settings")]
    public float maxMoveSpeed = 10f;
    float inputRaw;
    int lastMoveDir = 0;
    
    //normal dependencies
    public Rigidbody2D Rb2D {get; private set; }
    public Collider2D Col {get; private set; }
    public Animator Animator {get; private set; }

    //other
    int Health {get; set; }
    int jumpMode;  //0 - none left, >0 - jumps left
    bool facingLeft = false;
    // bool Attacking {get; set; } = false;

    void Start() {
        Rb2D = GetComponent<Rigidbody2D>();
        Col = GetComponent<Collider2D>();
        Animator =  GetComponent<Animator>();

        Health = initHealth;
        jumpMode = jumpCount;
    }

    void FixedUpdate() {
        handleMovement();
        
    }

    // void clampVelocity() {
    //     if(isGrounded())  Rb2D.velocity = Vector3.ClampMagnitude(Rb2D.velocity, maxMoveSpeed);
    // }
    void counterMovement() {
        if(switchedDir() || inputRaw == 0) Rb2D.velocity = new Vector2(0f, Rb2D.velocity.y);
    }
    void Update() {
        inputRaw = Input.GetAxisRaw("Horizontal");
        handleJump();
        counterMovement();
        flipToFace();

        lastMoveDir = (int) inputRaw;
    }

    void LateUpdate() {
        // Debug.Log(Rb2D.velocity.x);
        Animator.SetBool(Animator.StringToHash("Moving"), inputRaw != 0);
    }
    bool switchedDir() => lastMoveDir == (int) inputRaw;
    void flipToFace() {
        if(inputRaw > 0 && facingLeft || inputRaw < 0 && !facingLeft){
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y); 
            facingLeft = !facingLeft;
        }
    }
    
    void handleJump(){
        // Debug.Log(isGrounded());
        if(jumpMode != 0 && isGrounded() && Input.GetButtonDown("Jump")){
            jumpMode--;
            // Rb2D.velocity = new Vector2(Rb2D.velocity.x, jumpForce);

            // Rb2D.velocity += (new Vector2(0f, jumpForce *10) );
            // Debug.Log(Rb2D.velocity);
            Rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(audJump);

        }

        if(jumpMode == 0 && isGrounded()){ //temp
            jumpMode = jumpCount;
        }
    }

    void handleMovement(){

        // float sped = Mathf.SmoothDamp(Rb2D.velocity.x, maxMoveSpeed * inputRaw, ref currVel, smooth);
        Rb2D.velocity = new Vector2(inputRaw * maxMoveSpeed, Rb2D.velocity.y);
       
        // Rb2D.AddForce(new Vector2((inputRaw * moveForce * 10f * Time.fixedDeltaTime), 0f));
    }


    public bool isGrounded() {
        // Debug.DrawRay(transform.position, Vector2.down, Color.red, 10, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Col.bounds.extents.y + groundCheckThreshold, ~LayerMask.GetMask("Player"));
        return hit;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        
    }

    private void OnCollisionExit2D(Collision2D other) {
        
    }

}
