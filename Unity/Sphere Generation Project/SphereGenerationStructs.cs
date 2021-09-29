
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SphericalCoordinates
{
    public float radius;

    //use Theta to use value of theta effectively
    public float theta;
    //use Phi to use value of phi effectively
    public float phi;

    public float Theta
    {
        get
        {
            float pi_theta = theta / Mathf.PI;
            float tau_theta = theta / (Mathf.PI * 2f);
            if (!Mathf.Approximately(theta - Mathf.PI * Mathf.Floor(pi_theta), theta - (Mathf.PI * 2f) * Mathf.Floor(tau_theta)))
            {
                return Mathf.PI - (theta - Mathf.PI * Mathf.Floor(pi_theta));
            }
            return theta - Mathf.PI * Mathf.Floor(pi_theta);
        }

        set
        {
            theta = value;
        }
    }
    public float Phi
    {
        get
        {
            float pi_theta = theta / Mathf.PI;
            float tau_theta = theta / (Mathf.PI * 2f);
            if (Mathf.Approximately(theta - Mathf.PI * Mathf.Floor(pi_theta), theta - (Mathf.PI * 2f) * Mathf.Floor(tau_theta)))
            {
                return phi - (Mathf.PI * 2f) * Mathf.Floor(phi / (Mathf.PI * 2f));
            }
            else
            {
                return (phi + Mathf.PI) - (Mathf.PI * 2f) * Mathf.Floor((phi + Mathf.PI) / (Mathf.PI * 2f));
            }
        }

        set
        {
            phi = value;
        }
    }

    public SphericalCoordinates(float theta = 0, float phi = 0, float radius = 1)
    {
        this.radius = radius;
        this.theta = theta;
        this.phi = phi;
    }
    public SphericalCoordinates(SphericalCoordinates s)
    {
        radius = s.radius;
        theta = s.Theta;
        phi = s.Phi;
    }
    public SphericalCoordinates(Vector3 vector)
    {
        radius = Mathf.Sqrt(Mathf.Pow(vector.x, 2)  + Mathf.Pow(vector.z, 2) + Mathf.Pow(-vector.y, 2));
        theta = Mathf.Acos(-vector.y / radius);
        phi = Mathf.Atan2(vector.z, vector.x);
    }
    public static implicit operator SphericalCoordinates(Vector3 v)
    {
        float radius = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.z, 2) + Mathf.Pow(v.y, 2));
        float theta = Mathf.Acos(v.y / radius);
        float phi = Mathf.Atan2(v.z, v.x);
        return new SphericalCoordinates(theta, phi, radius);
    }
    public static implicit operator Vector3 (SphericalCoordinates s)
    {
        Vector3 vec = new Vector3(
            s.radius * Mathf.Sin(s.Theta) * Mathf.Cos(s.Phi),
            s.radius * Mathf.Cos(s.Theta),
            s.radius * Mathf.Sin(s.Theta) * Mathf.Sin(s.Phi)
            );
        return vec;
    }
    public bool Equals(SphericalCoordinates other)
    {
        return (other.Theta == Theta) && (other.Phi == Phi) && (other.radius == radius);
    }
    public override bool Equals(object other) => other is SphericalCoordinates o && Equals(o);
    public override int GetHashCode()
    {
        return radius.GetHashCode() ^ Phi.GetHashCode() << 4 ^ Theta.GetHashCode() << 2;
    }
    public static bool operator ==(SphericalCoordinates lhs, SphericalCoordinates rhs)
    {
        return (lhs.Theta == rhs.Theta) && (lhs.Phi == rhs.Phi) && (lhs.radius == rhs.radius);
    }
    public static bool operator !=(SphericalCoordinates lhs, SphericalCoordinates rhs)
    {
        return (lhs.Theta != rhs.Theta) || (lhs.Phi != rhs.Phi) || (lhs.radius != rhs.radius);
    }
}
public struct SphereArc
{
    //WARNING: DO NOT MODIFY MEMBERS DIRECTLY, ONLY USE CONSTRUCTOR
    public SphericalCoordinates start;
    public SphericalCoordinates end;
    public Vector3 cross_product;
    public Vector3 span_normal;
    public float angle;

