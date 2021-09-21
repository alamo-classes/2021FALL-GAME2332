using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitAsIcosohedron();
        Subdivide(2);
        GenerateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }
        public Material myMaterial;
        GameObject myPlanetMesh;


        public class Polygon
        {
            public List<int> myVerticies;
            public Polygon(int a, int b, int c)
            {
                myVerticies = new List<int>() { a, b, c };
            }
        }
        public List<Polygon> myPolygons;
        public List<Vector3> myVertices;

        public void InitAsIcosohedron()
        {
            myPolygons = new List<Polygon>();
            myVertices = new List<Vector3>();
            float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

            myVertices.Add(new Vector3(-1, t, 0).normalized);
            myVertices.Add(new Vector3(1, t, 0).normalized);
            myVertices.Add(new Vector3(-1, -t, 0).normalized);
            myVertices.Add(new Vector3(1, -t, 0).normalized);
            myVertices.Add(new Vector3(0, -1, t).normalized);
            myVertices.Add(new Vector3(0, 1, t).normalized);
            myVertices.Add(new Vector3(0, -1, -t).normalized);
            myVertices.Add(new Vector3(0, 1, -t).normalized);
            myVertices.Add(new Vector3(t, 0, -1).normalized);
            myVertices.Add(new Vector3(t, 0, 1).normalized);
            myVertices.Add(new Vector3(-t, 0, -1).normalized);
            myVertices.Add(new Vector3(-t, 0, 1).normalized);

            myPolygons.Add(new Polygon(0, 11, 5));
            myPolygons.Add(new Polygon(0, 5, 1));
            myPolygons.Add(new Polygon(0, 1, 7));
            myPolygons.Add(new Polygon(0, 10, 11));
            myPolygons.Add(new Polygon(1, 5, 9));
            myPolygons.Add(new Polygon(5, 11, 4));
            myPolygons.Add(new Polygon(11, 10, 2));
            myPolygons.Add(new Polygon(10, 7, 6));
            myPolygons.Add(new Polygon(7, 1, 8));
            myPolygons.Add(new Polygon(3, 9, 4));
            myPolygons.Add(new Polygon(3, 4, 2));
            myPolygons.Add(new Polygon(3, 2, 6));
            myPolygons.Add(new Polygon(3, 6, 8));
            myPolygons.Add(new Polygon(3, 8, 9));
            myPolygons.Add(new Polygon(4, 9, 5));
            myPolygons.Add(new Polygon(2, 4, 11));
            myPolygons.Add(new Polygon(6, 2, 10));
            myPolygons.Add(new Polygon(8, 6, 7));
            myPolygons.Add(new Polygon(9, 8, 1));

        }
        public void Subdivide(int recursions)
        {
            var midPointCache = new Dictionary<int, int>();

            for (int i = 0; i < recursions; i++)
            {
                var newPolys = new List<Polygon>();
                foreach (var poly in myPolygons)
                {
                    int a = poly.myVerticies[0];
                    int b = poly.myVerticies[1];
                    int c = poly.myVerticies[2];

                    //use getMidPointIndex to either create a new vertex between two old vertices,
                    //or find the one that was already created

                    int ab = getMidPointIndex(midPointCache, a, b);
                    int bc = getMidPointIndex(midPointCache, b, c);
                    int ca = getMidPointIndex(midPointCache, c, a);

                    //create the four new polygons using our original three vertices,
                    //and the three new midpoints

                    newPolys.Add(new Polygon(a, ab, ca));
                    newPolys.Add(new Polygon(b, bc, ab));
                    newPolys.Add(new Polygon(c, ca, bc));
                    newPolys.Add(new Polygon(ab, bc, ca));


                }
                //replace all our old polygons with the new set of subdivided ones
                myPolygons = newPolys;
            }
        }
        public int getMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB)
        {
            //we create a key out of the two original indices
            //by storing the smaller index in the upper two bytes
            //of an integer. and the larger index in the lower two
            //bytes. By sorting them according to whichever is smaller
            //we ensure that this function returns the same result
            //regardless of which order inputted

            int smallerIndex = Mathf.Min(indexA, indexB);
            int greaterIndex = Mathf.Max(indexA, indexB);
            int key = (smallerIndex << 16) + greaterIndex;

            //if mid point is defined return it

            int ret;
            if (cache.TryGetValue(key, out ret))
                return ret;

            //if we're here, its because a midpoint for these two
            //vertices hasn't been created yet. lets do that now

            Vector3 p1 = myVertices[indexA];
            Vector3 p2 = myVertices[indexB];
            Vector3 middle = Vector3.Lerp(p1, p2, .5f).normalized;

            ret = myVertices.Count;
            myVertices.Add(middle);
            cache.Add(key, ret);
            return ret;
        }
        public void GenerateMesh()
        {
            //we'll store our planet's mesh in the myPlanetMesh
            //variable so that we can delete the old copy when we want
            //to generate a new planetmesh

            if (myPlanetMesh)
                Destroy(myPlanetMesh);

            myPlanetMesh = new GameObject("Planet mesh");

            MeshRenderer surfaceRenderer = myPlanetMesh.AddComponent<MeshRenderer>();
            surfaceRenderer.material = myMaterial;

            Mesh terrainMesh = new Mesh();
            int vertexCount = myPolygons.Count * 3;
            int[] indices = new int[vertexCount];

            Vector3[] vertices = new Vector3[vertexCount];
            Vector3[] normals = new Vector3[vertexCount];
            Color32[] colors = new Color32[vertexCount];

            Color32 green = new Color32(20, 255, 30, 255);
            Color32 brown = new Color32(220, 150, 70, 255);
            Color32 red = new Color32(161, 37, 27, 255);

            for (int i = 0; i < myPolygons.Count; i++)
            {
                var poly = myPolygons[i];

                indices[i * 3 + 0] = i * 3 + 0;
                indices[i * 3 + 1] = i * 3 + 1;
                indices[i * 3 + 2] = i * 3 + 2;

                vertices[i * 3 + 0] = myVertices[poly.myVerticies[0]];
                vertices[i * 3 + 1] = myVertices[poly.myVerticies[1]];
                vertices[i * 3 + 2] = myVertices[poly.myVerticies[2]];

                //here is where we assign each polygon a random color from inputed colors

                Color32 polyColor = Color32.Lerp(green, brown, Random.Range(0.0f, 1.0f));

                colors[i * 3 + 0] = polyColor;
                colors[i * 3 + 1] = polyColor;
                colors[i * 3 + 2] = polyColor;

                //for now our planet is still perfectly spherical
                //so the normal of each vertex is just like the vertex
                //itself: pointing away from the origin.

                normals[i * 3 + 0] = myVertices[poly.myVerticies[0]];
                normals[i * 3 + 1] = myVertices[poly.myVerticies[1]];
                normals[i * 3 + 2] = myVertices[poly.myVerticies[2]];
            }

            terrainMesh.vertices = vertices;
            terrainMesh.normals = normals;
            terrainMesh.colors32 = colors;

            terrainMesh.SetTriangles(indices, 0);

            MeshFilter terrainFilter = myPlanetMesh.AddComponent<MeshFilter>();
            terrainFilter.mesh = terrainMesh;


        }
    }