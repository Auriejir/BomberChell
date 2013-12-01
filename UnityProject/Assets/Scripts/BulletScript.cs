using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

  
  [SerializeField]
  public int portalIndex;
  
  [SerializeField]
  public float shooterOrientation;

  GenerateTerrainScript gts;

	// Use this for initialization
	void Start () {

	}
	
  void OnTriggerEnter(Collider other) {
    var hittedThing = other.gameObject.transform;
    var currentPortalTransform = gts.Portals[portalIndex].transform;
    currentPortalTransform.position = hittedThing.position + (new Vector3(Mathf.Sin(shooterOrientation), 0, Mathf.Cos(shooterOrientation)));
    currentPortalTransform.rotation = new Quaternion(0,shooterOrientation,0,0);
  }

}
