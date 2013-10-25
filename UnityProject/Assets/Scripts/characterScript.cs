using UnityEngine;
using System.Collections;

public class characterScript : MonoBehaviour {

  [SerializeField]
  private float _speed = 2;
  
  public float Speed {
    get{return _speed;}
    set{_speed = Mathf.Clamp(value,0,40);}
  }
  
  [SerializeField]
  private float _orientation = 0;
  
  public float Orientation {
    get{return _orientation;}
    set{_orientation = Mathf.Clamp(value,0,360);}
  }
	
  private Transform form;
  public Transform Bomb;
  bool[] keys = {false,false,false,false};
  
  private NetworkView _myNetworkView = null;
  
	// Use this for initialization
	void Start () {
    GameObject origin = GameObject.Find("Origin");
    GenerateTerrainScript generateTerrainScript = origin.GetComponent<GenerateTerrainScript>();
    form = this.gameObject.transform;
    _myNetworkView = this.gameObject.GetComponent<NetworkView>();
    if (form.position.x == 1 && form.position.z == 1) {
      if (GameObject.Find("Player1")) DestroyObject(gameObject);
      else gameObject.name = "Player1";
    }
    if (form.position.x == generateTerrainScript.TerrainSize - 2 && form.position.z == generateTerrainScript.TerrainSize - 2) {
      if (GameObject.Find("Player2")) DestroyObject(gameObject);
      else gameObject.name = "Player2";
    }
    if (form.position.x == 1 && form.position.z == generateTerrainScript.TerrainSize - 2) {
      if (GameObject.Find("Player3")) DestroyObject(gameObject);
      else gameObject.name = "Player3";
    }
    if (form.position.x == generateTerrainScript.TerrainSize - 2 && form.position.z == 1) {
      if (GameObject.Find("Player4")) DestroyObject(gameObject);
      else gameObject.name = "Player4";
    }
  }
	
	// Update is called once per frame
	void Update () {
    
    if (networkView.isMine) {
      int x = (int)form.position.x;
      int y = (int)form.position.y;
      Position[0] = x;
      Position[1] = y;

      if (Input.GetKey(KeyCode.Q)) {
        _myNetworkView.RPC("move", RPCMode.Server, 1, _speed);
          //move('x',_speed);
      }
      if (Input.GetKey(KeyCode.D)) {
        _myNetworkView.RPC("move", RPCMode.Server, 1, _speed * -1);
          //move('x',_speed * -1);
      }
      if (Input.GetKey(KeyCode.Z)) {
        _myNetworkView.RPC("move", RPCMode.Server, 2, _speed);
          //move('y',_speed);
      }
      if (Input.GetKey(KeyCode.S)) {
        _myNetworkView.RPC("move", RPCMode.Server, 2, _speed * -1);
          //move('y',_speed * -1);
      }
      if (Input.GetKeyDown(KeyCode.Space)) {
        _myNetworkView.RPC("dropBomb", RPCMode.Server);
      }
    }
    /* stuff for move character... actualy working but not NETworking...
    if(Input.GetKeyDown(KeyCode.Z)){
      keys[0] = true;
    }
    if(Input.GetKeyUp(KeyCode.Z)){
      keys[0] = false;
    }
    if(Input.GetKeyDown(KeyCode.Q)){
      keys[2] = true;
    }
    if(Input.GetKeyUp(KeyCode.Q)){
      keys[2] = false;
    }
    if(Input.GetKeyDown(KeyCode.S)){
      keys[1] = true;
    }
    if(Input.GetKeyUp(KeyCode.S)){
      keys[1] = false;
    }
    if(Input.GetKeyDown(KeyCode.D)){
      keys[3] = true;
    }
    if(Input.GetKeyUp(KeyCode.D)){
      keys[3] = false;
    }
    if(Input.GetKeyDown(KeyCode.A)){
      
    }
    if(Input.GetKeyDown(KeyCode.E)){
      
    }
    
	}
  void FixedUpdate() {
    if (keys[0] || keys[1] || keys[2] || keys[3]) {
      var resultVector = Vector3.zero;
      if (keys[0]) resultVector += Vector3.forward;
      if (keys[1]) resultVector += Vector3.back;
      if (keys[2]) resultVector += Vector3.left;
      if (keys[3]) resultVector += Vector3.right;
      if (resultVector != Vector3.zero) {
        form.position += resultVector * Speed * Time.deltaTime;
        if (resultVector.x > 0) {
          if (resultVector.z > 0) Orientation = 45;
          else if (resultVector.z > 0) Orientation = 135;
          else Orientation = 90;
        }else if (resultVector.x > 0) {
          if (resultVector.z > 0) Orientation = 315;
          else if (resultVector.z > 0) Orientation = 225;
          else Orientation = 270;
        }else {
          if (resultVector.z > 0) Orientation = 0;
          else if (resultVector.z > 0) Orientation = 180;
        }
        form.rotation = Quaternion.Euler(0, Orientation, 0);
      }
    }*/
  }
  
  [RPC]
  void dropBomb() {
    Instantiate(Bomb, new Vector3((int)Mathf.Round(form.position.x), 0.25f, (int)Mathf.Round(form.position.z)), Quaternion.identity);
    if (Network.isServer) {
      _myNetworkView.RPC("dropBomb", RPCMode.Others);
    }
  }

  [RPC]
  void move(int axis, float value) {
    switch (axis) {
      case 1:
        form.position += Vector3.left * value * Time.deltaTime;
      break;
      case 2:
        form.position += Vector3.forward * value * Time.deltaTime;
      break;
    }
    if (Network.isServer) {
      _myNetworkView.RPC("move", RPCMode.Others, axis, value);
    }
  }
  
}
