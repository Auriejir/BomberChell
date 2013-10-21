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
  private float _rotateSpeed = 45;
  
  public float RotateSpeed {
    get{return _rotateSpeed;}
    set{_rotateSpeed = Mathf.Clamp(value,0,90);}
  }

  private int[] position = new int[2];
  public int[] Position {
      get { return position; }
      set { position = value; }
  }
  
  private Transform form;
  public Transform Bomb;
  
  void move(char axis,float value){
    switch(axis) {
      case 'x':
        form.position += Vector3.left * value * Time.deltaTime;
      break;
      case 'z':
        form.position += Vector3.up * value * Time.deltaTime;
      break;
      case 'y':
        form.position += Vector3.forward * value * Time.deltaTime;
      break;
    }
  }
  
	// Use this for initialization
	void Start () {
    form = this.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {
        int x = (int)form.position.x;
        int y = (int)form.position.y;
        Position[0] = x;
        Position[1] = y;

    if(Input.GetKey(KeyCode.Q)){
      move('x',_speed);
    }
    if(Input.GetKey(KeyCode.D)){
      move('x',_speed * -1);
    }
    if(Input.GetKey(KeyCode.Z)){
      move('y',_speed);
    }
    if(Input.GetKey(KeyCode.S)){
      move('y',_speed * -1);
    }
    if(Input.GetKeyDown(KeyCode.A)){
      //move('z',_speed);
      Speed += 1;
    }
    if(Input.GetKeyDown(KeyCode.E)){
      //move('z',_speed * -1);
      Speed -= 1;
    }
    if(Input.GetKeyDown(KeyCode.Space)){
      Instantiate(Bomb, new Vector3(Mathf.Round(form.position.x), 0.25f, Mathf.Round(form.position.z)), Quaternion.identity);
      
    }
    if(Input.GetKey(KeyCode.LeftArrow)){
      form.Rotate(Vector3.down * RotateSpeed * Time.deltaTime);
    }
    if(Input.GetKey(KeyCode.RightArrow)){
      form.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);
    }
    if(Input.GetKey(KeyCode.UpArrow)){
      form.Rotate(Vector3.left * RotateSpeed * Time.deltaTime);
    }
    if(Input.GetKey(KeyCode.DownArrow)){
      form.Rotate(Vector3.right * RotateSpeed * Time.deltaTime);
    }
	}
  
}
