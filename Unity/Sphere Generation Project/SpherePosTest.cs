using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePosTest : MonoBehaviour
{
    public SphericalCoordinates point;
    public float angle;
    public float theta;
    void Start()
    {
        point.radius = 1;
        point.Phi = Random.Range(0, Mathf.PI * 2f);
        point.Theta = Mathf.Acos(Random.Range(-1f, 1f));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public SphericalCoordinates RandomiseSphericalPoint(SphericalCoordinates point)
    {
        point.Phi = Random.Range(0, Mathf.PI * 2f);
        point.Theta = Mathf.Acos(Random.Range(-1f, 1f));
        return point;
    }    

    public SphericalCoordinates RandomSphericalPointWithinAngle(Vector3 point, float angle)
    {
        float theta = (2f * Mathf.PI) * Random.Range(0f, 1f);
        Vector3 direction = new Vector3(Mathf.Cos(theta), 0, Mathf.Sin(theta));
        Vector3 ortho = Vector3.Cross(new SphericalCoordinates(), point);
        ortho.Normalize();
        Quaternion look_at_ortho = Quaternion.LookRotation(ortho, point);
        return point * Mathf.Cos(angle) + look_at_ortho * direction * Mathf.Sin(angle);
    }

}
