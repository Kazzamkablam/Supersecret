using UnityEngine;
using System.Collections;
using System;

public class Listinfo : MonoBehaviour {
    public static Listinfo linfo;

    [Serializable]
    public class missions
    {
        public float e_reward;
        public int type;
        public GameObject target;
 
        public string text;
        public string title;
        public bool completed = false;
        // public missions(float rew, int typ, GameObject tar, string txt)
        // {
        //     e_reward = rew;
        //     typ = type;
        //     target = tar;
        //     text = txt;
        // }


    }

    [Serializable]
    public class sector_station
    {
        public int owner;
        public int type;
        public string name;
        public float xpos;
        public float ypos;
        public float zpos;
        public float xrot;
        public float yrot;
        public float zrot;
    }

    [Serializable]
    public class sector_fleet
    {
        public int owner;
        public float size;
        public int[] ships;
        public int[] number;
        public int[] ship_id;
        public float density = 100;
        public float spawndist = 500;
        public string name;
        public float xpos;
        public float ypos;
        public float zpos;
        public float xrot;
        public float yrot;
        public float zrot;
    }

    [Serializable]
    public class sector_stellar
    {
//        public int owner;
        public int type;
 
        public string name;
        public float xpos;
        public float ypos;
        public float zpos;
        public float xrot;
        public float yrot;
        public float zrot;
    }

    [Serializable]
    public class sector_jumpgate
    {
        //        public int owner;
        public int destination;

        public string name;
        public float xpos;
        public float ypos;
        public float zpos;
        public float xrot;
        public float yrot;
        public float zrot;
        public float player_xpos; // destination in sector where player will drop out of warp.
        public float player_ypos;
        public float player_zpos;
        public float player_xrot;
        public float player_yrot;
        public float player_zrot;
    }

    [Serializable]
    public class sector_asteroid_field
    {
        public float size;
        public float asteroids;
        public float density = 100;
        public float spawndist = 45000;
        public float avgsize = 100;
        public float xpos;
        public float ypos;
        public float zpos;
        public float xrot;
        public float yrot;
        public float zrot;
    }

    [Serializable]
    public class sector
    {
        public sector_station[] sector_stations;
        public sector_fleet[] sector_fleets;
        public sector_asteroid_field[] sector_asteroid_fields;
        public sector_stellar[] sector_stellars;
        public sector_jumpgate[] sector_jumpgates;
    }

    // Use this for initialization
    public GameObject hudtext;
    public GameObject inter_warp;
    public GameObject mission_target;
    public string[] partnames;
    public GameObject[] fractured;
    public GameObject[] asteroids;
    public GameObject[] aftparts;
    public GameObject[] wingparts;
    public GameObject[] noseparts;
    public GameObject[] noseexpparts;
    public GameObject[] wingexpparts;
    public GameObject[] aftexpparts;
    public GameObject[] hullexpparts;
    public GameObject[] explosions;
    public GameObject[] stations;
    public GameObject[] ships;
    public GameObject[] stellar_objects;
    public Material[] color_variants;
    public GameObject player;
    public GameObject fleet;
    public GameObject asteroid_field;
    public int maxships = 25;
    public GameObject curship;
    public GameObject curcockpit;
    public GameObject tablet;
    public GameObject target_box;
    public missions[] mission;

    public sector[] sectors;
    public SteamVR_TrackedObject trackedObj;
    public SteamVR_TrackedObject trackedObj2;
    public Objective_tracker tracker;

    void Awake()
    {

   

        
        if (linfo == null)
        {
            DontDestroyOnLoad(gameObject);
            linfo = this;
        }
        else if (linfo != this)
        {
            Destroy(gameObject);
        }
    }
    void Start () {
        linfo.partnames = partnames;
        linfo.aftparts = aftparts;
        linfo.wingparts = wingparts;
        linfo.noseparts = noseparts;
        linfo.noseexpparts = noseexpparts;
        linfo.wingexpparts = wingexpparts;
        linfo.aftexpparts = aftexpparts;
        linfo.hullexpparts = hullexpparts;
        linfo.explosions = explosions;
        linfo.player = player;
        linfo.fractured = fractured;
        linfo.curship = curship;
        linfo.target_box = target_box;
        linfo.hudtext = hudtext;
        linfo.tablet = tablet;
        linfo.ships = ships;
    //    Changesector(0); // for testing
	}

