using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildNavMesh : MonoBehaviour
{
    public Transform planet;
    public float bakeTerm = 20f;
    public float boundSize = 100f;
    public LayerMask navMeshLayer;

    private Vector3 bakePos;   
    private NavMeshDataInstance dataInstance;
    private int buildSettingsCount;
    NavMeshBuildSettings buildSettings;
    private List<NavMeshBuildSource> buildSources = new List<NavMeshBuildSource>();
    private Bounds bound;

    private void Awake()
    {
        InitializeBuildSetting();
        UpdateNavMesh();
    }

    void InitializeBuildSetting()
    {
        //build setting
        buildSettings = NavMesh.GetSettingsByID(1);

        //set build source
        Bounds planetBound = new Bounds(planet.position,planet.localScale*2);
        NavMeshBuilder.CollectSources(planetBound
            , navMeshLayer
            , new NavMeshCollectGeometry()
            , 0
            , new List<NavMeshBuildMarkup>()
            , buildSources);
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(bakePos, transform.position) > bakeTerm)
            UpdateNavMesh();
    }

    void UpdateNavMesh()
    {
        bakePos = transform.position;
        Vector3 gravityUp = (transform.position-planet.position).normalized;

        dataInstance.Remove();

        Bounds localBound = new Bounds(Vector3.zero, new Vector3(1,1,1)*boundSize);// new Bounds(transform.position, boundSize *new Vector3(1, 1, 1));
        bound = new Bounds(transform.position, new Vector3(1, 1, 1) * boundSize);

        //for ( int ind = 0; ind < buildSettingsCount; ind++ )
        //{
        //    NavMeshBuildSettings buildSettings = NavMesh.GetSettingsByID( ind );

        //    NavMeshData navMeshData = NavMeshBuilder.BuildNavMeshData(buildSettings
        //        , buildSources
        //        , localBound
        //        , transform.position
        //        , Quaternion.FromToRotation(Vector3.up, gravityUp));
        //    dataInstance = NavMesh.AddNavMeshData(navMeshData);

        //    Debug.Log("NavMeshBuildSettings "+ ind +"is Valid : "+ dataInstance.valid );
        //}

        NavMeshData navMeshData = NavMeshBuilder.BuildNavMeshData(buildSettings
            , buildSources
            , localBound
            , transform.position
            , Quaternion.FromToRotation(Vector3.up, gravityUp));
        dataInstance = NavMesh.AddNavMeshData(navMeshData);
    }

    public bool IsOnNavMesh( Transform ai )
    {
        if ( bound.Contains( ai.transform.position ) )
        {
            return true;
        }

        return false;
    }
}
