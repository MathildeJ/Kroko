using UnityEngine;
using System.Collections;

public class gripeBehavior : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rg2d;
    CircleCollider2D collider;

    public float speed ;
    private float speed_ini;
    public int coin = 0;
    public int death = 0;
    public bool invincible;
    public float pushForce = 10.0f;
    public Vector3 position_ini;
    public float range_attack = 0.3f;


    int hashLeft = Animator.StringToHash("run_left");
    int hashRight = Animator.StringToHash("run_right");

    int hashStayLeft = Animator.StringToHash("stand_left");
    int hashStayRight = Animator.StringToHash("stand_right");

    int hashRightToLeft = Animator.StringToHash("turn_right_to_left");
    int hashLeftToRight = Animator.StringToHash("turn_left_to_right");

    public GameObject aBomb;
    bool isJumping = false;
    bool isCrouching = false;
    bool sprint = false;

    int hashJumpLeft = Animator.StringToHash("JumpLeft");
    int hashJumpRight = Animator.StringToHash("JumpRight");

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 body_pos = Vector3.zero;

    private int count_death_anim = 0;

    int hashDeadLeft = Animator.StringToHash("DeathLeft");
    int hashDeadRight = Animator.StringToHash("DeathRight");
    string sprite_death = "death_left";

    private float offset_collider = 0.02f;

    public float jump;
    int count = 0;

    private bool alive = true;
    private bool isAttacking = false;

    public float timeFall = 0f;
    public float deathLimit = 2.5f;

    GameObject groundedOn = null;
    bool isGrounded = false;

    public bool waiting = false;
    public float timeToWait = 7.0f; 
    float currentTime = 0f;

    // Score
    public float s_tps = 0;
    private GameMaster m_Master;

    float tps_die = 0.5f;
    bool bol_die;
    float time_die;
    int jump_change;

    AudioClip sound_kill;
    AudioClip sound_iskill;
    private AudioSource source;
    
    // Use this for initialization
    void Start()
    {
        //Audio
        source = GetComponent<AudioSource>();
        sound_kill = (AudioClip)Resources.Load("Sound/pshit");
        sound_iskill = (AudioClip)Resources.Load("Sound/crisp");

        animator = GetComponent<Animator>();
        rg2d = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
        currentTime = Time.time + timeToWait;
        speed_ini = speed;
        time_die = Time.time;
    }

    void FixedUpdate()
    {
        if (alive)
        {
            if (((animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashJumpLeft) || (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashJumpRight)) && !isJumping)
            {
                isJumping = true;
                /*
                if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashJumpLeft)
                {
                    jump_change = -1;
                }
                if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashJumpRight)
                {
                    jump_change = 1;
                }
                */
            }

            if (((animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashRightToLeft) || (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashLeftToRight)))
            {
                rg2d.AddForce(Vector2.zero, ForceMode2D.Impulse);
                jump_change = 0;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashLeft)
            {
                float vy = rg2d.velocity.y;
                rg2d.velocity = new Vector2(-speed, vy);

            }
            if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashRight)
            {
                float vy = rg2d.velocity.y;
                rg2d.velocity = new Vector2(speed, vy);
    
            }
            if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashStayRight)
            {
                float vy = rg2d.velocity.y;
                rg2d.velocity = new Vector2(0.0f, vy);
                jump_change = 0;
            }
            if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashStayLeft)
            {
                float vy = rg2d.velocity.y;
                rg2d.velocity = new Vector2(0.0f, vy);
                jump_change = 0;
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashDeadLeft)
        {
            sprite_death = "death_left";
        }
        if (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashDeadRight)
        {
            sprite_death = "death_right";
        }

    }

