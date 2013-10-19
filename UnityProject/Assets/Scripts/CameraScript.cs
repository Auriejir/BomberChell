using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.transform.position = new Vector3(6,13,-7);
        this.gameObject.transform.rotation = Quaternion.Euler(40, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
