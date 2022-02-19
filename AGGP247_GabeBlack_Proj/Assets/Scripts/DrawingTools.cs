using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingTools : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float V3ToAngle(Vector3 startPoint, Vector3 endPoint)
	{
        float start = Mathf.Atan2(startPoint.y, startPoint.x);
        float end = Mathf.Atan2(endPoint.y, endPoint.x);

        float result = Mathf.Atan2(start, end);

        return (result * (180 / Mathf.PI));
	}

    public static float LineToAngle(Line line)
	{
        return V3ToAngle(line.start, line.end);
	}

    public static Vector3 RotatePoint(Vector3 Center, float angle, Vector3 pointIN)
	{
        float radAngle = Mathf.Deg2Rad * angle;

        pointIN -= Center;

        float xNew = pointIN.x * Mathf.Cos(radAngle) - pointIN.y * Mathf.Sin(radAngle);
        float yNew = pointIN.x * Mathf.Sin(radAngle) + pointIN.y * Mathf.Cos(radAngle);

        pointIN = Center + new Vector3(xNew, yNew);

        return pointIN;
    }

    public static Vector3 CircleRadiusPoint(Vector3 Origin, float angle, float radius)
	{
        Vector3 returnPoint = Origin + new Vector3(radius, 0, 0);

        returnPoint = RotatePoint(Origin, angle, returnPoint);

        return returnPoint;
	}

    /*public static Vector3 EllipseRadiusPoint(Vector3 Origin, float angle, float radius)
    {

    }*/

    public static float ToDegrees(float radians)
	{
        return Mathf.Rad2Deg * radians;
	}

    public static float ToRadians(float degrees)
    {
        return Mathf.Deg2Rad * degrees;
    }
}
