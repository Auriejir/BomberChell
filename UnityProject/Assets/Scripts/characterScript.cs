using UnityEngine;
using System.Collections;

public class characterScript : MonoBehaviour {

  [SerializeField]
  private float _speed = 2;

  public float Speed {
    get { return _speed; }
    set { _speed = Mathf.Clamp(value, 0, 40); }
  }

  [SerializeField]
  private float _orientation = 0;

  public float Orientation {
    get { return _orientation; }
    set { _orientation = Mathf.Clamp(value, 0, 360); }
  }

  private int playerNumber;

  private int[] position = new int[2];
  public int[] Position {
    get { return position; }
    set { position = value; }
  }

  private Transform form;
  public Transform Bomb;
  public GameObject Bullet;
  private bool[] keys = { false, false, false, false, false, false, false }; //Z,Q,S,D,SPACE,A,E

  private GameObject origin;
  private GenerateTerrainScript generateTerrainScript;
  private ArrayList bombp;

  private NetworkView _myNetworkView = null;

  // Use this for initialization
  void Start() {
    form = this.gameObject.transform;
    Position[0] = (int)Mathf.Round(form.position.x);
    Position[1] = (int)Mathf.Round(form.position.z);
    origin = GameObject.Find("Origin");
    generateTerrainScript = origin.GetComponent<GenerateTerrainScript>();
    _myNetworkView = this.gameObject.GetComponent<NetworkView>();
    if (form.position.x == 1 && form.position.z == 1) {
      if (GameObject.Find("Player1")) DestroyObject(gameObject);
      else gameObject.name = "Player1";
    }
    if (form.position.x == generateTerrainScript.TerrainX - 2 && form.position.z == generateTerrainScript.TerrainZ - 2) {
      if (GameObject.Find("Player2")) DestroyObject(gameObject);
      else gameObject.name = "Player2";
    }
    if (form.position.x == 1 && form.position.z == generateTerrainScript.TerrainZ - 2) {
      if (GameObject.Find("Player3")) DestroyObject(gameObject);
      else gameObject.name = "Player3";
    }
    if (form.position.x == generateTerrainScript.TerrainX - 2 && form.position.z == 1) {
      if (GameObject.Find("Player4")) DestroyObject(gameObject);
      else gameObject.name = "Player4";
    }
    playerNumber = (int)this.gameObject.name[this.gameObject.name.Length - 1] - 48;
  }

  // Update is called once per frame
  void Update() {
    if(Input.GetKeyDown(KeyCode.Z)) keys[0] = true;
    if(Input.GetKeyUp(KeyCode.Z)) keys[0] = false;
    if(Input.GetKeyDown(KeyCode.Q)) keys[2] = true;
    if (Input.GetKeyUp(KeyCode.Q)) keys[2] = false;
    if (Input.GetKeyDown(KeyCode.S)) keys[1] = true;
    if (Input.GetKeyUp(KeyCode.S)) keys[1] = false;
    if (Input.GetKeyDown(KeyCode.D)) keys[3] = true;
    if (Input.GetKeyUp(KeyCode.D)) keys[3] = false;
    if (Input.GetKeyDown(KeyCode.Space)) keys[4] = true;
    if (Input.GetKeyUp(KeyCode.Space)) keys[4] = false;
    if (Input.GetKeyDown(KeyCode.A)) keys[5] = true;
    if (Input.GetKeyUp(KeyCode.A)) keys[5] = false;
    if (Input.GetKeyDown(KeyCode.E)) keys[6] = true;
    if (Input.GetKeyUp(KeyCode.E)) keys[6] = false;
  }
    
  void FixedUpdate() {
    if (networkView.isMine) {
      Position[0] = (int)Mathf.Round(form.position.x);
      Position[1] = (int)Mathf.Round(form.position.z);
      if (keys[0] || keys[1] || keys[2] || keys[3]) {
        var resultVector = Vector3.zero;
        if (keys[0]) resultVector += Vector3.forward;
        if (keys[1]) resultVector += Vector3.back;
        if (keys[2]) resultVector += Vector3.left;
        if (keys[3]) resultVector += Vector3.right;
        if (resultVector != Vector3.zero) _myNetworkView.RPC("move", RPCMode.Server, resultVector);
      }
      if (keys[4]) {_myNetworkView.RPC("dropBomb", RPCMode.Server, (int)Mathf.Round(form.position.x), (int)Mathf.Round(form.position.z)); keys[4] = false;}
      if (keys[5]) {_myNetworkView.RPC("shootPortal",RPCMode.Server,playerNumber * 2 - 2); keys[5]=false;}
      if (keys[6]) {_myNetworkView.RPC("shootPortal",RPCMode.Server,playerNumber * 2 - 1); keys[6]=false;}
    }
  }

  [RPC]
  void shootPortal(int newPortalIndex) {
    GameObject portalAmo = ((Transform)Instantiate(Bullet, new Vector3(form.position.x + Mathf.Sin(Orientation), 0.5f, form.position.z - Mathf.Cos(Orientation)), Quaternion.identity)).gameObject;
    Debug.Log(portalAmo);
    BulletScript portalAmoScript = (BulletScript)portalAmo.GetComponent("BulletScript");
    portalAmoScript.portalIndex = newPortalIndex;
    portalAmoScript.shooterOrientation = Orientation;
    Rigidbody portalAmoRB = portalAmo.GetComponent("Rigidbody") as Rigidbody;
    portalAmoRB.AddForce( Mathf.Sin(Orientation) * 10, 0, Mathf.Cos(Orientation) * 10, ForceMode.Impulse);
    if (Network.isServer) _myNetworkView.RPC("shootPortal", RPCMode.Others, newPortalIndex);
  }

  [RPC]
  void move(Vector3 resultVector) {
    form.position = form.position + resultVector * Speed * Time.deltaTime;
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
    if (Network.isServer) _myNetworkView.RPC("move", RPCMode.Others, resultVector);
  }

  [RPC]
  void dropBomb(int x, int y) {
    bombp = generateTerrainScript.BombPlace;
    string positionBomb = x + "," + y;
    print("test :" + positionBomb);
    bombp.Add(positionBomb);
    Network.Instantiate(Bomb, new Vector3(x, 0.75f, y), Quaternion.identity,0);
   // if (Network.isServer) _myNetworkView.RPC("dropBomb", RPCMode.Others, x, y);
  }
}


/* stuff removed...
if (networkView.isMine) {
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
_myNetworkView.RPC("dropBomb", RPCMode.Server, (int)Mathf.Round(form.position.x), (int)Mathf.Round(form.position.z));
}
}*/