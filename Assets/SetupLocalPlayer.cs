using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI; 

public class SetupLocalPlayer : NetworkBehaviour {
	public Text namePrefab;
	public Text nameLabel;
	public Transform namePos;
	string textboxname = "";
	[SyncVar (hook= "OnChangeName")] public string pName = "player";
	
	public void OnChangeName(string n)
	{
		// SyncVar part runs on the Client 
		pName = n;
		nameLabel.text = pName;
	}

	[Command] public void CmdChangeName(string newName)
	{
		// This part runs on server
		pName = newName;
		nameLabel.text = pName;
	}

	void OnGUI()
	{
		if(isLocalPlayer)
		{
			textboxname = GUI.TextField(new Rect(25, 15, 100, 25), textboxname);
			if(GUI.Button(new Rect(130, 15, 35, 25), "SET"))
				CmdChangeName(textboxname); 		
		}
	}

	void Start () 
	{
		if(isLocalPlayer)
		{
			GetComponent<PlayerController>().enabled = true;
			CameraFollow360.player = this.gameObject.transform;
		}
		else
		{
			GetComponent<PlayerController>().enabled = false;
		}

		GameObject canvas = GameObject.FindWithTag("MainCanvas"); 
		nameLabel = Instantiate(namePrefab, Vector3.zero, Quaternion.identity) as Text;
		nameLabel.transform.SetParent(canvas.transform);
	}

	void Update()
	{
		Vector3 nameLabelPos = Camera.main.WorldToScreenPoint(namePos.position);
		nameLabel.transform.position = nameLabelPos;
	}
}
