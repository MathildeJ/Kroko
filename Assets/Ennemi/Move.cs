using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {

    Rigidbody2D m_rigid;
    public float force = 3;
    bool moving;

    // Use this for initialization
    void Start () {

        m_rigid = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        moving = false;

        if (Input.GetKey(KeyCode.Z))
        {
            m_rigid.velocity = new Vector2(0.0f, force);
            moving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            m_rigid.velocity = new Vector2(0.0f, -force);
            moving = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            m_rigid.velocity = new Vector2(force, 0.0f);
            moving = true;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            m_rigid.velocity = new Vector2(-force, 0.0f);
            moving = true;
        }
        if (!moving)
        {
            m_rigid.velocity = new Vector2(0.0f, 0.0f);
        }
    }
}
