using UnityEngine;
using System.Collections;

public class Trajet_Beacon : MonoBehaviour {

    public GameObject Next_beacon;

	// Use this for initialization
	void Start () {
        SpriteRenderer rendered = GetComponent<SpriteRenderer>();
        rendered.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
