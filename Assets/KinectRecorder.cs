using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class KinectRecorder : MonoBehaviour {
	
	public int TimeDisplayPrecision;
	
	bool recording = false;
	float RecordedTime;

	StreamWriter sw;
	void OnGUI()
	{
		if (recording) 
		{
			if (GUI.Button(new Rect(10, 10, 150, 100), 
			               "Stop : "+
			               (((int)(RecordedTime*Mathf.Pow(10,TimeDisplayPrecision)))/Mathf.Pow(10,TimeDisplayPrecision))))
			{
				StopRecord();
			}
		}
		else
		{
			if (GUI.Button(new Rect(10, 10, 150, 100), "Record"))
			{
				StartRecord();
			}
		}
	}

	private void StopRecord()
	{
		recording = false;
		
		sw.Close();
		sw = null;
		
	}
	
	private void StartRecord()
	{
		string now = System.DateTime.Now.ToString();
		
		sw = new StreamWriter("Assets/Records/"+"d"+now.Replace("/","_").Replace(" ","_t").Replace(":","_")+".csv",false);
		sw.WriteLine("RecordDate:"+now);
		
		string head = "";
		
		head += "time,";
		
		foreach(Transform t in cubes)
		{
			head+= t.name + "X,";
			head+= t.name + "Y,";
			head+= t.name + "Z,";
		}
		
		sw.WriteLine(head);
		
		
		recording = true;
		RecordedTime = 0;
	}
	
	
	private List<Transform> cubes = new List<Transform>();

	void OnApplicationQuit() 
	{
		if(sw!=null)sw.Close();

		
		recording = false;
		sw = null;
	}

	void OnDestroy()
	{
		if(sw!=null)sw.Close();
		
		recording = false;
		sw = null;
	}

	void OnDisable()
	{
		if(sw!=null)sw.Close();


		recording = false;
		sw = null;
	}

	// Use this for initialization
	void Start () 
	{
		Transform[] ts = transform.GetComponentsInChildren<Transform>();

		foreach (Transform t in ts) {

			if(t.name!="Cubeman")
			{
				t.gameObject.SetActive(false);
				cubes.Add(t);	
			}
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			if(recording)
			{
				StopRecord();
			}
			else
			{
				StartRecord();
			}
		}


		if (recording) 
		{
			RecordedTime += Time.deltaTime;

			string line = "";
			
			line += RecordedTime+",";
			
			foreach(Transform t in cubes)
			{
				Vector3 position = t.localPosition;

				if(t.gameObject.activeSelf&&position!=Vector3.zero)
				{
				
				
				
				line+= position.x+",";
				line+= position.y+",";
				line+= position.z+",";
				}
				else
				{
					line+= "NaN,";
					line+= "NaN,";
					line+= "NaN,";
				}
			}
			
			sw.WriteLine(line);
			
		}
	
	}
}
