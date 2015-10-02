using UnityEngine;
using System.Collections;
enum AIStates {
Chase,
Wait,
Search,
Dead
};
public class AIScript : MonoBehaviour {
    NavMeshAgent navagent;
    MessageHub meshub;
    AIStates curstate = AIStates.Wait;
    RaycastHit hitinfo;
    public float MeleeRange;
    public int MeleeDamage;
    public float MeleeRate;
    public GameObject Explosion;
    int zombieskilled =0;
    float curtime;
    public int health = 100;
    public bool Standstill;
    [HideInInspector]
    public int fullhealth;    
    // Use this for initialization
    void Start () {
        meshub = GameObject.Find("MessageHub").GetComponent<MessageHub>();
        navagent = this.gameObject.GetComponent<NavMeshAgent>();
       
        fullhealth = health;
    }
    void Wait() {

    }
    void Chase() {

    }
    void Search() {
    }
    void Dead() {
        Vector3 aZ = meshub.GetMesPos("amountZombies");
        aZ.x--;
        meshub.setMesPos("amountZombies", aZ);
        Vector3 aZK = meshub.GetMesPos("zombiesLeft");
        aZK.x--;
        GameObject.Instantiate(Explosion, this.transform.position, this.transform.rotation);        
        meshub.setMesPos("zombiesLeft",aZK);
        Destroy(this.gameObject);
    }
    void UpdateState() {
        if (health < 1) curstate = AIStates.Dead;
    }
    // Update is called once per frame
    void Update () {

        if (Standstill) { }
        else
        {
            switch (curstate)
            {
                case AIStates.Wait:
                    Wait();
                    break;
                case AIStates.Search:
                    Search();
                    break;
                case AIStates.Chase:
                    Chase();
                    break;
                case AIStates.Dead:
                    Dead();
                    break;

                default: break;
            }
            UpdateState();
            if (meshub.IsMesPosSet("playerpos")) Physics.Raycast(this.transform.position, (meshub.GetMesPos("playerpos") - this.transform.position).normalized, out hitinfo, this.transform.localScale.x + MeleeRange);
            if (hitinfo.collider != null)
            {
                if (hitinfo.collider.CompareTag("Player") && curtime >= MeleeRate)
                {
                    hitinfo.collider.GetComponent<PlayerScript>().Health -= MeleeDamage;
                    curtime = 0.0f;
                }
            }
            if (meshub.IsMesPosSet("playerpos")) navagent.SetDestination(meshub.GetMesPos("playerpos"));

            curtime += Time.deltaTime;
        }
    }
}
