//sylvain.lefebvre@inria.fr
//[gprog] Problème

using UnityEngine;
using System.Collections;

public class Ennemi_Controller_Sabre : MonoBehaviour
{

    //Les différents comportements
    public bool Follow_trajet;
    public bool Fire;
    public bool ligne_vue;
    public bool stay_onplateform;
    public bool Follow_target;

    //La cible
    public GameObject Target;
    Vector2 vec_target;
    Vector3 vec_target3D;
    float target_circle;

    //Faire des tours de ronde
    public GameObject go_ward;
    Vector2 vec_ward;
    float ward_stop;

    //La ligne de vue
    bool OnSight_mode;
    RaycastHit2D[] hit;
    int nb_hit;
    bool has_sight;

    //Gestion du stay on plateform
    RaycastHit2D hit_s;
    RaycastHit2D hit_m;
    bool Fall_mode;
    float time_check;

    //gestion du Follow target
    bool Follow_mode;
    public float start_chase;

    //gestion interraction 
    bool Onchase_mode;

    //Fait feu
    public GameObject Missile;
    public float range;
    public float duration_missile;
    public float time_betweenfire;
    public float missile_speed;
    float t_lastshot;

    //L'ennemi
    public float speed;
    Rigidbody2D m_rbody;
    CircleCollider2D m_collider;
    float size_circle;
    float speed_ini;
    int right_left;
    int pred_right_left;
    GameObject m_gameobject;

    GameObject groundedOn = null;
    bool isGrounded = true;

    private Animator Animator_Ennemi;
    private int hash_lefttoright = Animator.StringToHash("Ennemi_lefttoright");
    private int hash_righttoleft = Animator.StringToHash("Ennemi_righttoleft");
    private int hash_goleft = Animator.StringToHash("Ennemi_goleft");
    private int hash_goright = Animator.StringToHash("Ennemi_goright");
    private int hash_beingkilled = Animator.StringToHash("emi_iskilledled");
    private int hash_isdead = Animator.StringToHash("Ennemi_isdead");
    private int current_state;

    private int hash_attleft = Animator.StringToHash("Ennemi_att_left");
    private int hash_attright= Animator.StringToHash("Ennemi_att_right");



