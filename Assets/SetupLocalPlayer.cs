using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI; 

public class SetupLocalPlayer : NetworkBehaviour {
	public Text namePrefab;
	public Text nameLabel;
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

	}

}
