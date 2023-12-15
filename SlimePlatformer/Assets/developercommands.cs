using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class developercommands : MonoBehaviour
{
    private float spawnX;
    private float spawnY;

    public GameObject player;

    private int level;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt)) {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("TELEPORTED TO LVL 1");
                spawnX = (float)9.96;
                spawnY = (float)0.58;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("TELEPORTED TO LVL 2");
                spawnX = (float)27.21;
                spawnY = (float)-7.67;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("TELEPORTED TO LVL 3");
                spawnX = (float)16.23;
                spawnY = (float)-15.43;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log("TELEPORTED TO LVL 4");
                spawnX = (float)44;
                spawnY = (float)-12.56;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Debug.Log("TELEPORTED TO LVL 5");
                spawnX = (float)68.98;
                spawnY = (float)-19.31;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                Debug.Log("TELEPORTED TO LVL 6");
                spawnX = (float)83.1;
                spawnY = (float)-16.36;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                Debug.Log("TELEPORTED TO LVL 7");
                spawnX = (float)90;
                spawnY = (float)-17.37;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                Debug.Log("TELEPORTED TO LVL 8");
                spawnX = (float)96.98;
                spawnY = (float)-16.44;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                Debug.Log("TELEPORTED TO LVL 9");
                spawnX = (float)107.99;
                spawnY = (float)-8.53;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Debug.Log("TELEPORTED TO LVL 10");
                spawnX = (float)98.76;
                spawnY = (float)-0.15;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
        }
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift))
        {
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Debug.Log("TELEPORTED TO LVL 11");
                spawnX = (float)103.72;
                spawnY = (float)15.67;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Debug.Log("TELEPORTED TO LVL 12");
                spawnX = (float)95.5;
                spawnY = (float)24.51;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Debug.Log("TELEPORTED TO LVL 13");
                spawnX = (float)85.18;
                spawnY = (float)16.6;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Debug.Log("TELEPORTED TO LVL 14");
                spawnX = (float)72.85;
                spawnY = (float)24.51;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Debug.Log("TELEPORTED TO LVL 15");
                spawnX = (float)70.45;
                spawnY = (float)-0.47;
                player.transform.position = new Vector3(spawnX, spawnY, 0);
            }
        }
    }
}
