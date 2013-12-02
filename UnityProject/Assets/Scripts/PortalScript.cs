using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {
 
  private int thisPortalIndex = -1;
  private int linkedPortalIndex = -1;
  private Transform linkedPortal;

	
  private GenerateTerrainScript gts;

  [SerializeField]
  private int _portalIndex;
  public int PortalIndex {
    get { return _portalIndex; }
    set { _portalIndex = Mathf.Clamp(value, 0, 9); }
  }

  // Use this for initialization
  void Start () {
    //linkedPortalIndex = thisPortalIndex + (1 - (2 * (thisPortalIndex % 2)));
    GameObject ori = GameObject.Find("Origin");
    gts = ori.GetComponent("GenerateTerrainScript") as GenerateTerrainScript;
  }
  
  void OnTriggerEnter(Collider other) {
    if (thisPortalIndex < 0) thisPortalIndex = (int)this.gameObject.name[this.gameObject.name.Length - 1] - 48;
    if (linkedPortalIndex < 0) linkedPortalIndex = thisPortalIndex + (1 - (2 * (thisPortalIndex % 2)));
    if (gts.Portals[linkedPortalIndex].activeInHierarchy) {
      linkedPortal = gts.Portals[linkedPortalIndex].gameObject.transform;
      other.gameObject.transform.position = new Vector3((int)Mathf.Round(linkedPortal.position.x), 1, (int)Mathf.Round(linkedPortal.position.z));
    }
  }

}