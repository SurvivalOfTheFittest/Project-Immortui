using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
[System.Serializable]

public class ShootScript : MonoBehaviour {
    RaycastHit hitinfo;
    public Vector3 orgPos;
    public GameObject cam;
    public GameObject explosion;
    public GameObject explosionPosGO;
    public int Damage = 10;
    public float FireRate = 0.01f;
    public int gunid;
    static public int curgunid;
    public int MagSize = 6;
     int curAmmo;
    [HideInInspector]
    public int MagsLeft = 10;
    public int MaxMags = 10;
    public float ReloadTime;
   float reloadTime =0.0f;
    float curtime;
    public bool semiauto = false;
    string label1;
    string label2;
    public GUIStyle ReloadLabelStyle;
    public GUIStyle AmmoLabelStyle;

    [HideInInspector]
    public bool IsShotgun;

    [HideInInspector]
    public float AngleBetweenShots;
    [HideInInspector]
    public int AmountOfShots;
    List<GameObject> muzzleflashes = new List<GameObject>();
    void Awake() {
        gunid = curgunid + 1;
        curgunid++;
    }
    // Use this for initialization
    void Start () {

        curAmmo = MagSize;
        
        orgPos = this.transform.localPosition;
    }
    void HitHandling(RaycastHit fhitinfo) {
        AIScript aiscript = fhitinfo.collider.gameObject.GetComponent<AIScript>();
        if (aiscript != null)
        {
            aiscript.health -= Damage;
            if(fhitinfo.collider.gameObject.CompareTag("Zombie")) Debug.Log(aiscript.health);
            Color c = hitinfo.collider.gameObject.GetComponent<Renderer>().material.color;
            c.r += (float)Damage / (float)aiscript.fullhealth;
            c.g -= c.g <= 0.0f ? (float)Damage / (float)aiscript.fullhealth : (c.g = 0.0f);
            c.b -= c.b <= 0.0f ? (float)Damage / (float)aiscript.fullhealth : (c.b=0.0f);
            hitinfo.collider.gameObject.GetComponent<Renderer>().material.color = c; 

        }
        
    }
    // Update is called once per frame
    void Update() {
        
        if (reloadTime <= 0)
        {
            label2 = "NULL";
            if (curAmmo > 0)
            {
                Shoot(); 
            }
            else {
                if (Input.GetMouseButtonDown(0)) {

                    Reload();

                }
            }
            MagsLeft = Mathf.Max(0, MagsLeft);
            MagsLeft = Mathf.Min(MaxMags, MagsLeft);
            if (Input.GetKeyDown(KeyCode.R) && MagsLeft > 0 )
            {
                Reload();
            }
            muzzleflashes.RemoveAll(delegate (GameObject o) { return o == null; });
            foreach (GameObject mf in muzzleflashes)
            {
                if (mf != null)
                {
                    mf.transform.position = explosionPosGO.transform.position;
                }

            }

            curtime += Time.deltaTime;

            Aim();
        }
        else {

            reloadTime -= Time.deltaTime;
            label2 = "RELOADING...";
            this.transform.localPosition = orgPos;

        }
        label1 = curAmmo.ToString() + ":" + MagsLeft.ToString();
    }//End of Update
    void OnGUI()
    {
        if (label2 == "RELOADING...")
            GUI.Label(new Rect(10, 10, 150, 100), label2, ReloadLabelStyle);
        else
            GUI.Label(new Rect(10, 10, 150, 100), label1,AmmoLabelStyle);
    }//End of OnGui
    void Reload() {
        if (MagsLeft <= 0) return;
        if (curAmmo == MagSize) return;
        reloadTime = ReloadTime;
        curAmmo = MagSize;
        MagsLeft--;
    }//End of Reload
    void ShootShotgun() {
        for (int i = 1; i <= AmountOfShots; i++)
        {
            Ray shotray = new Ray(cam.transform.position,
                     Quaternion.AngleAxis(
                         (-(AmountOfShots * AngleBetweenShots / 2)) + (i * AngleBetweenShots),
                         cam.transform.up) * cam.transform.forward);
            
            Physics.Raycast(shotray,
                            out hitinfo,
                            10000.0f,
                            ~(1 << 8));
            GameObject explosiontemp = (GameObject)GameObject.Instantiate(explosion, explosionPosGO.transform.position, Quaternion.AngleAxis(
                                (-(AmountOfShots * AngleBetweenShots / 2)) + (i * AngleBetweenShots),
                                explosionPosGO.transform.up));
            muzzleflashes.Add(explosiontemp);
          
            LineRenderer tracin = explosiontemp.GetComponent<LineRenderer>();
            if (tracin != null) {
                tracin.SetPosition(0, explosiontemp.transform.position);
                if (hitinfo.collider != null)
                        tracin.SetPosition(1, hitinfo.point);
                else tracin.SetPosition(1, shotray.GetPoint(100.0f));
            }
            Debug.DrawRay(cam.transform.position,
                            Quaternion.AngleAxis(
                                (-(AmountOfShots * AngleBetweenShots / 2)) + (i * AngleBetweenShots),
                                cam.transform.up) * cam.transform.forward, Color.red, 10000.0f);
            if (hitinfo.collider != null) HitHandling(hitinfo);
        }
        
    }
    void Shoot() {
        if (!semiauto)
        {
            if (IsShotgun)
            {
                if (Input.GetMouseButton(0) && curtime >= FireRate)
                {

                    ShootShotgun();
                  
                    curAmmo--;
                    curtime = 0.0f;
                    //Debug.Log(hitinfo.collider.name);
                }
            }
            else
            {
                if (Input.GetMouseButton(0) && curtime >= FireRate)
                {

                    Physics.Raycast(cam.transform.position, cam.transform.forward, out hitinfo, 10000.0f, ~(1 << 8));
                    muzzleflashes.Add((GameObject)GameObject.Instantiate(explosion, explosionPosGO.transform.position, explosionPosGO.transform.rotation));
                    if (hitinfo.collider != null) HitHandling(hitinfo);
                    curAmmo--;
                    curtime = 0.0f;
                    //Debug.Log(hitinfo.collider.name);
                }
            }
        }
        else
        {
            if (IsShotgun)
            {
                if (Input.GetMouseButtonDown(0) && curtime >= FireRate)
                {

                    ShootShotgun();

                    curAmmo--;
                    curtime = 0.0f;
                    //Debug.Log(hitinfo.collider.name);
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && curtime >= FireRate)
                {

                    Physics.Raycast(cam.transform.position, cam.transform.forward, out hitinfo, 10000.0f, ~(1 << 8));
                    muzzleflashes.Add((GameObject)GameObject.Instantiate(explosion, explosionPosGO.transform.position, explosionPosGO.transform.rotation));
                    if (hitinfo.collider != null) HitHandling(hitinfo);
                    curtime = 0.0f;
                    curAmmo--;
                    //Debug.Log(hitinfo.collider.name);
                }
            }
        }


    }//End of Shoot
    void Aim() {
        if (Input.GetMouseButton(1))
        {
            Vector3 transpos = Vector3.zero;
            transpos.x = -this.transform.parent.localPosition.x;
            transpos.y = 0.1f;
            this.transform.localPosition = transpos;
        }
        else
        {

            this.transform.localPosition = orgPos;

        }
    }//End of Aim
}


[CustomEditor(typeof(ShootScript))]
[CanEditMultipleObjects]
public class PlayerEffectsEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ShootScript effectPlayer = (ShootScript)target;
      
        effectPlayer.IsShotgun = EditorGUILayout.Toggle("Is Shotgun", effectPlayer.IsShotgun);

        if (effectPlayer.IsShotgun)
        {
            effectPlayer.AngleBetweenShots = EditorGUILayout.FloatField(new GUIContent("Angle Between Shots"), effectPlayer.AngleBetweenShots);
            effectPlayer.AmountOfShots = EditorGUILayout.IntField(new GUIContent("Amount Of Shots"), effectPlayer.AmountOfShots);
        }

    }
}
