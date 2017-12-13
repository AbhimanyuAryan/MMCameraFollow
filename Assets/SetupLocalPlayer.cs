using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLocalPlayer : NetworkBehaviour {
	public Text namePrefab;
	public Text nameLabel;
	public Transform namePos;
	string textboxname = "";
	string colourboxname = ""; 
	[SyncVar (hook= "OnChangeName")] public string pName = "player";
	[SyncVar (hook= "OnChangeColour")] public string pColour = "#ffffff";

	void OnChangeColour(string n)
	{
		pColour = n;
		Renderer[] rends = GetComponentsInChildren<Renderer>(); 

		foreach(Renderer r in rends) 
		{
			if(r.gameObject.name == "BODY")
				r.material.SetColor("_Color", ColorFromHex(pColour)); 
		}
	}

	public void OnChangeName(string n)
	{
		// SyncVar part runs on the Client 
		pName = n;
		nameLabel.text = pName;
	}

	[Command] public void CmdChangeColour(string newColour)
	{
		pColour = newColour;
		Renderer[] rends = GetComponentsInChildren<Renderer>();

		foreach(Renderer r in rends)
		{
			if(r.gameObject.name == "BODY")
				r.material.SetColor("_Color", ColorFromHex(pColour)); 
		}
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

			colourboxname = GUI.TextField(new Rect(170, 15, 100, 25), colourboxname);
			if(GUI.Button(new Rect(275, 15, 35, 25), "Set"))
				CmdChangeColour(colourboxname); 	
		}
	}

	Color ColorFromHex(string hex) 
	{
		hex = hex.Replace("0x", "");
		hex = hex.Replace("#", "");
		byte a = 255;
		byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		if(hex.Length == 8)
		{
			a = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
		}
		return new Color32(r,g,b,a); 
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
		// determine if the object is inside the camera's viewing volume
		Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
		bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 &&
					screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
		
		// if it is on screen draw its label attached to is name position
		if(onScreen)
		{
			Vector3 nameLabelPos = Camera.main.WorldToScreenPoint(namePos.position);
			nameLabel.transform.position = nameLabelPos;
		}
		else // otherwise draw it WAY off the screen
			nameLabel.transform.position = new Vector3(-1000, -1000, 0); 
	}

	public void OnDestroy()
	{
		if(nameLabel != null)
			Destroy(nameLabel.gameObject);
	}
}
