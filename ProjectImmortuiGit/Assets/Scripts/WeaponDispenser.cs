using UnityEngine;
using System.Collections;

public class WeaponDispenser : MonoBehaviour {
    public GameObject Weapon;
    GameObject curgun = null;
    GameObject oldgun = null;
    public int MagsLeft = 10;
    public float Timeout;
    float curtime;
    // Use this for initialization
    void Start () {
	    
	}
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("error");
        if (other.CompareTag("Player")) {
            if (Weapon != other.gameObject.GetComponentInChildren<InstantiateGun>().CurrentGun)
            {
                other.gameObject.GetComponentInChildren<InstantiateGun>().MagsLeft = MagsLeft;
                other.gameObject.GetComponentInChildren<InstantiateGun>().CurrentGun = Weapon;
            }
            else {
                other.gameObject.GetComponentInChildren<ShootScript>().MagsLeft = MagsLeft;
            }
                
            
            Destroy(oldgun);
            Destroy(this.gameObject);   
        }
        
    }
    // Update is called once per frame
    void Update () {
        if (curgun != Weapon)
        {
            Destroy(oldgun);
            oldgun = (GameObject)GameObject.Instantiate(Weapon, this.transform.position, this.transform.rotation);
            oldgun.transform.parent = this.gameObject.transform;
            Destroy(oldgun.GetComponent<ShootScript>());
            curgun = Weapon;
        }
        if (curtime >= Timeout) Destroy(this.gameObject);
        curtime += Time.deltaTime;
    }
}
