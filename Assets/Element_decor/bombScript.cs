using UnityEngine;
using System.Collections;

public class bombScript : MonoBehaviour {

    private Animator animator;
    float cmp = 0.0F;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        cmp += Time.deltaTime;
        if (cmp > 2.0F)
        {
            //animator.SetBool("GoOff", true);

            //yield return new WaitForSeconds(0.167f);
            //Destroy(gameObject);
        }

    }
}
