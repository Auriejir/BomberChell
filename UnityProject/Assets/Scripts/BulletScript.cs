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
      Transform hitThing = other.gameObject.transform;
      Transform currentPortalTransform = gts.Portals[portalIndex].transform;
      currentPortalTransform.position = hitThing.position + (new Vector3(Mathf.Sin(shooterOrientation * Mathf.Deg2Rad)/1.9f, 0f, -1 * Mathf.Cos(shooterOrientation * Mathf.Deg2Rad)/1.9f));
      currentPortalTransform.rotation = Quaternion.Euler(0, (shooterOrientation + 180) % 360, 0);
      gts.Portals[portalIndex].SetActive(true);
      Debug.Log("created portal " + portalIndex + " at " + gts.Portals[portalIndex].transform.position);
      Destroy(this.gameObject,0f);
    }
  }

}
