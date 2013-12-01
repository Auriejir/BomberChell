using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {
 
  //private Transform thisPortal;
  private Transform linkedPortal;

  private int linkedPortalIndex;
	
  private GenerateTerrainScript gts;

  [SerializeField]
  private int _portalIndex;
  public int PortalIndex {
    get { return _portalIndex; }
    set { _portalIndex = Mathf.Clamp(value, 0, 9); }
  }

  // Use this for initialization
  void Start () {
    //thisPortal = this.gameObject.transform;
    linkedPortalIndex = PortalIndex + (1 - (2 * (PortalIndex % 2)));
    GameObject ori = GameObject.Find("Origin");
    gts = ori.GetComponent("GenerateTerrainScript") as GenerateTerrainScript;
  }
  
  void OnTriggerEnter(Collider other) {
    if (gts.Portals[linkedPortalIndex].activeInHierarchy) {
      if (linkedPortal==null) linkedPortal = gts.Portals[linkedPortalIndex].gameObject.transform;
      other.gameObject.transform.position = new Vector3((int)Mathf.Round(linkedPortal.position.x), 0, (int)Mathf.Round(linkedPortal.position.z));
    }
  }

}