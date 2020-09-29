using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colourChange : MonoBehaviour
{
    //GridLayout gridLayout;
    Grid gridLayout;

    // Start is called before the first frame update
    void Start()
    {
        //gridLayout = transform.parent.GetComponentInParent<GridLayout>();
    }

    // Update is called once per frame
    void Update()
    {
        changeTileColour();
    }

    void changeTileColour()
    {
        //if the character is on top of a tile (specific location), colour in that tile.
        //Vector3Int tileToColour = gridLayout.WorldToCell(GameObject.Find("Character").transform.position);
        
        //Tile.changecolour(tileToColour);

        //if (GameObject.Find("Character").transform.position.x == )

    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("Collision with character");
    //    var pos = GameObject.Find("Character").transform.position; //Gets the position of the black square (character)
    //}
}
