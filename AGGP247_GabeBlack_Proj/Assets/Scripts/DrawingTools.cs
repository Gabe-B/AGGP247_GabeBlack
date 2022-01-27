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
        float xNew = pointIN.x * Mathf.Cos(angle) - pointIN.y * Mathf.Sin(angle);
        float yNew = pointIN.x * Mathf.Sin(angle) + pointIN.y * Mathf.Cos(angle);

        return new Vector3(xNew, yNew, 0);
    }
}
