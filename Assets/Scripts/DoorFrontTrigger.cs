using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFrontTrigger : MonoBehaviour
{

    public GameObject doorTopGO;
    public GameObject doorBotGO;
    public GameObject doorMidGO;

    public bool playerIsBehindDoor;

    public const string DOOR_FRONT_LAYER_NAME = "DoorFrontPlayer";
    public const string DOOR_BEHIND_LAYER_NAME = "DoorBehindPlayer";

    private SpriteRenderer doorTopSR;
    private SpriteRenderer doorBotSR;
    private SpriteRenderer doorMidSR;


    // Start is called before the first frame update
    void Start()
    {
        doorTopSR = doorTopGO.GetComponent<SpriteRenderer>();
        doorBotSR = doorBotGO.GetComponent<SpriteRenderer>();
        doorMidSR = doorMidGO.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(playerIsBehindDoor)
        {
            doorTopSR.sortingLayerName = DOOR_FRONT_LAYER_NAME;
            doorTopSR.sortingOrder = 1;
            doorMidSR.sortingLayerName = DOOR_FRONT_LAYER_NAME;
            doorMidSR.sortingOrder = 0;
            doorBotSR.sortingLayerName = DOOR_FRONT_LAYER_NAME;
            doorBotSR.sortingOrder = -1;
        }
        else if(!playerIsBehindDoor)
        {
            doorTopSR.sortingLayerName = DOOR_BEHIND_LAYER_NAME;
            doorTopSR.sortingOrder = 1;
            doorMidSR.sortingLayerName = DOOR_BEHIND_LAYER_NAME;
            doorMidSR.sortingOrder = 0;
            doorBotSR.sortingLayerName = DOOR_BEHIND_LAYER_NAME;
            doorBotSR.sortingOrder = -1;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
		if (col.gameObject.tag == "Player")
        {
			playerIsBehindDoor = true;
		}
	}

    void OnTriggerExit2D(Collider2D col)
    {
		if (col.gameObject.tag == "Player")
        {
			playerIsBehindDoor = false;
		}
	}
}
