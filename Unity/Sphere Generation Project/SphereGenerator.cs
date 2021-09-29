using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SphereGenerator : MonoBehaviour
{
    public bool autoUpdate;
    public int resolution;
    public FibbonacciSphere sphere;

    public int seed;

    public Vector2[] projectedPoints;
    public List<DelaunayTri> tris;
    public List<List<int>> triHash;
    [Range(0, 10)]
    public float jitter;

    public GameObject meshPrefab;

    public void Start()
    {
        Generate();
    }

    public void Generate()
    {
        Random.InitState(seed);
        sphere = new FibbonacciSphere(resolution);


        float s = 3.6f / Mathf.Sqrt(resolution);
        float dz = 2.0f / resolution;
        float z = 1 - (dz / 2f);
        float lon = 0;
        for (int k = 0; k < resolution; k++, z -= dz)
        {
            float r = Mathf.Sqrt(1 - (z * z));
            float theta = Mathf.Acos(z);
            float phi = lon;
            float randLat = Random.Range(-jitter / 60f, jitter / 60f);
            float randLon = Random.Range(-jitter / 60f, jitter / 60f);
            
            theta += randLat;
            phi += randLon;

            sphere.points[k] = new SphericalCoordinates(theta, phi);

            lon += s / r;
        }

        Triangulate();
        GenerateMeshes();

    }

    public void Triangulate()
    {
        float minX = float.NaN;
        float minY = float.NaN;
        float maxX = float.NaN;
        float maxY = float.NaN;
        projectedPoints = new Vector2[resolution + 3];
        for (int i = 1; i < resolution; i++)
        {
            float r = Mathf.Sin(sphere.points[i].Theta) / (1 - Mathf.Cos(sphere.points[i].Theta));
            float angle = sphere.points[i].Phi;
            projectedPoints[i] = new Vector2(r * Mathf.Cos(angle), r * Mathf.Sin(angle));

            if (float.IsNaN(minX))
            {
                minX = projectedPoints[i].x;
                minY = projectedPoints[i].y;
                maxX = projectedPoints[i].x;
                maxY = projectedPoints[i].y;
            }
            else
            {
                if (projectedPoints[i].x < minX)
                    minX = projectedPoints[i].x;
                if (projectedPoints[i].y < minY)
                    minY = projectedPoints[i].y;
                if (projectedPoints[i].x > maxX)
                    maxX = projectedPoints[i].x;
                if (projectedPoints[i].y > maxY)
                    maxY = projectedPoints[i].y;
            }
        }

        tris = new List<DelaunayTri>();
        //super-triangle abc
        projectedPoints[resolution + 2] = new Vector2((maxX + minX) / 2.0f, minY - (maxY - minY));
        projectedPoints[resolution + 1] = new Vector2(minX - (maxX - minX), maxY + 1);
        projectedPoints[resolution] = new Vector2(maxX + (maxX - minX), maxY + 1);

        DelaunayTri dt = new DelaunayTri(resolution, resolution + 1, resolution + 2 );
        CalculateCircumcircle(projectedPoints[dt.a], projectedPoints[dt.b], projectedPoints[dt.c], out Vector2 circumcenter, out float circumradius);


        dt.circumcenter = circumcenter;
        dt.circumradius = circumradius;

        tris.Add(dt);

        triHash = new List<List<int>>();
        for (int i = 1; i < resolution + 3; i++)
        {
            if(i < resolution + 1)
            {
                triHash.Add(new List<int>());
            }
            
            //badTriangles:= empty set
            List<DelaunayTri> badTris = new List<DelaunayTri>();
            //for each triangle in triangulation do // first find all the triangles that are no longer valid due to the insertion
            foreach (DelaunayTri tri in tris)
            {
                //if point is inside circumcircle of triangle
                if (Vector2.Distance(projectedPoints[i], tri.circumcenter) < tri.circumradius)
                {
                    //add triangle to badTriangles
                    badTris.Add(tri);
                    continue;
                }
            }

            //polygon := empty set
            List<Edge> polygon = new List<Edge>();
            //for each triangle in badTriangles do // find the boundary of the polygonal hole
            foreach (DelaunayTri badTri in badTris)
            {
                //for each edge in triangle do

                //if edge is not shared by any other triangles in badTriangles
                //add edge to polygon
                bool add = true;
                foreach (DelaunayTri thisTri in badTris)
                {
                    if(badTri != thisTri)
                    {
                        if (badTri.AB == thisTri.AB)
                        {
                            add = false;
                            break;
                        }
                        if (badTri.AB == thisTri.BC)
                        {
                            add = false;
                            break;
                        }
                        if (badTri.AB == thisTri.CA)
                        {
                            add = false;
                            break;
                        }
                    }
                }
                if (add)
                {
                    polygon.Add(badTri.AB);
                }

                //if edge is not shared by any other triangles in badTriangles
                //add edge to polygon
                add = true;
                foreach (DelaunayTri thisTri in badTris)
                {
                    if (badTri != thisTri)
                    {
                        if (badTri.BC == thisTri.AB)
                        {
                            add = false;
                            break;
                        }
                        if (badTri.BC == thisTri.BC)
                        {
                            add = false;
                            break;
                        }
                        if (badTri.BC == thisTri.CA)
                        {
                            add = false;
                            break;
                        }
                    }
                }
                if (add)
                {
                    polygon.Add(badTri.BC);
                }

                //if edge is not shared by any other triangles in badTriangles
                //add edge to polygon
                add = true;
                foreach (DelaunayTri thisTri in badTris)
                {
                    if (badTri != thisTri)
                    {
                        if (badTri.CA == thisTri.AB)
                        {
                            add = false;
                            break;
                        }
                        if (badTri.CA == thisTri.BC)
                        {
                            add = false;
                            break;
                        }
                        if (badTri.CA == thisTri.CA)
                        {
                            add = false;
                            break;
                        }
                    }

                }
                if (add)
                {
                    polygon.Add(badTri.CA);
                }

                //remove triangle from triangulation
                for (int t = 0; ; t++)
                {
                    if (t >= tris.Count)
                        break;

                    if (tris[t] == badTri)
                    {
                        tris.RemoveAt(t);
                        t--;
                        continue;
                    }
                }
            }

            //for each edge in polygon do // re-triangulate the polygonal hole
            foreach (Edge edge in polygon)
            {
                if (edge.point1 == i || edge.point2 == i)
                    continue;
                //newTri:= form a triangle from edge to point
                DelaunayTri tri = new DelaunayTri(edge.point1, edge.point2, i);
                CalculateCircumcircle(projectedPoints[tri.a], projectedPoints[tri.b], projectedPoints[tri.c], out circumcenter, out circumradius);


                tri.circumcenter = circumcenter;
                tri.circumradius = circumradius;


                //add newTri to triangulation
                tris.Add(tri);
            }
        }

        //for each triangle in triangulation // done inserting points, now clean up
        for (int t = 0; t < tris.Count; t++)
        {
            DelaunayTri tri = tris[t];

            int val = 0;
            //if triangle contains a vertex from original super-triangle
            if (tris[t].a == resolution || tris[t].a == resolution + 1 || tris[t].a == resolution + 2)
            {
                tri.a = 0;
                val++;
            }
            if (tris[t].b == resolution || tris[t].b == resolution + 1 || tris[t].b == resolution + 2)
            {
                tri.b = 0;
                val++;
            }
            if (tris[t].c == resolution || tris[t].c == resolution + 1 || tris[t].c == resolution + 2)
            {
                tri.c = 0;
                val++;
            }

            if (val >= 2)
            {
                tris.RemoveAt(t);
                t--;
                continue;
            }

            CalculateCentroid(sphere.points[tri.a], sphere.points[tri.b], sphere.points[tri.c], out Vector3 pos);
            tri.centroid = pos;
            tris[t] = tri;

            triHash[tri.a].Add(t);
            triHash[tri.b].Add(t);
            triHash[tri.c].Add(t);
        }

        sphere.tris = new int[tris.Count * 3];

        for(int i = 0; i < tris.Count; i++)
        {
            if(tris[i].a < resolution)
            {
                sphere.tris[i * 3] = tris[i].a;
            }
            
            if(tris[i].b < resolution)
            {
                sphere.tris[i * 3 + 1] = tris[i].b;
            }

            if(tris[i].c < resolution)
            {
                sphere.tris[i * 3 + 2] = tris[i].c;
            }
        }
    }

    public void GenerateMeshes()
    {
        for(; transform.childCount != 0;)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        if (sphere.points != null)
        {
            for (int i = 0; i < sphere.points.Length; i++)
            {
                Color col = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                Vector3 pos = sphere.points[i];
                GameObject meshObj = Instantiate(meshPrefab, pos, new Quaternion(), transform);
                MeshFilter meshFilter = meshObj.GetComponent<MeshFilter>();
                Mesh mesh = new Mesh();

                List<Vector3> verticies = new List<Vector3>();
                verticies.Add(Vector3.zero);
                List<Color> colors = new List<Color>();
                colors.Add(col);
                List<int> triangles = new List<int>();

                int j = 0;
                List<int> checkedIndecies = new List<int>();
                for (int u = 0; u < triHash[i].Count; u++)
                {
                    checkedIndecies.Add(j);
                    int v = ((tris[triHash[i][j]].a == i) ? 1 : 0) | ((tris[triHash[i][j]].b == i) ? 1 : 0) << 1 | ((tris[triHash[i][j]].c == i) ? 1 : 0) << 2;
                    verticies.Add(tris[triHash[i][j]].centroid - pos);
                    colors.Add(col);

                    triangles.Add(0);
                    triangles.Add(u + 1);
                    for (int k = 0; k < triHash[i].Count; k++)
                    {
                        if (checkedIndecies.Contains(k))
                        {
                            continue;
                        }

                        switch (v)
                        {
                            case 1:
                                {
                                    //check AB and CA
                                    if (tris[triHash[i][k]].HasEdge(tris[triHash[i][j]].CA))
                                    {
                                        j = k;
                                    }
                                    break;
                                }
                            case 2:
                                {
                                    //check AB and BC
                                    if (tris[triHash[i][k]].HasEdge(tris[triHash[i][j]].AB))
                                    {
                                        j = k;
                                    }
                                    break;
                                }
                            case 4:
                                {
                                    //check BC and CA
                                    if (tris[triHash[i][k]].HasEdge(tris[triHash[i][j]].BC))
                                    {
                                        j = k;
                                    }
                                    break;
                                }
                        }

                        if (j == k)
                            break;
                    }

                    if (u + 1 < triHash[i].Count)
                    {
                        triangles.Add(u + 2);
                    }
                    else
                    {
                        triangles.Add(1);
                    }
                    
                }

                mesh.SetVertices(verticies);
                mesh.SetTriangles(triangles, 0);
                mesh.SetColors(colors);
                mesh.RecalculateNormals();
                meshFilter.mesh = mesh;

            }
        }
    }

    public void CalculateCentroid(Vector3 P, Vector3 Q, Vector3 R, out Vector3 centroid)
    {
        SphericalCoordinates pos = (P + Q + R) / 3f;
        pos.radius = 1;

        centroid = pos;
    }

    public void CalculateCircumcircle(Vector2 P, Vector2 Q, Vector2 R, out Vector2 circumcenter, out float circumradius)
    {
        //slope of PQ bisector
        float aPQ = -(P.x - Q.x);
        float bPQ = Q.y - P.y;
        float cPQ = aPQ * ((P.x + Q.x) / 2.0f) + bPQ * ((P.y + Q.y) / 2.0f);

        //slope of QR bisector
        float aQR = -(Q.x - R.x);
        float bQR = R.y - Q.y;
        float cQR = aQR * ((Q.x + R.x) / 2.0f) + bQR * ((Q.y + R.y) / 2.0f);

        //slope of RP bisector
        float aRP = -(R.x - P.x);
        float bRP = P.y - R.y;
        float cRP = aRP * ((R.x + P.x) / 2.0f) + bRP * ((R.y + P.y) / 2.0f);


        float determinant_PQ_QR = aPQ * bQR - aQR * bPQ;
        float determinant_QR_RP = aQR * bRP - aRP * bQR;
        float determinant_RP_PQ = aRP * bPQ - aPQ * bRP;

        Vector2 intersection_PQ_QR = new Vector2((cPQ * bQR - cQR * bPQ) / determinant_PQ_QR, (aPQ * cQR - aQR * cPQ) / determinant_PQ_QR);
        Vector2 intersection_QR_RP = new Vector2((cQR * bRP - cRP * bQR) / determinant_QR_RP, (aQR * cRP - aRP * cQR) / determinant_QR_RP);
        Vector2 intersection_RP_PQ = new Vector2((cRP * bPQ - cPQ * bRP) / determinant_RP_PQ, (aRP * cPQ - aPQ * cRP) / determinant_RP_PQ);

        circumcenter = (intersection_PQ_QR + intersection_QR_RP + intersection_RP_PQ) / 3f;
        circumradius = Vector2.Distance(circumcenter, P);
    }

    private void OnDrawGizmos()
    {
        if(tris != null)
        {
            foreach (DelaunayTri tri in tris)
            {
                //Gizmos.DrawSphere(tri.centroid, 0.01f);
            }
        }
    }

    private void OnValidate()
    {
        if (resolution < 6)
        {
            resolution = 6;
        }

        if (resolution > 200)
        {
            autoUpdate = false;
        }
    }
}
