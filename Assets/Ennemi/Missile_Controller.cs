using UnityEngine;
using System.Collections;

public class Missile_Controller : MonoBehaviour {

    Rigidbody2D m_rbody;
    Vector2 aim;


    // Use this for initialization
    void Start () {
        m_rbody = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Fire(Vector2 direction)
    {
        m_rbody.velocity = direction;
        //Destroy(this, 1);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        DestroyObject(gameObject);

    }
}
