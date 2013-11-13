using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject origin = GameObject.Find("Origin");
        GenerateTerrainScript generateTerrainScript = origin.GetComponent<GenerateTerrainScript>();
        int sizeTerrain = generateTerrainScript.TerrainSize;
        this.gameObject.transform.position = new Vector3(sizeTerrain/2, 15, 0);
        this.gameObject.transform.rotation = Quaternion.Euler(65, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
