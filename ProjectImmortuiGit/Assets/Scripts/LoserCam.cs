using UnityEngine;
using System.Collections;

public class LoserCam : MonoBehaviour {
    public GUIStyle loserStyle;
    MessageHub meshub;
	// Use this for initialization
	void Start () {
        meshub = GameObject.Find("MessageHub").GetComponent<MessageHub>();

    }

    // Update is called once per frame
    void Update () {
	
	}
    void OnGUI() {
        if (!meshub.IsMesBoolSet("gamedone")) return;

        if (!meshub.GetMesBool("gamedone"))
        {
            Destroy(this.gameObject);
        }
            string label = "YOUR BRAIN WAS VERY TASTY!!";
        Vector2 textsize = loserStyle.CalcSize(new GUIContent(label));
        GUI.Label(new Rect(Screen.width/2 -textsize.x,Screen.height/2 - textsize.y,textsize.x,textsize.y),label,loserStyle);
       /* if (GUI.Button(new Rect(Screen.width - 70, Screen.height - 70, 70, 70), "QUIT")) {
            Debug.Log("ghgfd");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif

        }*/
    }
}
