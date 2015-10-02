using UnityEngine;
using System.Collections;

public class ZombieSpawner : MonoBehaviour {

    public GameObject Terrain;
    public GameObject Zombie;
    MessageHub meshub;
    public float maxWait;
    public float minWait;
    public int MaxZombies;
    int curZombies;
    float curtime = 0.0f;
    float spawntime;
    // Use this for initialization
    void Start()
    {
        meshub = GameObject.Find("MessageHub").GetComponent<MessageHub>();
        meshub.setMesPos("maxZombies", new Vector3((float)MaxZombies, 0, 0));
   
        meshub.setMesPos("amountZombies", new Vector3(0.0f,0,0));
        meshub.setMesBool("zombiereset", false);
        spawntime = Random.Range(minWait, maxWait);
    }

    // Update is called once per frame
    void Update()
    {
        if (!meshub.IsMesPosSet("zombiesLeft"))  meshub.setMesPos("zombiesLeft", meshub.GetMesPos("maxZombies"));

        if (curZombies < MaxZombies)
        {
            if (curtime >= spawntime)
            {
                Vector3 randvec = new Vector3(Random.Range(Terrain.transform.localScale.x / 2, -Terrain.transform.localScale.x / 2),
                    0.5f,
                    Random.Range(Terrain.transform.localScale.z / 2, -Terrain.transform.localScale.z / 2));
                GameObject gunspawner = (GameObject)GameObject.Instantiate(Zombie, Terrain.transform.position + randvec, this.transform.rotation);
                spawntime = Random.Range(minWait, maxWait);
                curtime = 0.0f;
                curZombies++;
                Vector3 aZ = meshub.GetMesPos("amountZombies");
                aZ.x++;
                meshub.setMesPos("amountZombies", aZ);

            }
            
            curtime += Time.deltaTime;
        }
        else {
            if (meshub.GetMesBool("zombiereset")) {
                curZombies = 0;
                meshub.setMesBool("zombiereset", false);


            }
        }
        if (meshub.GetMesPos("zombiesLeft").x <= 0) { meshub.setMesBool("gamedone", true); }
            
    }
    void OnGUI() {
        GUIStyle sf = new GUIStyle();
      if(curZombies >= MaxZombies) GUI.Label(new Rect(Screen.width - 120, Screen.height - 70, 120, 120), "ZOMBIE MAX REACHED",sf);
    }
}
