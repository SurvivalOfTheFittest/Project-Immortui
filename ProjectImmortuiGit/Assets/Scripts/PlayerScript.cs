using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    MessageHub meshub;
    public int Health;
    public GameObject losercam;
    [HideInInspector]
    public int Fullhealth;
    public GUIStyle Healthbar;
    string label;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        meshub = GameObject.Find("MessageHub").GetComponent<MessageHub>();
        meshub.setMesPos("playerpos",this.transform.position);
        meshub.setMesBool("gamedone", false);
        meshub.setMesBool("lost", false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Health <= 0) {

            meshub.setMesBool("lost", true);
            Instantiate(losercam,new Vector3(0,20.0f,0),Quaternion.Euler(0,0,90));
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            meshub.setMesBool("gamedone", true);

            Destroy(this.gameObject);
        }
        meshub.setMesPos("playerpos", this.transform.position);
        
    }
    void OnGUI() {
        label = "Health: " + Health.ToString();
        //label = Input.mouseScrollDelta.ToString();
            GUI.Label(new Rect(Screen.width -Healthbar.CalcSize(new GUIContent(label)).x, 10, label.Length, 100), label,Healthbar);
    }
}
