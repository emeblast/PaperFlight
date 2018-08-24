﻿using UnityEngine;
using System.Collections;

public class RandomWeapon : MonoBehaviour 
{
	public GameObject[] WeaponPrefabs;
	public float waitTime = 5;
	MeshRenderer mRender;
  
	bool taken = false;
	float t = 0;
     public GameObject prefadWeaponPickUp = GameObject.FindGameObjectWithTag("randomWeapon");

	void Start()
	{
		mRender = GetComponent<MeshRenderer> ();
	}


	void Update()
	{

		//transform.position = new Vector3 (transform.position.x, Mathf.PingPong (Time.deltaTime, 2.0f), transform.position.z);

       
        //respawn if actor has been destroy, respawn item, with same position 
        // get position, set position.
		if(taken)
		{
			t += Time.deltaTime;
			if(t >= waitTime)
			{

              
                taken = false;
				GetComponent<NetworkView>().RPC("setEnable",RPCMode.AllBuffered,true);

			}
            
		}
    }

    void OnTriggerEnter(Collider other)
	{
		Debug.Log ("Player enter collider");//collider works
		if(other.tag == "Player")//if collides with the mesh tagged player
		{
			if(taken == false)
			{
                print("has been taken");
                taken = true;
				t = 0;
				int wpn = RandomPrefab();
                
				//respawn game object
				// get world postion
				GameObject weaponGenetor = Resources.Load("Prefab/randomWeapon")as GameObject;
				Vector3 weaponLocation = transform.position;
                Destroy(gameObject);


                //is respawning needs detla time, give time in between
                t+=Time.deltaTime;
                if (t >= waitTime)
                {
                    Instantiate(weaponGenetor, transform.position, transform.rotation);


                    //need to create sprite animation, with a GUI Layout
                    GameObject prefab = WeaponPrefabs[wpn];
                    Shooting Shoot = other.GetComponentInParent<Shooting>();
                    Shoot.Secondary = prefab;
                    //need to destroy game object

                    GetComponent<NetworkView>().RPC("setEnable", RPCMode.AllBuffered, false);

                }
                
			}
		}
	}

	int RandomPrefab()
	{
		int ran = Random.Range (0, WeaponPrefabs.Length);
		return ran;
	}

	void ChangeColor()
	{
		Color newcolor = new Color (Random.value, Random.value, Random.value, 1.0f);
		GetComponent<Renderer>().material.color = newcolor;
	}

	[RPC]
	void setEnable(bool state)
	{
		mRender.enabled = state;
	}
}
