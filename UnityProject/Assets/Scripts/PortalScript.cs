using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {

  [SerializeField]
  private char _orientation;
  public char Orientation {
    get{return _orientation;}
    set{_orientation = value;}
  }
  [SerializeField]
  private GameObject _owner;
  public GameObject Owner {
    get{return _owner;}
    set{_owner = value;}
  }
  [SerializeField]
  private int _type;
  public int Type {
    get{return _type;}
    set{_type = value;}
  }
  
	// Use this for initialization
	void Start () {
    
	}
	
	// Update is called once per frame
	void Update () {
    
	}
}