    public void Changesector(int sector)
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.tag == "Ship" || go.tag == "Poi" || go.tag == "Station" || go.tag == "SectorObject" || go.tag == "Asteroid")
            {
                if (go != curship)
                    Destroy(go.gameObject);
            }
        }
        foreach (sector_station stat in sectors[sector].sector_stations)
        {

            GameObject go;

            go = Instantiate(stations[stat.type], new Vector3(stat.xpos, stat.ypos, stat.zpos), Quaternion.Euler(stat.xrot, stat.yrot, stat.zrot)) as GameObject;
            if (stat.type == 4) mission[0].target = go; // required for tutorial mission to point to right object
            if (stat.type == 5) mission[1].target = go; //warp tutorial
        }

        foreach (sector_fleet ship in sectors[sector].sector_fleets)
        {

            GameObject go;

            go = Instantiate(fleet, new Vector3(ship.xpos, ship.ypos, ship.zpos), Quaternion.Euler(ship.xrot, ship.yrot, ship.zrot)) as GameObject;         
                for (int i = 0; i < ship.ships.Length; i++)
            {
                go.GetComponent<Fleet>().ship_id[i] = ships[ship.ships[i]];
                go.GetComponent<Fleet>().ships[i] = ship.number[i];
                go.GetComponent<Fleet>().density = ship.density;
                go.GetComponent<Fleet>().spawndist = ship.spawndist;
                go.GetComponent<Fleet>().size = ship.size; //radius which to spawn fleet.
                go.GetComponent<Fleet>().enabled = true;
            }
        }

        foreach (sector_asteroid_field asteroid in sectors[sector].sector_asteroid_fields)
        {

            GameObject go;

            go = Instantiate(asteroid_field, new Vector3(asteroid.xpos, asteroid.ypos, asteroid.zpos), Quaternion.Euler(asteroid.xrot, asteroid.yrot, asteroid.zrot)) as GameObject;

                go.GetComponent<Asteroid_Field>().spawndist = asteroid.spawndist;
            go.GetComponent<Asteroid_Field>().size = asteroid.size;
            go.GetComponent<Asteroid_Field>().density = asteroid.density;
            go.GetComponent<Asteroid_Field>().avgsize = asteroid.avgsize;
            go.GetComponent<Asteroid_Field>().asteroids = asteroid.asteroids;

        }

        foreach (sector_stellar stellar in sectors[sector].sector_stellars)
        {

            GameObject go;

            go = Instantiate(stellar_objects[stellar.type], new Vector3(stellar.xpos, stellar.ypos, stellar.zpos), Quaternion.Euler(stellar.xrot, stellar.yrot, stellar.zrot)) as GameObject;
        }

        foreach (sector_jumpgate jgate in sectors[sector].sector_jumpgates)
        {

            GameObject go;

            go = Instantiate(stations[6], new Vector3(jgate.xpos, jgate.ypos, jgate.zpos), Quaternion.Euler(jgate.xrot, jgate.yrot, jgate.zrot)) as GameObject;
            go.GetComponent<Jumpgate_Think>().jumpdestination = jgate.destination;
            go.GetComponent<Jumpgate_Think>().player_xpos = jgate.player_xpos;
            go.GetComponent<Jumpgate_Think>().player_ypos = jgate.player_ypos;
            go.GetComponent<Jumpgate_Think>().player_zpos = jgate.player_zpos;
            go.GetComponent<Jumpgate_Think>().player_xrot = jgate.player_xrot;
            go.GetComponent<Jumpgate_Think>().player_yrot = jgate.player_yrot;
            go.GetComponent<Jumpgate_Think>().player_zrot = jgate.player_zrot;
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
