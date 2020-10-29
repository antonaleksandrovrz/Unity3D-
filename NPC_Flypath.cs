using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class NPC_Flypath : MonoBehaviour
{
    public GameObject interact;
    public GameObject flydestination;
    public bool isInteracting = false;
    public GameObject player;
    public GameObject dragon;

    public Transform[] destinations;

    private void Update()
    {
        if (isInteracting)
        {
            if (Input.GetKeyDown(KeyCode.E) && interact.active)
            {
                player.GetComponent<FirstPersonController>().enabled = false;
                interact.active = false;
                flydestination.active = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            else if(Input.GetKeyDown(KeyCode.E) && !interact.active)
            {
                player.GetComponent<FirstPersonController>().enabled = true;
                interact.active = true;
                flydestination.active = false;
                Cursor.visible = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player = other.gameObject;
            interact.active = true;
            isInteracting = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interact.active = false;
            isInteracting = false;
        }
    }

    public void SendToFly(int startEnd)
    {
        player.gameObject.SetActive(false);
        dragon.SetActive(true);
        flydestination.active = false;
        dragon.transform.position = transform.position;
        List<Transform> newDestination = new List<Transform>();
        Transform target = null;
        Debug.Log(startEnd / 10 + " i " + startEnd % 10);
        foreach (var item in destinations)
        {
            if(item.name.StartsWith((startEnd / 10).ToString()) || item.transform.name == (startEnd % 10).ToString())
            {
                newDestination.Add(item.transform);
                if (item.transform.name == (startEnd % 10).ToString()) target = item;
            }

        }

        newDestination = newDestination.OrderBy(x => Vector2.Distance(this.transform.position, x.transform.position)).ToList();

        dragon.GetComponent<FlyPath>().target = target;
        dragon.GetComponent<FlyPath>().flyPathPositions = newDestination;
    }
}