// Update is called once per frame
void Update()
    {
        if (isGrounded)
        {
            timeFall= 0f;
            isJumping = false;
            animator.SetBool("Jump", false);
        }
        if (!isGrounded)
        {
            timeFall += Time.deltaTime;

        }

        if (Input.anyKeyDown)
        {
            animator.SetBool("Wait", false);
            currentTime = Time.time + timeToWait;
        }
        else
        {
            checkTime();
        }

        if (alive) {
            animator.SetBool("Dead", false);

            if (Input.GetKey(KeyCode.P))
            {
                StartCoroutine("Die");
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                animator.SetBool("GoLeft", true);
                animator.SetBool("GoRight", false);
                if (!isGrounded && jump_change!=-1)
                {
                    rg2d.velocity = new Vector2(-speed/1.5f, rg2d.velocity.y);
                }
                else
                {
                    //rg2d.velocity = new Vector2(-speed, rg2d.velocity.y);
                }
                jump_change = -1;
                //Debug.Log(jump_change);


            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                animator.SetBool("GoLeft", false);
                animator.SetBool("GoRight", true);
                if (!isGrounded && jump_change!=1 &&isJumping)
                {
                    rg2d.velocity = new Vector2(speed/ 1.5f, rg2d.velocity.y);
                }
 
                jump_change = 1;
                //Debug.Log(jump_change);
            }
            else {
                animator.SetBool("GoLeft", false);
                animator.SetBool("GoRight", false);
            }

            if (Input.GetKey(KeyCode.DownArrow) && !isCrouching)
            {
                isCrouching = true;
                animator.SetBool("Crouch", true);
                float scale = collider.radius;
                Vector2 i_offset = collider.offset;
                collider.radius = scale*0.5f;
                collider.offset = i_offset + new Vector2(scale*0.5f, -scale * 0.5f + offset_collider);
            }

            else if (Input.GetKeyUp(KeyCode.DownArrow) && isCrouching)
            {
                isCrouching = false;
                animator.SetBool("Crouch", false);
                collider.radius = 0.1474877f;
                collider.offset = new Vector2(-0.04f, 0.0f);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && count == 0 && (isGrounded || isJumping))
            {
                animator.SetBool("Jump", true);
                count++;
             
                rg2d.AddForce(new Vector2(0.0f, jump), ForceMode2D.Impulse);

            }

            if (Input.GetKey(KeyCode.N))
            {
                invincible = true;
            }
            else
            {
                invincible = false;
            }

            if (!isJumping)
            {
                count = 0;
                //isGrounded = true;

                if (Input.GetKey(KeyCode.LeftShift) && !sprint)
                {
                    sprint = true;
                    float new_speed = speed * 1.5f;
                    speed = new_speed;
                }

                else if (Input.GetKeyUp(KeyCode.LeftShift) && sprint)
                {
                    sprint = false;
                    speed = speed_ini;
                }

            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if ((animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashLeft) || (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashStayLeft) || (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashJumpLeft))
                {
                    isAttacking = true;
                    animator.SetBool("Bite", true);
                    bite(-1);
                }
                else if ((animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashRight) || (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashStayRight) || (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashJumpRight))
                {
                    isAttacking = true;
                    animator.SetBool("Bite", true);
                    bite(1);
                }
                /*
                isAttacking = true;
                animator.SetBool("Bite", true);
                bite();
                */
            }

            if (Input.GetKeyUp(KeyCode.R) && isAttacking)
            {
                isAttacking = false;
                animator.SetBool("Bite", false);
            }

            //Coup de queue
            if (Input.GetKeyDown(KeyCode.T))
            {

                if ((animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashLeft) || (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashStayLeft) || (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashJumpLeft))
                {
                    isAttacking = true;
                    animator.SetBool("TailStrokeLeft", true);
                    tail(-1);
                }
                else if ((animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashRight) || (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashStayRight) || (animator.GetCurrentAnimatorStateInfo(0).shortNameHash == hashJumpRight))
                {
                    isAttacking = true;
                    animator.SetBool("TailStrokeRight", true);
                    tail(1);
                }
            
            }

            if (Input.GetKeyUp(KeyCode.T) && isAttacking)
            {
                isAttacking = false;
                animator.SetBool("TailStrokeLeft", false);
                animator.SetBool("TailStrokeRight", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Instantiate(aBomb, transform.position, transform.rotation);
            }

            if (timeFall >= deathLimit)
            {
                animator.SetBool("Wait", false);
                transform.position = position_ini;
                death++;
                m_Master.morts = death;
                m_Master.mortsT++;
                animator.SetBool("Dead", false);

            }

        }
        if (!alive)
        {
            StartCoroutine("Die");
            //death++;
        }
        
        if (Time.time  > time_die + 2 * tps_die)
        {
            bol_die = true;
        }
            


        // Scores
        m_Master = GameMaster.m_Instance;
        m_Master.tps += Time.deltaTime;
        m_Master.tpsT += Time.deltaTime;
    }

    void checkTime()
    {
        if (Time.time > currentTime)
        {
            animator.SetBool("Wait", true);
            currentTime = Time.time + timeToWait;
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        bool onground;
        onground = false;
        foreach (ContactPoint2D contact in col.contacts)
        {
            if (contact.normal.y > 0.01f)
            {
                isGrounded = true;
                groundedOn = col.gameObject;
                onground = true;
                break;
            }
            /*
            if (!onground)
            {
                isJumping = false;
            }
            */
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject == groundedOn)
        {
            groundedOn = null;
            isGrounded = false;
        }

    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (isGrounded)
        {
            isJumping = false;
            animator.SetBool("Jump", false);
        }
        else
        {
            timeFall = 0f;
            rg2d.AddForce(new Vector2(-0.3f, 0.0f), ForceMode2D.Impulse);
            //isGrounded = true;
        }


        if (col.transform.gameObject.name == "exit")
        {
            //alive = false;
            /*	m_Master.lvl++;
                    Debug.Log ("level"+m_Master.lvl);
                    Application.LoadLevel (1+m_Master.lvl);*/
            Debug.Log("bilan");
            Application.LoadLevel(1);
        }


        if (col.transform.gameObject.tag == "missile" || col.transform.gameObject.tag=="ennemi_killable" || col.transform.gameObject.tag == "ennemi_notkillable") 
        {
            if (!invincible)
            {
                alive = false;
                animator.SetBool("Dead", true);
            }
            //Debug.Log("Mort");
        }

    }

    IEnumerator Die()
    {
        if (bol_die)
        {
            death++;
            source.PlayOneShot(sound_iskill);
            m_Master.morts =death;
            m_Master.mortsT++;
            bol_die = false;
            time_die = Time.time;
        }
        body_pos = transform.position;
        yield return new WaitForSeconds(tps_die);
        //animator.CrossFade("stand_left", 0.0f);
        Spawn();
        

    }

    void Spawn()
    {
        animator.SetBool("Jump", false);
        animator.SetBool("Bite", false);
        animator.SetBool("Crouch", false);
        animator.SetBool("TailStrokeLeft", false);
        animator.SetBool("TailStrokeRight", false);
        transform.position = position_ini ;
        animator.SetBool("Dead", false);

        //death++;

        alive = true;

        GameObject body = new GameObject();
        body.transform.position = body_pos;

        Sprite[] death_Sprite = Resources.LoadAll<Sprite>(sprite_death);

        body.AddComponent<SpriteRenderer>();
        body.GetComponent<SpriteRenderer>().sprite = death_Sprite[4];
        body.AddComponent<Rigidbody2D>();
        body.AddComponent<BoxCollider2D>();

        StopCoroutine("Die");
    }

    void GainCoin(GameObject go)
    {
        coin++;
        m_Master.coin++;
        m_Master.coinT++;
        Destroy(go);
    }

    void bite(int side)
    {
        //ICI
        RaycastHit2D hit;
        hit = Physics2D.Linecast(transform.position + new Vector3(side*collider.radius, 0.0f, 0.0f), transform.position + new Vector3(side*collider.radius + side*range_attack, 0.0f, 0.0f));
        //Debug.DrawLine(transform.position + new Vector3(collider.radius, 0.0f, 0.0f), transform.position + new Vector3(collider.radius + range_bite, 0.0f, 0.0f), Color.blue);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "ennemi_killable")
            {
                //Debug.Log("Truc");
                //MES DEUX LIGNES
                hit.collider.GetComponent<Animator>().SetBool("Dead", true);
                source.PlayOneShot(sound_kill);
                Destroy(hit.collider.gameObject, 0.70f);

            }
        }
    }

    void tail(int side)
    {
        //ICI
        RaycastHit2D hit;
        hit = Physics2D.Linecast(transform.position + new Vector3(side * collider.radius, 0.0f, 0.0f), transform.position + new Vector3(side*collider.radius + side * range_attack, 0.0f, 0.0f));
        //Debug.DrawLine(transform.position + new Vector3(collider.radius, 0.0f, 0.0f), transform.position + new Vector3(collider.radius + range_bite, 0.0f, 0.0f), Color.blue);
        if (hit.collider != null)
        {
            //Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "ennemi_killable" || hit.collider.tag == "ennemi_notkillable")
            {
                float Vx = Mathf.Sqrt(pushForce) * Mathf.Cos(30 * Mathf.Deg2Rad);
                float Vy = Mathf.Sqrt(pushForce) * Mathf.Sin(30 * Mathf.Deg2Rad);
                //col.gameObject.transform.Translate(new Vector3(pushForce*Time.deltaTime, 0, 0));
                //hit.collider.gameObject.transform.Translate(new Vector3(side*Vx, Vy, 0));
                hit.collider.GetComponent<Rigidbody2D>().AddForce(new Vector3(side * Vx, Vy, 0),ForceMode2D.Impulse);
            }
        }
    }

    void OnGUI()
    {
        /*Texture2D[] Textures = new Texture2D[6];
        Textures = Resources.LoadAll<Texture2D>("C:/Users/AlexandreetJeremie/Documents/Croco/score.png");*/

        //Sprite s =  Resources.Load("score", typeof(Sprite)) as Sprite; //Textures[0];

        Sprite s = Resources.Load<Sprite>("score");
        /*Texture t = s.texture;
                 Rect tr = s.textureRect;
                 Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height );*/

        // GUI.DrawTextureWithTexCoords(new Rect(400, 400, tr.width, tr.height), t, r);

        GUI.Label(new Rect(30, 5, 100, 60), ": X" + m_Master.coin);
        GUI.Label(new Rect(80, 5, 100, 60), ": X" + m_Master.morts);

        GUI.Label(new Rect(30, 20, 160, 60), " Timer : " + m_Master.tps);

        GUI.Label(new Rect(600, 5, 100, 60), ": X" + m_Master.coinT);
        GUI.Label(new Rect(680, 5, 100, 60), ": X" + m_Master.mortsT);

        GUI.Label(new Rect(600, 20, 160, 60), " Timer : " + m_Master.tpsT);
    }

}
