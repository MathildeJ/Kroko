using UnityEngine;
using System.Collections;

public class Plateforme_mouvante : MonoBehaviour {

    //Faire des tours de ronde
    public GameObject go_ward;
    Vector3 vec_ward;
    float ward_stop;
    public float speed = 0.9f;
    Rigidbody2D m_rbody;
    Animator animator;

    /*
    private int hash_goleft = Animator.StringToHash("Ennemi_goleft");
    private int hash_goright = Animator.StringToHash("Ennemi_goright");
    private int current_state;
    */

    // Use this for initialization
    void Start () {
        m_rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ward_stop = 0.05f;
        vec_ward = go_ward.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        m_rbody.velocity = speed * (vec_ward.normalized);

        Debug.DrawLine(transform.position, go_ward.transform.position, Color.yellow);
        vec_ward = go_ward.transform.position - transform.position;
        if (vec_ward.magnitude < ward_stop)
        {
            go_ward = go_ward.GetComponent<Trajet_Beacon>().Next_beacon;
            //Debug.Log("Change ward");
            //bounce();
        }
        if (vec_ward.x >0)
        {
            animator.SetInteger("left_right", 1);
        }
        else
        {
            animator.SetInteger("left_right", -1);
        }
    }

    /*
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
    */
}
