using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TilePlacementChecker : MonoBehaviour
{
    public bool TileCanBePlaced = true;
    public Collider tileCollider;
    public bool PlacementCheckCompleted = false;
    public TileGenerator TileCreator;
    [SerializeField]
    private GameObject[] baseTileSelection;
    [SerializeField]
    private GameObject[] capTileSelection;
    [SerializeField]
    private GameObject[] endTileSelection;
    private GameObject GeneratedTile;
    private LevelGeneratorManager GenerationManager;
    public bool tileGenerated;
    private bool canGenerateBaseTiles;
    private bool canGenerateEndTile;
    public bool TilePlacementGoAhead;
    private GameObject CreatedTile;
    private Quaternion Rotation;

    private void Awake()
    {
        //gets a reference to the game manager to supply the list of generatable objects
        GenerationManager = GameObject.FindGameObjectWithTag("GenerationManager").GetComponent<LevelGeneratorManager>();
        //Gets all of the lists of prefabs that the manager has
        baseTileSelection = GenerationManager.ReturnBaseTileList();
        endTileSelection = GenerationManager.ReturnEndTileList();
        capTileSelection = GenerationManager.ReturnCapTileList();
        Rotation = this.transform.rotation;
        TilePlacementGoAhead = false;

    }

    private void FixedUpdate()
    {
        //performs a check to see if the tile is placed somewhere it is allowed to be, if this check has already been made, then nothing happens.
        if (PlacementCheckCompleted == false)
        {
            Debug.Log("Starting to check tile placement");
            StartCoroutine(TilePlacementIsAllowed());
            Debug.Log("Placement Completed Successfully");
            PlacementCheckCompleted = true;
        }
    }


    IEnumerator TilePlacementIsAllowed()
    {
        yield return new WaitForFixedUpdate();
        //if the tile is not allowed to be in this position, it generates a new tile overtop of itself and deletes itself.
        if (TileCanBePlaced == false)
        {
            if (tileGenerated == false)
            {
                //checks if basic tiles can be generated
                canGenerateBaseTiles = GenerationManager.CheckTileGenState();
                if (canGenerateBaseTiles == true)
                {
                    //selects a random basic tile to generate
                    GeneratedTile = baseTileSelection[Random.Range(0, baseTileSelection.Length)];
                    
                }
                if (canGenerateBaseTiles == false)
                {
                    //checks if the end tile has already been generated
                    canGenerateEndTile = GenerationManager.CheckEndGenState();
                    if (canGenerateEndTile == true)
                    {
                        GeneratedTile = endTileSelection[Random.Range(0, endTileSelection.Length)];
                        
                    }
                    if (canGenerateEndTile == false)
                    {
                        GeneratedTile = capTileSelection[Random.Range(0, capTileSelection.Length)];
                        

                    }

                }
                CreatedTile = Instantiate(GeneratedTile, transform.position, Rotation);
                tileGenerated = true;
            }
            Debug.Log("tile could not be placed");
            //checks if the tile placed is a normal or end tile, and removes this tile from the list and adds the newly placed one
            if (GeneratedTile.tag == "Tile")
            {
                GenerationManager.PlacedTiles.Remove(gameObject);
                GenerationManager.PlacedTiles.Add(CreatedTile);
            }
            else
            {
                GenerationManager.PlacedEndTiles.Remove(gameObject);
                GenerationManager.PlacedEndTiles.Add(CreatedTile);
            }
            Destroy(this.gameObject);

        }
        else
        {
            //if the tile can fit in, it tells it sets the confirmation to place new tiles to true for the tile generators attach to this object.
            Debug.Log("tile placed succesfully");
            TilePlacementGoAhead = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if another tile collides with this one, then it returns that the tile cannot be properly placed.
        if (other.tag == "Tile" || other.tag == "EndTile")
        {
            TileCanBePlaced = false;
            Debug.Log("other tile interacted, " + TileCanBePlaced);
        }
    }

    private void OnDestroy()
    {
    
    }
}
