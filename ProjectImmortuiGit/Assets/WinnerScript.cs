using UnityEngine;
using System.Collections;

public class WinnerScript : MonoBehaviour {
    public float TimeLimitInSeconds;
    float curtime;
    public GameObject Player;
    MessageHub meshub;

    public GUIStyle WinnerStyle;
    public Vector3 spawnpoint;
    public GameObject spawngun;
    public int spawngunMagsLeft;
    bool GameDone = false;
    string label;
	// Use this for initialization
	void Start () {
        meshub = GameObject.Find("MessageHub").GetComponent<MessageHub>();
        meshub.addMesBool("gamedone", false);
        this.gameObject.GetComponent<Camera>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (curtime >= TimeLimitInSeconds) {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            this.gameObject.GetComponent<Camera>().enabled = true;
            label = "YOU HAVE WON THE GAME!";
            meshub.setMesBool("gamedone", true);
            GameDone = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        curtime += Time.deltaTime;
	}
    void OnGUI()
    {
        if (!meshub.IsMesBoolSet("gamedone")) return;
        if (!meshub.GetMesBool("gamedone")) {
            
            label = "Time Left: " + (TimeLimitInSeconds - ((int)curtime)).ToString();
            if (meshub.IsMesPosSet("zombiesLeft")) label = label + "\nZombiesLeft: " + meshub.GetMesPos("zombiesLeft").x.ToString();
            
            GUI.Box(new Rect(0, Screen.height - 70, label.Length*10+10, 70), label);

        }else { 
        if (GUI.Button(new Rect(Screen.width - 70, Screen.height - 70, 70, 70), "QUIT"))
        {
            Debug.Log("ghgfd");
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_WEBPLAYER
                     Application.OpenURL(webplayerQuitURL);
            #else
                     Application.Quit();
            #endif

        }
        if (GUI.Button(new Rect(0, Screen.height - 70, 100, 70), "PLAY AGAIN?"))
        {
            GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
            GameObject[] guns = GameObject.FindGameObjectsWithTag("Gun");
            foreach (var gd in zombies) Destroy(gd);
            foreach (var gd in guns) Destroy(gd);
            GameObject player = (GameObject)GameObject.Instantiate(Player,spawnpoint, Quaternion.identity);
            player.GetComponentInChildren<InstantiateGun>().CurrentGun = spawngun;
            player.GetComponentInChildren<InstantiateGun>().MagsLeft = spawngunMagsLeft;
            this.gameObject.GetComponent<Camera>().enabled = false;
            curtime = 0.0f;
            meshub.setMesBool("gamedone", false);
            meshub.setMesPos("zombiesLeft", meshub.GetMesPos("maxZombies"));

                GameDone = false;
        }
        }
    }
}
