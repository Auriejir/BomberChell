using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
    void Start() {
        GameObject origin = GameObject.Find("Origin");
        GenerateTerrainScript generateTerrainScript = origin.GetComponent<GenerateTerrainScript>();
        int sizeTerrainX = generateTerrainScript.TerrainX;
        int sizeTerrainZ = generateTerrainScript.TerrainZ;
        this.gameObject.transform.position = new Vector3(sizeTerrainX / 2, sizeTerrainZ, 0);
        this.gameObject.transform.rotation = Quaternion.Euler(65, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
