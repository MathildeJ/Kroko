using UnityEngine;
using System.Collections;

public class Bilan : MonoBehaviour {

	public int m_Score=0;
	private GameMaster m_Master;
	// Use this for initialization
	void Start () {
		//m_Master = GameObject.Find ("GameMaster1").GetComponent<GameMaster> ();
	}

	// Update is called once per frame
	void Update () {
		m_Master = GameMaster.m_Instance;
	}

	void OnGUI(){
		GUI.Label (new Rect (450, 10, 400, 50), "Level " + m_Master.lvl + ": ");

		GUI.Label (new Rect (450, 100, 400, 150), "Temps " + m_Master.tps);

		GUI.Label (new Rect (450, 120, 400, 250), "Pièces " + m_Master.coin);

		GUI.Label (new Rect (450, 140, 400, 450), "Morts " + m_Master.morts);


		int score = 0;

		if(m_Master.coin >= 30)
			score ++;
		
		if(m_Master.tps < 20 && m_Master.tps>0)
			score ++;

		if(m_Master.morts==0)
			score ++;

		if(score==0)
			GUI.Label (new Rect (450, 160, 400, 450), "Rang C ");
		else if(score==1)
			GUI.Label (new Rect (450, 160, 400, 450), "Rang B ");
		else if(score==2)
			GUI.Label (new Rect (450, 160, 400, 450), "Rang A ");
		else if(score==3)
			GUI.Label (new Rect (450, 160, 400, 450), "Rang S ");

		if (GUI.Button (new Rect (400, 200, 100, 30), "Recommencer")) {
			m_Master.tpsT -= m_Master.tps;
			m_Master.coinT -= m_Master.coin;
			m_Master.mortsT -= m_Master.morts;
			m_Master.tps = 0;
			m_Master.coin = 0;
			m_Master.morts = 0;
			Debug.Log ("level"+m_Master.lvl);
            //Application.LoadLevel (1+m_Master.lvl);
            Application.LoadLevel(0);
        }

		if (GUI.Button (new Rect (500, 200, 100, 30), "Continuer")) {
			m_Master.tps = 0;
			m_Master.coin = 0;
			m_Master.morts = 0;
			m_Master.lvl++;
			Debug.Log ("level"+m_Master.lvl);
			Application.LoadLevel (1+m_Master.lvl);
		}

	}
}