    public SphereArc(Vector3 start, Vector3 end)
    {
        cross_product = Vector3.Cross(start, end);
        span_normal = cross_product.normalized;
        angle = Mathf.Acos(Vector3.Dot(start, end) / (start.magnitude * end.magnitude));
        this.start = start;
        this.end = end;
    }
}

[System.Serializable]
public struct FibbonacciSphere
{
    public SphericalCoordinates[] points;
    public int[] tris;
    public FibbonacciSphere(int size)
    {
        points = new SphericalCoordinates[size];
        tris = null;
    }
}

[System.Serializable]
public struct DelaunayTri
{
    public int a;
    public int b;
    public int c;
    public Edge AB
    {
        get
        {
            return new Edge(a, b);
        }
    }
    public Edge BC
    {
        get
        {
            return new Edge(b, c);
        }
    }
    public Edge CA
    {
        get
        {
            return new Edge(c, a);
        }
    }
    public Vector2 circumcenter;
    public float circumradius;
    public Vector3 centroid;

    public DelaunayTri(int a, int b, int c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        circumcenter = new Vector2(float.NaN, float.NaN);
        circumradius = float.NaN;
        centroid = new Vector3(float.NaN, float.NaN, float.NaN);
    }
    public bool HasEdge(Edge other)
    {
        return AB == other || BC == other || CA == other;
    }
    public bool Equals(DelaunayTri other)
    {
        return other.AB == AB && other.BC == BC && other.CA == CA;
    }
    public override bool Equals(object other) => other is DelaunayTri o && Equals(o);
    public override int GetHashCode()
    {
        return a.GetHashCode() ^ b.GetHashCode() << 4 ^ c.GetHashCode() << 2;
    }
    public static bool operator ==(DelaunayTri rhs, DelaunayTri lhs)
    {
        bool AB = (rhs.AB == lhs.AB) || (rhs.AB == lhs.BC) || (rhs.AB == lhs.CA);
        bool BC = (rhs.BC == lhs.AB) || (rhs.BC == lhs.BC) || (rhs.BC == lhs.CA);
        bool CA = (rhs.CA == lhs.AB) || (rhs.CA == lhs.BC) || (rhs.CA == lhs.CA);
        return AB && BC && CA;
    }
    public static bool operator !=(DelaunayTri rhs, DelaunayTri lhs)
    {
        bool AB = (rhs.AB == lhs.AB) || (rhs.AB == lhs.BC) || (rhs.AB == lhs.CA);
        bool BC = (rhs.BC == lhs.AB) || (rhs.BC == lhs.BC) || (rhs.BC == lhs.CA);
        bool CA = (rhs.CA == lhs.AB) || (rhs.CA == lhs.BC) || (rhs.CA == lhs.CA);
        return !AB || !BC || !CA;
    }
}

[System.Serializable]
public struct Edge
{
    public int point1;
    public int point2;
    public Edge(int p1, int p2)
    {
        point1 = p1;
        point2 = p2;
    }
    public bool Equals(Edge other)
    {
        return (point1 == other.point1 && point2 == other.point2) | (point1 == other.point2 && point2 == other.point1);
    }
    public override bool Equals(object other) => other is Edge o && Equals(o);
    public override int GetHashCode()
    {
        return point1.GetHashCode() ^ point2.GetHashCode();
    }
    public static bool operator ==(Edge lhs, Edge rhs)
    {
        return (lhs.point1 == rhs.point1 && lhs.point2 == rhs.point2) || (lhs.point1 == rhs.point2 && lhs.point2 == rhs.point1);
    }
    public static bool operator !=(Edge lhs, Edge rhs)
    {
        if(lhs.point1 != rhs.point1 && lhs.point1 != rhs.point2)
        {
            return true;
        }
        else
        {
            return !((lhs.point1 == rhs.point1 && lhs.point2 == rhs.point2) || (lhs.point1 == rhs.point2 && lhs.point2 == rhs.point1));
        }
    }
}
