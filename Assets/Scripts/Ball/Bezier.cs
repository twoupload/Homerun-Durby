using System.Collections.Generic;
using UnityEngine;

// 베지어 곡선에 관련된 스크립트
// 최초 작성자 : 이상도
// 수정자: 이상도
// 최종 수정일: 2025-04-28

public class Bezier
{
    ////////////////////////////////////////////
    // Component
    public List<Vector3> PathPoints;
	private int m_Segments;
	private const int m_TotalPoints = 100;

    ///////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////
    // Bezier function
    ///////////////////////////////////////////////////////////////

	// init Bezier
    public Bezier(List<Vector3> controlPoints)
	{

		PathPoints = new List<Vector3>();
		CreateCurve(controlPoints);
	}

	// Create Curve path
	private void CreateCurve(List<Vector3> controlPoints)
	{

		m_Segments = controlPoints.Count / 3;


		for (int s = 0; s < controlPoints.Count - 3; s += 3)
		{
			Vector3 p0 = controlPoints[s];
			Vector3 p1 = controlPoints[s + 1];
			Vector3 p2 = controlPoints[s + 2];
			Vector3 p3 = controlPoints[s + 3];

			if (s == 0)
			{
				PathPoints.Add(BezierPathCalculation(p0, p1, p2, p3, 0.0f));
			}

			float total_interval = m_TotalPoints / m_Segments;
			for (int p = 0; p < total_interval; p++)
			{
				PathPoints.Add(BezierPathCalculation(p0, p1, p2, p3, 1.0f / total_interval * p));
			}
		}
	}

	// calculation Bezier Curve
	private Vector3 BezierPathCalculation(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		float tt = t * t;   // t^2
		float ttt = t * tt; // t^3
		float u = 1.0f - t; // 1-t
		float uu = u * u;   // (1-t)^2
		float uuu = u * uu; // (1-t)^3

		//B =[(1-t)^3 * p0] + [3*(1-t)^2 * t * p1] + [3*(1-t) * t^2 * p2] + [(1-t)^3 * p3]
		Vector3 B = uuu * p0;
		B += 3.0f * uu * t * p1;    //3*(1-t)^2 * t * p1
		B += 3.0f * u * tt * p2;    //3*(1-t) * t^2 * p2
		B += ttt * p3;              //(1-t)^3 * p3

		return B;
	}
}