    // Use this for initialization
    void Start()
    {
        tag = "ennemi_killable";
        Animator_Ennemi = GetComponent<Animator>();
        m_rbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<CircleCollider2D>();
        t_lastshot = Time.time - 2 * time_betweenfire;
        time_check = Time.time;
        size_circle = m_collider.radius;
        target_circle = Target.GetComponent<CircleCollider2D>().radius;
        Animator_Ennemi.SetBool("Dead", false);
        speed_ini = speed;

        ward_stop = 2 * size_circle;
        Follow_mode = false;
        Onchase_mode = false;

        if (Follow_trajet)
        {
            vec_ward = go_ward.transform.position - transform.position;
            if (vec_ward.x > 0)
            {
                //Debug.Log("A droite");
                Animator_Ennemi.SetBool("goright", true);
                Animator_Ennemi.SetBool("goleft", false);
            }
            else
            {
                //Debug.Log("A gauche");
                Animator_Ennemi.SetBool("goright", false);
                Animator_Ennemi.SetBool("goleft", true);
            }
        }
        else
        {
            Follow_trajet = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

        vec_target = Target.transform.position - transform.position;
        vec_target3D = new Vector3(vec_target.x, vec_target.y, 0.0f);

        //Determine si doit suivre
        if (Follow_target)
        {
            if (vec_target.y < size_circle + target_circle && vec_target.magnitude < start_chase)
            {
                Follow_mode = true;
            }
            else
            {
                Follow_mode = false;
            }
        }

        //Determine si ligne de vue
        if (ligne_vue)
        {
            hit = Physics2D.LinecastAll(transform.position + vec_target3D.normalized * 2 * size_circle, Target.transform.position - vec_target3D.normalized * target_circle * 2);
            nb_hit = Physics2D.LinecastNonAlloc(transform.position + vec_target3D.normalized * 2 * size_circle, Target.transform.position - vec_target3D.normalized * target_circle * 2, hit);
            Debug.DrawLine(transform.position + vec_target3D.normalized * 2 * size_circle, Target.transform.position - vec_target3D.normalized * target_circle * 2, Color.yellow);
            has_sight = true;
            for (int i = 0; i < nb_hit; i++)
            {
                if (!(hit[i].collider.tag == "missile"))
                {
                    has_sight = false;
                }
            }

            if (has_sight && vec_target.x * is_rightorleft() > 0)
            {
                OnSight_mode = true;
            }
            else
            {
                OnSight_mode = false;
            }

        }
        else
        {
            OnSight_mode = true;
        }

        //Parametrage Follow trajet
        if (Follow_trajet)
        {
            //if (vec_ward.magnitude < ward_dstop && !Onchase_mode)
            Debug.DrawLine(transform.position, go_ward.transform.position, Color.green);
            vec_ward = go_ward.transform.position - transform.position;
            if (Mathf.Abs(vec_ward.x) < ward_stop)
            {
                go_ward = go_ward.GetComponent<Trajet_Beacon>().Next_beacon;
                //Debug.Log("Change ward");
                if (!Onchase_mode)
                {
                    bounce();
                }
            }
        }

        //Risque de chute et mur
        if (stay_onplateform)
        {
            hit_s = Physics2D.Linecast(transform.position + new Vector3(is_rightorleft() * 3 * size_circle, 0.0f, 0.0f), transform.position + new Vector3(is_rightorleft() * 3 * size_circle, -3 * size_circle, 0.0f));
            hit_m = Physics2D.Linecast(transform.position + new Vector3(is_rightorleft() * 2 * size_circle, 0.0f, 0.0f), transform.position + new Vector3(is_rightorleft() * 4 * size_circle, 0.0f, 0.0f));
            Debug.DrawLine(transform.position + new Vector3(is_rightorleft() * 3 * size_circle, 0.0f, 0.0f), transform.position + new Vector3(is_rightorleft() * 3 * size_circle, -3 * size_circle, 0.0f), Color.red);
            Debug.DrawLine(transform.position + new Vector3(is_rightorleft() * 2 * size_circle, 0.0f, 0.0f), transform.position + new Vector3(is_rightorleft() * 4 * size_circle, 0.0f, 0.0f), Color.blue);

            //Debug.Log(Time.time + time_check);
            if (Time.time - time_check > 0.01f)//(1/speed)*size_circle)
            {
                /*
                //Le debug
                if (hit_m.collider != null)
                {
                    Debug.Log(hit_m.collider.name);
                }
                if (hit_s.collider == null)
                {
                    Debug.Log("Ne touche pas le sol");
                }
                */

                if (hit_m.collider != null)
                {
                    if (hit_m.collider.tag == "Player")
                    {
                        Animator_Ennemi.SetBool("attack", true);
                    }
                    else if (hit_m.collider.tag != "missile")
                    {
                        bounce();
                        time_check = Time.time;
                    }
                }

                else if (hit_s.collider == null || hit_m.collider != null)
                {
                    bounce();
                    time_check = Time.time;
                }

            }
            else
            {
                Fall_mode = false;
            }
        }
        else
        {
            Fall_mode = false;
        }
    }

    void FixedUpdate()
    {
        Animator_Ennemi.SetBool("attack", false);
        current_state = Animator_Ennemi.GetCurrentAnimatorStateInfo(0).shortNameHash;
        /*
        if (m_collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        */
        m_rbody.velocity = def_speed();
        if (current_state == hash_beingkilled)
        {
            print("Currently killed");
        }
        if (current_state == hash_isdead)
        {
            Debug.Log("Godead");
            be_killed();
        }

        //Debug.Log(isGrounded);

        //Debug.Log(Follow_mode + " "+ OnSight_mode +" "+ !Fall_mode);
        if (Follow_mode && OnSight_mode && !Fall_mode)
        {
            Onchase_mode = true;
        }
        else
        {
            Onchase_mode = false;
        }


        Debug.DrawLine(transform.position + vec_target3D.normalized * size_circle, Target.transform.position - vec_target3D.normalized * target_circle, Color.red);

        if (Fire)
        {
            //Debug.Log(vec_target.magnitude);
            if (vec_target.magnitude < range && Time.time - t_lastshot > time_betweenfire)
            {
                if (ligne_vue)
                {
                    if (OnSight_mode)
                    {
                        Shot();
                        t_lastshot = Time.time;
                    }
                }
                else
                {
                    Shot();
                    t_lastshot = Time.time;
                }
            }
        }
    }

    void Shot()
    {
        var MissileClone = (GameObject)Instantiate(Missile, transform.position + vec_target3D.normalized * 2 * size_circle, transform.rotation) as GameObject;
        MissileClone.GetComponent<Rigidbody2D>().velocity = vec_target3D.normalized * missile_speed;
        MissileClone.tag = "missile";
        Destroy(MissileClone, duration_missile);
    }

    //La gestion de l'animator
    /*Distinguer l'intention et l'action => temporiser*/
    void bounce()
    {

        if (current_state == hash_goleft)
        {
            Animator_Ennemi.SetBool("goright", true);
            Animator_Ennemi.SetBool("goleft", false);
            //Debug.Log("go_left");
        }
        else if (current_state == hash_goright)
        {
            Animator_Ennemi.SetBool("goright", false);
            Animator_Ennemi.SetBool("goleft", true);
            //Debug.Log("go_right");
        }
    }

    Vector3 def_speed()
    {

        Vector3 vitesse = new Vector3(m_rbody.velocity.x, m_rbody.velocity.y, 0.0f);
        if (isGrounded)
        {
            /*
            if (current_state == hash_lefttoright || current_state == hash_righttoleft)
            {
                vitesse = new Vector3(0.0f, m_rbody.velocity.y, 0.0f);
            }
            */
            vitesse = new Vector3(0.0f, m_rbody.velocity.y, 0.0f);
            if (current_state == hash_goleft)
            {
                vitesse = new Vector3(-speed, m_rbody.velocity.y, 0.0f);
            }
            else if (current_state == hash_goright)
            {
                vitesse = new Vector3(speed, m_rbody.velocity.y, 0.0f);
            }
            return vitesse;
        }

        return vitesse;
    }

    int is_rightorleft()
    {
        if (current_state == hash_goleft)
        {
            return -1;
        }
        return 1;
    }


    void be_killed()
    {
        Debug.Log("Isdead");
        Destroy(this.gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        isGrounded = true;

        foreach (ContactPoint2D contact in col.contacts)
        {
            if (contact.collider.tag=="Player")
            {
                Animator_Ennemi.SetBool("attack", true);
            }
        }

    }

    /*
    void OnCollision(Collision2D col)
    {
        isGrounded = true;
    }

    
    void OnCollisionEnter2D(Collision2D col)
    {
        isGrounded = true;
        
        foreach (ContactPoint2D contact in col.contacts)
        {

            if (contact.normal.y > 0.01f)
            {
                isGrounded = true;
                groundedOn = col.gameObject;
                break;
            }
        }
        
    }

    void OnCollisionExit2D(Collision2D col)
    {
        
        if (col.gameObject == groundedOn)
        {
            groundedOn = null;
            isGrounded = false;
        }
        
        isGrounded = false;
    }

    */
}