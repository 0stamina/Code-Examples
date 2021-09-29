using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTerrainGeneration : MonoBehaviour
{
    [System.Serializable]
    public struct Plate
    {
        public SphericalCoordinates position;
        public float direction;
        public int key_point;
        public Color color;
    }

    public List<Plate> plates;
    public List<SphericalCoordinates> hotSpots;
    public List<SphericalCoordinates> keyPoints;
    public int[][] plateIndecies;

    void Start()
    {
        Generate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Generate();
        }
    }

    void Generate()
    {
        plates = new List<Plate>();
        hotSpots = new List<SphericalCoordinates>();
        keyPoints = new List<SphericalCoordinates>();
        int key_points = Random.Range(8, 15);
        plateIndecies = new int[key_points][];
        for (int i = 0; i < key_points; i++)
        {
            SphericalCoordinates best_key_point = new SphericalCoordinates();
            if(i != 0)
            {
                float best_distance = 0;
                for (int u = 0; u < 5; u++)
                {
                    float distance = Mathf.PI * 2f;
                    SphericalCoordinates point = new SphericalCoordinates(Mathf.Acos(Random.Range(-1f, 1f)), Random.Range(0, Mathf.PI * 2f));
                    foreach (SphericalCoordinates key_point in keyPoints)
                    {
                        float d = Mathf.Acos(Vector3.Dot(point, key_point));
                        if(d < distance)
                        {
                            distance = d;
                        }
                    }

                    if(distance > best_distance)
                    {
                        best_distance = distance;
                        best_key_point = point;
                    }
                }
            }
            else
            {
                best_key_point = new SphericalCoordinates(Mathf.Acos(Random.Range(-1f, 1f)), Random.Range(0, Mathf.PI * 2f));
            }

            keyPoints.Add(best_key_point);

            //bias calculation
            float x = Random.Range(0f, 1f);
            float k = Mathf.Pow(1 - 0.5f, 3f);
            int num_plates = Mathf.FloorToInt(1 + (x * k / (x * k - x + 1) * 4));
            plateIndecies[i] = new int[num_plates];

            for (int j = 0; j < num_plates; j++)
            {
                Plate best_plate = new Plate();
                if (j != 0)
                {
                    float best_distance = 0;
                    SphericalCoordinates best_point = new SphericalCoordinates();

                    for (int u = 0; u < 10; u++)
                    {
                        float distance = Mathf.PI * 2f;

                        float angle = Random.Range(0.01f, 0.2f);
                        Vector3 vec3_point = best_key_point;
                        float theta = Random.Range(0, 2f * Mathf.PI);
                        Vector3 unitCirc = new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta));
                        Vector3 ortho = Vector3.Cross(Vector3.up, vec3_point).normalized;
                        Quaternion look_at_ortho = Quaternion.LookRotation(ortho, vec3_point);

                        vec3_point = vec3_point * Mathf.Cos(angle) + look_at_ortho * unitCirc * Mathf.Sin(angle);

                        for(int p = 0; p < plateIndecies[i].Length; p++)
                        {
                            float d = Mathf.Acos(Vector3.Dot(vec3_point, plates[plateIndecies[i][p]].position));
                            if (d < distance)
                            {
                                distance = d;
                            }
                        }

                        if (distance > best_distance)
                        {
                            best_distance = distance;
                            best_point = vec3_point;
                        }
                    }


                    best_plate.position = best_point;
                    best_plate.position.radius = 1;
                    best_plate.direction = Random.Range(0, 2f * Mathf.PI);
                    best_plate.key_point = i;
                }
                else
                {
                    float angle = Random.Range(0.01f, 0.2f);
                    Vector3 vec3_point = best_key_point;
                    float theta = Random.Range(0, 2f * Mathf.PI);
                    Vector3 unitCirc = new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta));
                    Vector3 ortho = Vector3.Cross(Vector3.up, vec3_point).normalized;
                    Quaternion look_at_ortho = Quaternion.LookRotation(ortho, vec3_point);
                    vec3_point = vec3_point * Mathf.Cos(angle) + look_at_ortho * unitCirc * Mathf.Sin(angle);


                    best_plate.position = vec3_point;
                    best_plate.position.radius = 1;
                    best_plate.direction = Random.Range(0, 2f * Mathf.PI);
                    best_plate.key_point = i;
                }
                best_plate.color = Color.HSVToRGB(Random.Range(0f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                plateIndecies[i][j] = plates.Count;
                plates.Add(best_plate);
            }
        }

        int hot_spots = Random.Range(25, 50);
        for (int h = 0; h < hot_spots; h++)
        {
            hotSpots.Add(new SphericalCoordinates(Mathf.Acos(Random.Range(-1f, 1f)), Random.Range(0, Mathf.PI * 2f)));
        }
    }

    public void OnDrawGizmos()
    {
        if (plates != null && plates.Count > 0)
        {
            foreach (Plate p in plates)
            {
                Vector3 pos = p.position;
                Vector3 k = new Vector3(Mathf.Cos(p.direction), 0, Mathf.Sin(p.direction));
                Vector3 ortho = Vector3.Cross(Vector3.up, pos);
                ortho.Normalize();
                Quaternion look_at_ortho = Quaternion.LookRotation(ortho, pos);

                Gizmos.color = Color.HSVToRGB(p.direction / (2f * Mathf.PI), 1f, 1f);
                Gizmos.DrawSphere(pos, 0.01f);
                Gizmos.DrawLine(pos, pos + (look_at_ortho * k).normalized / 10f);
            }

            /*
            Gizmos.color = Color.black;
            foreach (SphericalCoordinates hot_spot in hotSpots)
            {
                Gizmos.DrawSphere(hot_spot, 0.01f);
            }
            */

            for (float theta = 0f; !(theta > Mathf.PI); theta += 0.1f)
            {
                for (float phi = 0f; !(phi > Mathf.PI * 2f); phi += 0.1f)
                {
                    SphericalCoordinates check_pos = new SphericalCoordinates(theta, phi);
                    float distance = Mathf.PI * 2;
                    int index = 0;
                    for(int i = 0; i < plates.Count; i++)
                    {
                        if(Mathf.Acos(Vector3.Dot(check_pos, plates[i].position)) < distance)
                        {
                            index = i;
                            distance = Mathf.Acos(Vector3.Dot(check_pos, plates[i].position));
                        }
                    }

                    Gizmos.color = plates[index].color;
                    Gizmos.DrawSphere(check_pos, 0.01f);
                }
            }
        }
    }

}
