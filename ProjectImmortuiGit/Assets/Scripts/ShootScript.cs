using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]

public class ShootScript : MonoBehaviour {
    RaycastHit hitinfo;
    public Vector3 orgPos;
    bool IsAiming = false;
    public GameObject cam;
    public GameObject explosion;
    public GameObject explosionPosGO;
    public int Damage = 10;
    public float FireRate = 0.01f;
    public int gunid;
    public float RecoilAngle = 45.0f;
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
    public float AimDeltaY = 0.1f;
    public float AimDeltaZ = 0f;
    public float PercentOfTimeRecoilGoingUp;
    [HideInInspector]
    public bool IsShotgun;
    public float camRecoil;
    [HideInInspector]
    public float AngleBetweenShots;
    [HideInInspector]
    public int AmountOfShots;
    List<GameObject> muzzleflashes = new List<GameObject>();
    private float recoil = 0.0f;
    private float maxRecoil_x = -20f;
    private float maxRecoil_y = 0f;
    private float recoilSpeed = 2f;

    public void StartRecoil(float recoilParam, float maxRecoil_xParam, float recoilSpeedParam)
    {
        // in seconds
        recoil = recoilParam;
        maxRecoil_x = maxRecoil_xParam;
        recoilSpeed = recoilSpeedParam;
        maxRecoil_y = Random.Range(-maxRecoil_xParam, maxRecoil_xParam);
    }

    void recoiling()
    {
        if (recoil > 0f)
        {
            Quaternion maxRecoil = Quaternion.Euler(maxRecoil_x,  maxRecoil_y, 0f);
            Quaternion maxRecoilCam = Quaternion.Euler(-camRecoil,  0f, 0f);
            // Dampen towards the target rotation
            cam.transform.localRotation = Quaternion.Slerp(cam.transform.localRotation, maxRecoilCam, Time.deltaTime * (camRecoil*Mathf.PI/180)/(FireRate * PercentOfTimeRecoilGoingUp));
            transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, Time.deltaTime * (RecoilAngle*Mathf.PI/180)/(FireRate* PercentOfTimeRecoilGoingUp));
            recoil -= Time.deltaTime;
        }   
        else
        {
            recoil = 0f;
            // Dampen towards the target rotation
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * (RecoilAngle * Mathf.PI / 180) / (FireRate/2));
        }
    }
    void Awake() {
        gunid = curgunid + 1;
        curgunid++;
    }
    // Use this for initialization
    void Start () {
        maxRecoil_x = -RecoilAngle;
        curAmmo = MagSize;
        curtime = 0.0f;
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
        recoiling();

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
                if (curtime >= FireRate)
                {
                    if (Input.GetMouseButton(0) )
                    {

                        ShootShotgun();

                        curAmmo--;
                        curtime = 0.0f;
                        //Debug.Log(hitinfo.collider.name);
                        Recoil(curtime);
                    }
                }
                else {
                    

                }

            }
            else
            {
                if (curtime >= FireRate)
                {
                    if (Input.GetMouseButton(0))
                    {

                        ShootBullet();
                        Recoil(curtime);
                        //Debug.Log(hitinfo.collider.name);
                    }
                }
                else {

                }
            }
        }
        else
        {
            if (IsShotgun)
            {
                if (curtime >= FireRate)
                {
                    if (Input.GetMouseButtonDown(0) && curtime >= FireRate)
                    {

                        ShootShotgun();
                        Recoil(curtime);
                        curAmmo--;
                        curtime = 0.0f;
                        //Debug.Log(hitinfo.collider.name);
                    }
                }
                else {

                }
            }
            else
            {
                if (curtime >= FireRate)
                {
                    if (Input.GetMouseButtonDown(0) && curtime >= FireRate)
                    {

                        ShootBullet();
                        //Debug.Log(hitinfo.collider.name);
                        Recoil(curtime);

                    }
                }
                else {
                }
            }
        }


    }//End of Shoot
    void ShootBullet() {
       
        if(!IsAiming)
            Physics.Raycast(explosionPosGO.transform.position, explosionPosGO.transform.forward, out hitinfo, 10000.0f, ~(1 << 8));
        else
            Physics.Raycast(cam.transform.position, cam.transform.forward, out hitinfo, 10000.0f, ~(1 << 8));
        muzzleflashes.Add((GameObject)GameObject.Instantiate(explosion, explosionPosGO.transform.position, explosionPosGO.transform.rotation));
        if (hitinfo.collider != null) HitHandling(hitinfo);
        curtime = 0.0f;
        curAmmo--;
    }
    void Aim() {
        if (Input.GetMouseButton(1))
        {
            IsAiming = true;
            Vector3 transpos = Vector3.zero;
            transpos.x = -this.transform.parent.localPosition.x;
            transpos.y = AimDeltaY;
            transpos.z = AimDeltaZ;
            this.transform.localPosition = transpos;
        }
        else
        {
            IsAiming = false;

            this.transform.localPosition = orgPos;

        }
    }//End of Aim
    void Recoil(float fcurtime) {
        /* Vector3 eAngleT;
         if (curtime < FireRate/2)
         {
             if (this.transform.localEulerAngles.x <= RecoilAngle) {
                 eAngleT = this.transform.localEulerAngles;
                 eAngleT.x =  RecoilAngle / (FireRate / 2) * curtime;
                 this.transform.localEulerAngles = eAngleT;
             }
         }
         else {
             if (this.transform.localEulerAngles.x >= 0) {
                 eAngleT = this.transform.localEulerAngles;
                 eAngleT.x = RecoilAngle-( RecoilAngle / (FireRate / 2) * curtime);
                 this.transform.localEulerAngles = eAngleT;
             }

             Debug.Log("Dfg");
         }
         */
        recoil += FireRate * PercentOfTimeRecoilGoingUp;
    }
}

#if UNITY_EDITOR

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
#endif
