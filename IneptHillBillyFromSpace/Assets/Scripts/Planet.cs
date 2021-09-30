using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Planet : MonoBehaviour
{
    public int mySubDivideCount;
    // Start is called before the first frame update
    void Start()
    {
        InitAsIcosohedron();
        Subdivide(mySubDivideCount);
        GenerateMesh();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Material myMaterial;
    GameObject myPlanetMesh;

    public void CalculateNeighbors()
    {
        foreach (Polygon poly in myPolygons)
        {
            foreach (Polygon other_poly in myPolygons)
            {
                if (poly == other_poly)
                    continue;

                if (poly.IsNeighborOf(other_poly))
                    poly.myNeighbors.Add(other_poly);
            }
        }
    }
    public List<int> CloneVertices(List<int> old_verts)
    {
        List<int> new_verts = new List<int>(); foreach (int old_vert in old_verts)
        {
            Vector3 cloned_vert = myVertices[old_vert];
            new_verts.Add(myVertices.Count);
            myVertices.Add(cloned_vert);
        }
        return new_verts;
    }
    
    public class Edge
    {
        // The Poly that's inside the Edge. This is the one 
        // we'll be extruding or insetting.
        public Polygon m_InnerPoly;   
        // The Poly that's outside the Edge. We'll be leaving                             
        // this one alone.
        public Polygon m_OuterPoly;  
        //The vertices along this edge, according to the Outer poly.
        public List<int> m_OuterVerts;  
        //The vertices along this edge, according to the Inner poly.
        public List<int> m_InnerVerts; public Edge(Polygon inner_poly, Polygon outer_poly)
        {
            m_InnerPoly = inner_poly;
            m_OuterPoly = outer_poly;
            m_OuterVerts = new List<int>(2);
            m_InnerVerts = new List<int>(2);    
            //Find which vertices these polys share.
            foreach (int vertex in inner_poly.myVertices)
            {
                if (outer_poly.myVertices.Contains(vertex))
                    m_InnerVerts.Add(vertex);
            }
            // For consistency, we want the 'winding order' of the 
            // edge to be the same as that of the inner polygon.
            // So the vertices in the edge are stored in the same order
            // that you would encounter them if you were walking clockwise
            // around the polygon. That means the pair of edge vertices 
            // will be:
            // [1st inner poly vertex, 2nd inner poly vertex] or
            // [2nd inner poly vertex, 3rd inner poly vertex] or
            // [3rd inner poly vertex, 1st inner poly vertex]
            //
            // The formula above will give us [1st inner poly vertex, 
            // 3rd inner poly vertex] though, so we check for that 
            // situation and reverse the vertices.

            if (m_InnerVerts[0] == inner_poly.myVertices[0] &&
            m_InnerVerts[1] == inner_poly.myVertices[2])
            {
                int temp = m_InnerVerts[0];
                m_InnerVerts[0] = m_InnerVerts[1];
                m_InnerVerts[1] = temp;
            }    // No manipulations have happened yet, so the outer and 
                 // inner Polygons still share the same vertices.
                 // We can instantiate m_OuterVerts as a copy of m_InnerVerts.
            m_OuterVerts = new List<int>(m_InnerVerts);
        }
    }

    public class EdgeSet : HashSet<Edge>
    {
        // Split - Given a list of original vertex indices and a list of
        // replacements, update m_InnerVerts to use the new replacement
        // vertices.  
        public void Split(List<int> oldVertices, List<int> newVertices)
        {
            foreach (Edge edge in this)
            {
                for (int i = 0; i < 2; i++)
                {
                    edge.m_InnerVerts[i] = newVertices[oldVertices.IndexOf(
                                           edge.m_OuterVerts[i])];
                }
            }
        }  // GetUniqueVertices - Get a list of all the vertices referenced
           // in this edge loop, with no duplicates.  
        public List<int> GetUniqueVertices()
        {
            List<int> vertices = new List<int>();
            foreach (Edge edge in this)
            {
                foreach (int vert in edge.m_OuterVerts)
                {
                    if (!vertices.Contains(vert))
                        vertices.Add(vert);
                }
            }
            return vertices;
        }
    }

    public class Polygon
    {
        public List<int> myVertices;
        public List<Polygon> myNeighbors;
        public Color32 myColor;
        public bool mySmoothNormals;
        public Polygon(int a, int b, int c)
        {
            myVertices = new List<int>() { a, b, c };
            myNeighbors = new List<Polygon>();

            // This will determine whether a polygon's normals smoothly
            // blend into its neighbors, or if it should have sharp edges.
            mySmoothNormals = true;

            // Hot Pink is an excellent default color because you'll 
            // notice instantly if you forget to set it to something else.
            myColor = new Color32(255, 0, 255, 255);
        }
        public bool IsNeighborOf(Polygon other_poly)
        {
            int shared_vertices = 0;
            foreach (int vertex in myVertices)
            {
                if (other_poly.myVertices.Contains(vertex))
                    shared_vertices++;
            }    // A polygon and its neighbor will share exactly
                 // two vertices. Ergo, if this poly shares two
                 // vertices with the other, then they are neighbors.    return shared_vertices == 2;
            return shared_vertices == 2;
        }
        public void ReplaceNeighbor(Polygon oldNeighbor, Polygon newNeighbor)
        {
            for (int i = 0; i < myNeighbors.Count; i++)
            {
                if (oldNeighbor == myNeighbors[i])
                {
                    myNeighbors[i] = newNeighbor;
                    return;
                }
            }
        }
    }
    public class PolySet : HashSet<Polygon>
    {
        //Given a set of Polys, calculate the set of Edges
        //that surround them.  
        public EdgeSet CreateEdgeSet()
        {
            EdgeSet edgeSet = new EdgeSet();
            foreach (Polygon poly in this)
            {
                foreach (Polygon neighbor in poly.myNeighbors)
                {
                    if (this.Contains(neighbor))
                        continue;

                    // If our neighbor isn't in our PolySet, then
                    // the edge between us and our neighbor is one
                    // of the edges of this PolySet.        
                    Edge edge = new Edge(poly, neighbor);
                    edgeSet.Add(edge);
                }
            }
            return edgeSet;
        }  // GetUniqueVertices calculates a list of the vertex indices 
           // used by these Polygons with no duplicates.  
        public List<int> GetUniqueVertices()
        {
            List<int> verts = new List<int>();
            foreach (Polygon poly in this)
            {
                foreach (int vert in poly.myVertices)
                {
                    if (!verts.Contains(vert))
                        verts.Add(vert);
                }
            }
            return verts;
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
                int a = poly.myVertices[0];
                int b = poly.myVertices[1];
                int c = poly.myVertices[2];

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

            vertices[i * 3 + 0] = myVertices[poly.myVertices[0]];
            vertices[i * 3 + 1] = myVertices[poly.myVertices[1]];
            vertices[i * 3 + 2] = myVertices[poly.myVertices[2]];

            //here is where we assign each polygon a random color from inputed colors

            Color32 polyColor = Color32.Lerp(red, brown, Random.Range(0.0f, 1.0f));

            colors[i * 3 + 0] = polyColor;
            colors[i * 3 + 1] = polyColor;
            colors[i * 3 + 2] = polyColor;

            //for now our planet is still perfectly spherical
            //so the normal of each vertex is just like the vertex
            //itself: pointing away from the origin.

            normals[i * 3 + 0] = myVertices[poly.myVertices[0]];
            normals[i * 3 + 1] = myVertices[poly.myVertices[1]];
            normals[i * 3 + 2] = myVertices[poly.myVertices[2]];
        }

        terrainMesh.vertices = vertices;
        terrainMesh.normals = normals;
        terrainMesh.colors32 = colors;

        terrainMesh.SetTriangles(indices, 0);

        MeshFilter terrainFilter = myPlanetMesh.AddComponent<MeshFilter>();
        terrainFilter.mesh = terrainMesh;

        MeshCollider meshc = myPlanetMesh.AddComponent(typeof(MeshCollider)) as MeshCollider;
    }

}