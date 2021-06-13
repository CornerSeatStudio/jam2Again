using System.Collections;
using UnityEngine;


public class Player : MonoBehaviour {

    public int Player_ID {get; set; } = 1;
    public AudioClip audDeath;
    public AudioClip audJump;
    public AudioClip audDamage;
    public int initHealth = 3;
    public float invulnTime = 2f;

    [Header("Jump Settings")]
    public float jumpForce = 100f;
    public float groundCheckThreshold = .1f;
    public int jumpCount = 1;

    [Header("Move Settings")]
    public float maxMoveSpeed = 10f;
    float inputRaw;
    int lastMoveDir = 0;
    public float slopeCheckMod = 2f;
    public float slopeDownForce = 50f;
    
    //normal dependencies
    public Rigidbody2D Rb2D {get; private set; }
    public Collider2D Col {get; private set; }
    public Animator Animator {get; private set; }
    public SpriteRenderer sprenderer {get; private set; }
    public GameHandler gameHandler {get; private set; }

    //other
    public int Health {get; set; }
    int jumpMode;  //0 - none left, >0 - jumps left
    public bool FacingLeft {get; private set; } = false;
    public bool Invulnerable {get; set; } = false;
    // bool Attacking {get; set; } = false;

    void Start() {
        Rb2D = GetComponent<Rigidbody2D>();
        Col = GetComponent<Collider2D>();
        Animator =  GetComponent<Animator>();
        sprenderer = GetComponent<SpriteRenderer>();
        gameHandler = FindObjectOfType<GameHandler>();

        Health = initHealth;
        jumpMode = jumpCount;
    }

    void FixedUpdate() {
        handleMovement();
        if(OnSlope()) Rb2D.AddForce(Vector2.down * slopeDownForce);

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
        Animator.SetBool(Animator.StringToHash("Falling"), !isGrounded());
    }


    bool switchedDir() => lastMoveDir == (int) inputRaw;
    void flipToFace() {
        if(Health <= 0) return;

        if(inputRaw > 0 && FacingLeft || inputRaw < 0 && !FacingLeft){
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y); 
            FacingLeft = !FacingLeft;
        }
    }
    
    void handleJump(){
        if(Health <= 0) return;
        // Debug.Log(isGrounded());
        if(jumpMode != 0 && Input.GetButtonDown("Jump")){
            jumpMode--;
     
            Rb2D.velocity = new Vector2(Rb2D.velocity.x, 0f);
            Rb2D.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            AudioSource audio = GetComponent<AudioSource>();
            audio.PlayOneShot(audJump);

        }

        if(jumpMode == 0 && (isGrounded() || OnSlope())){ //temp
            jumpMode = jumpCount;
        }
    }

    void handleMovement(){
        if(Health <= 0) return;

        // float sped = Mathf.SmoothDamp(Rb2D.velocity.x, maxMoveSpeed * inputRaw, ref currVel, smooth);
        Rb2D.velocity = new Vector2(inputRaw * maxMoveSpeed, Rb2D.velocity.y);
       
        // Rb2D.AddForce(new Vector2((inputRaw * moveForce * 10f * Time.fixedDeltaTime), 0f));
    }


    public bool isGrounded() {
        // Debug.DrawRay(transform.position, Vector2.down, Color.red, 10, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Col.bounds.extents.y + groundCheckThreshold, ~LayerMask.GetMask("Player"));
        return hit;
    }

    bool OnSlope(){
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Col.bounds.extents.y * slopeCheckMod, ~LayerMask.GetMask("Player"));
        return hit && hit.normal != Vector2.up;
    }

    public void takeDamage(){
        Health--;
        GetComponent<AudioSource>().PlayOneShot(audDamage);
        if(Health <= 0){
            onDeath();
        } else {
            StartCoroutine(tempInvuln());
        }
    }

    public void onDeath(){
        //todo fall to floor
        Animator.Play(Animator.StringToHash("death"));
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audDeath);
        // Col.enabled = false;

        StartCoroutine(disablePlayerAfterDeath());

        gameHandler.onGameEnd();
    }

    public IEnumerator disablePlayerAfterDeath(){
        yield return new WaitForSeconds(2f);
        Rb2D.isKinematic = true;
        Col.enabled = false;
    }

    
    IEnumerator flashing;

    IEnumerator flashHandle(){
        while(true){
            sprenderer.color = new Color(1f,1f,1f,.2f);
            yield return new WaitForSeconds(.2f);
            sprenderer.color = Color.white;
            yield return new WaitForSeconds(.2f);


        }
    }

    IEnumerator tempInvuln(){
        Invulnerable = true;
        flashing = flashHandle();
        StartCoroutine(flashing);
        yield return new WaitForSeconds(invulnTime);
        StopCoroutine(flashing);
        sprenderer.color = Color.white;
        Invulnerable = false;
        // Debug.Log("invlun done");
    }

    IEnumerator tempIgnoreCollision(Collision2D other){
        Physics2D.IgnoreCollision(other.collider, Col);
        yield return new WaitForSeconds(invulnTime);
        Debug.Log("COLLISION PLEASE FIX");
        Physics2D.IgnoreCollision(Col, other.collider, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(Col, other.collider, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(Col, other.collider, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(Col, other.collider, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(Col, other.collider, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(Col, other.collider, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(other.collider, Col, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(other.collider, Col, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(other.collider, Col, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(other.collider, Col, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(other.collider, Col, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(other.collider, Col, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(other.collider, Col, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        Physics2D.IgnoreCollision(other.collider, Col, false); //THIS IS BUGGED - SO SPAM IT UNTIL IT WORKS LMFAO
        yield return null;

    }
    private void OnCollisionEnter2D(Collision2D other) {

        if(other.gameObject.CompareTag("Enemy") && !Invulnerable){
            takeDamage();
            StartCoroutine(tempIgnoreCollision(other));
            // Debug.Log("perhaps");
        }
    }


    // private void OnCollisionExit2D(Collision2D other) {
    //     if(other.gameObject.CompareTag("Enemy")){
    //         if(Invulnerable){
    //             Physics2D.IgnoreCollision(other.collider, Col, false);
    //         }
    //     }
    // }

}
