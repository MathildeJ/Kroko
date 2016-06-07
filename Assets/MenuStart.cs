using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuStart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BoutonStart()
    {
        SceneManager.LoadScene("scene");

    }
}
