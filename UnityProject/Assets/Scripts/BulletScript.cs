using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

  
  [SerializeField]
  public int portalIndex;
  
  [SerializeField]
  public float shooterOrientation;

  GameObject ori;

	// Use this for initialization
	void Start () {
    ori = GameObject.Find("Origin");
	}
	
  void OnTriggerEnter(Collider other) {
    if (other.name.Contains("Wall")) {
      GenerateTerrainScript gts = ori.GetComponent("GenerateTerrainScript") as GenerateTerrainScript;
      Transform hittedThing = other.gameObject.transform;
      Transform currentPortalTransform = gts.Portals[portalIndex].transform;
      currentPortalTransform.position = hittedThing.position + (new Vector3(Mathf.Sin(shooterOrientation * Mathf.Deg2Rad), 0, Mathf.Cos(shooterOrientation * Mathf.Deg2Rad)));
      currentPortalTransform.rotation = Quaternion.AngleAxis(shooterOrientation, Vector3.forward);
      gts.Portals[portalIndex].SetActive(true);
    }
  }

}
