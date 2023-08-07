using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCollisionDetection : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] string thisGameObjectName;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        thisGameObjectName = this.name;
    }

    [System.Obsolete]
    private void OnCollisionEnter(Collision collision)
    {
        if(collision != null)
        {
            if(collision.collider.tag == "Player")
            {
                Renderer render = gameObject.GetComponent<Renderer>();
                Material material = render.material;
                if(material.color != Color.red || !transform.GetChild(0).gameObject.active)
                {
                    //material.color = Color.green;
                    TilePosition tile = gameManager.GetCoordinates(thisGameObjectName);
                    int row = tile.row;
                    int col = tile.col;
                    gameManager.SetPlayerPosition(row, col);
                    gameManager.TileMarker();
                }
                else
                {
                    if(material.color != Color.red || transform.GetChild(0).gameObject.active)
                    {
                        Debug.Log("Score Increamented");
                    }
                    else
                    {
                        Debug.Log("Death");
                    }
                }
            }
        }
    }
}
