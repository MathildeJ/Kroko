using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class GameMasterData
{
	public float m_Time = 0;
	public int m_Coin=0;
	public int m_Morts=0;

	public float m_Time_total = 0;
	public int m_Coin_total=0;
	public int m_Morts_total=0;

	public int m_Level=1;

}

public class GameMaster : MonoBehaviour {

	public GameMasterData m_Data = new GameMasterData();


	static public GameMaster m_Instance = null;

	public int coin{
		get { return m_Data.m_Coin; }
		set { m_Data.m_Coin = value; }
	}

	public int morts{
		get { return m_Data.m_Morts; }
		set { m_Data.m_Morts = value; }
	}

	public float tps{
		get { return m_Data.m_Time; }
		set { m_Data.m_Time = value; }
	}

	public int coinT{
		get { return m_Data.m_Coin_total; }
		set { m_Data.m_Coin_total = value; }
	}

	public int mortsT{
		get { return m_Data.m_Morts_total; }
		set { m_Data.m_Morts_total = value; }
	}

	public float tpsT{
		get { return m_Data.m_Time_total; }
		set { m_Data.m_Time_total = value; }
	}

	public int lvl{
		get { return m_Data.m_Level; }
		set { m_Data.m_Level = value; }
	}

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad (this);
		if (m_Instance == null) {
			m_Instance = this;
		} else {
			Destroy (this);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/gamemaster.dat",FileMode.Create);
		bf.Serialize (file, m_Data);
		file.Close ();
	}

	public void Load()
	{
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/gamemaster.dat", FileMode.Open);
		m_Data = (GameMasterData)bf.Deserialize (file);
		file.Close ();
	}

	void OnGUI()
	{/*
		if (GUI.Button (new Rect (200, 0, 100, 30), "Save")) {
			Save();
		}

		if (GUI.Button (new Rect (200, 40, 100, 30), "Load")) {
			Load();
		}*/
	}
}
