using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{

    [SerializeField]
    private GameObject[] baseTileSelection;
    [SerializeField]
    private GameObject[] capTileSelection;
    [SerializeField]
    private GameObject[] endTileSelection;
    private GameObject GeneratedTile;
    private LevelGeneratorManager GenerationManager;
    private TilePlacementChecker MyTilePlacementChecker;
    public bool tileGenerated;
    private Transform TilePosition;
    private bool canGenerateBaseTiles;
    private bool canGenerateEndTile;
    public bool NewTileIsPlacable;
    private Vector3 Position;
    private Quaternion Rotation;
    private GameObject CreatedTile;
    // Start is called before the first frame update
    void Awake()
    {
        //gets a reference to the game manager to supply the list of generatable objects
        GenerationManager = GameObject.FindGameObjectWithTag("GenerationManager").GetComponent<LevelGeneratorManager>();
        //Gets all of the lists of prefabs that the manager has
        baseTileSelection = GenerationManager.ReturnBaseTileList();
        endTileSelection = GenerationManager.ReturnEndTileList();
        capTileSelection = GenerationManager.ReturnCapTileList();
        //Gets the Tile Checker script off the parent of this object
        MyTilePlacementChecker = this.gameObject.GetComponentInParent<TilePlacementChecker>();
        //Gets current position and rotation of the generator object
        Position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Rotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the placement checker of this tile confirms that it can begin generating the next tile(s)
        if (MyTilePlacementChecker.TilePlacementGoAhead == true || MyTilePlacementChecker == null)
        {
            ActivateTileGeneration();
        }
    }

    private void ActivateTileGeneration()
    {
        if (tileGenerated == false)
        {
            TilePosition = this.transform;
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
                    //selects a random Level end tile to generate (Only one should be generated)
                    GeneratedTile = endTileSelection[Random.Range(0, endTileSelection.Length)];
                   
                }
                if (canGenerateEndTile == false)
                {
                    //selects a random cap tile to generate
                    GeneratedTile = capTileSelection[Random.Range(0, capTileSelection.Length)];
                    

                }

            }
            //Generates the selected tile on top of the tile generator
            CreatedTile = Instantiate(GeneratedTile, transform.position, Rotation);
            //checks if the tile generated is a normal tile, if so, adds it to the list of placed normal tiles
            if (GeneratedTile.tag == "Tile")
            {
                GenerationManager.PlacedTiles.Add(CreatedTile);
            }
            else
            {
                GenerationManager.PlacedEndTiles.Add(CreatedTile);
            }
            tileGenerated = true;
        }
    }


    
}
