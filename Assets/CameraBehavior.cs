using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {
    public GameObject player;
	public GameObject SECOND_PLAN;
    public float smooth_time = 0.3f;
    float speed = 0.0f;



	// Use this for initialization
	void Start () {
        //player = GameObject.Find("player");
		//SECOND_PLAN = GameObject.Find("SECOND_PLAN");

    }
	
	// Update is called once per frame
	void Update () {
        float playerX = player.transform.position.x;
		float playerY = player.transform.position.y;
        float cameraX = transform.position.x;
		float cameraY = transform.position.y;
		float SECOND_PLANX = SECOND_PLAN.transform.position.x;
        //if (Mathf.Abs(playerX - cameraX) > 0.5f)
        //{
            float pos = Mathf.SmoothDamp(cameraX, playerX, ref speed, smooth_time);
		transform.Translate(new Vector3(pos-cameraX, playerY-cameraY, 0.0f));
			SECOND_PLAN.transform.position+= new Vector3 (0.5f * (pos-cameraX), 0.5f * (pos - cameraY), 0);
        //}      

    }
		

}
