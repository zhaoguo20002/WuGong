using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;

public class Statics {
	static tk2dCamera tkCamera;
	/// <summary>
	/// Inits the camera.
	/// </summary>
	public static void InitCamera() {
		tkCamera = Camera.main.GetComponent<tk2dCamera>();
	}
	/// <summary>
	/// Gets the resolution.
	/// </summary>
	/// <returns>The resolution.</returns>
	public static Vector2 GetResolution() {
		return tkCamera.TargetResolution;
	}

	/// <summary>
	/// Gets the index of the map col.
	/// </summary>
	/// <returns>The map col index.</returns>
	/// <param name="x">The x coordinate.</param>
	public static int GetMapColIndex(float x) {
		return (int)((x / 0.32f) + 0.01f);
	}

	/// <summary>
	/// Gets the index of the map row.
	/// </summary>
	/// <returns>The map row index.</returns>
	/// <param name="y">The y coordinate.</param>
	public static int GetMapRowIndex(float y) {
		return (int)((y / 0.32f) + 0.01f);
	}

	/// <summary>
	/// Gets the map x.
	/// </summary>
	/// <returns>The map x.</returns>
	/// <param name="col">Col.</param>
	public static float GetMapX(int col) {
		return col * 0.32f + 0.16f;
	}

	/// <summary>
	/// Gets the map y.
	/// </summary>
	/// <returns>The map y.</returns>
	/// <param name="row">Row.</param>
	public static float GetMapY(int row) {
		return row * 0.32f + 0.16f;
	}

	/// <summary>
	/// Gets the map reset position.
	/// </summary>
	/// <returns>The map reset position.</returns>
	/// <param name="position">Position.</param>
	public static Vector3 GetMapResetPosition(Vector3 position) {
		return new Vector3(GetMapX(GetMapColIndex(position.x)), GetMapY(GetMapRowIndex(position.y)), position.z);
	}

	static float eps = 1e-6f;
	static int sgn(float x)  
	{  
		return x < -eps ? -1 : 1;  
	} 
	static float cross(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)  
	{  
		return (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);  
	}
	static float area(Vector3 p1, Vector3 p2, Vector3 p3)  
	{  
		return cross(p1, p2, p1, p3);  
	}
	static float farea(Vector3 p1, Vector3 p2, Vector3 p3)  
	{  
		return Mathf.Abs(area(p1, p2, p3));  
	}
	/// <summary>
	/// Determines if is line meeting the specified p1 p2 p3 p4.
	/// </summary>
	/// <returns><c>true</c> if is line meeting the specified p1 p2 p3 p4; otherwise, <c>false</c>.</returns>
	/// <param name="p1">P1.</param>
	/// <param name="p2">P2.</param>
	/// <param name="p3">P3.</param>
	/// <param name="p4">P4.</param>
	public static bool IsLineMeeting(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
		return Mathf.Max(Mathf.Min(p1.x, p2.x), Mathf.Min(p3.x, p4.x)) <= Mathf.Min(Mathf.Max(p1.x, p2.x), Mathf.Max(p3.x, p4.x))  
			&& Mathf.Max(Mathf.Min(p1.y, p2.y), Mathf.Min(p3.y, p4.y)) <= Mathf.Min(Mathf.Max(p1.y, p2.y), Mathf.Max(p3.y, p4.y))  
				&& sgn(cross(p3, p2, p3, p4) * cross(p3, p4, p3, p1)) >= 0  
				&& sgn(cross(p1, p4, p1, p2) * cross(p1, p2, p1, p3)) >= 0; 
	}
	/// <summary>
	/// PL the specified P11, P12, P21 and P22.
	/// </summary>
	/// <param name="P11">P11.</param>
	/// <param name="P12">P12.</param>
	/// <param name="P21">P21.</param>
	/// <param name="P22">P22.</param>
	public static Vector3 GetIntersection(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
	{
		float k = farea(p1, p2, p3) / farea(p1, p2, p4);  
		return new Vector3((p3.x + k * p4.x) / (1 + k), (p3.y + k * p4.y) / (1 + k));
	}

	/// <summary>
	/// 静态方法反射
	/// </summary>
	/// <param name="className"></param>
	/// <param name="methodName"></param>
	/// <param name="param"></param>
	public static bool CallStaticMethod(string className, string methodName, object[] param = null) {
		Type t = Type.GetType(className);
		if (t == null) {
			return false;
		}
		MethodInfo method = t.GetMethod(methodName);
		if (method != null) { 
			method.Invoke(null, param);
			return true;
		}
		return false;
	}
	
	/// <summary>
	/// 公共方法反射
	/// </summary>
	/// <param name="thisObj"></param>
	/// <param name="methodName"></param>
	/// <param name="param"></param>
	public static void CallPublicMethod(object thisObj, string methodName, object[] param) {
		Type t = thisObj.GetType();
		if (t == null) {
			return;
		}
		MethodInfo method = t.GetMethod(methodName);
		if (method != null) {
			method.Invoke(thisObj, param);
		}
	}

	/// <summary>
	/// Gets the obstacle.
	/// </summary>
	/// <returns>The obstacle.</returns>
	/// <param name="sceneName">Scene name.</param>
	public static Dictionary<string, bool> GetObstacle(string sceneName) {
		Type t = Type.GetType("Game.GetObstacle");
		if (t == null) {
			return null;
		}
		MethodInfo method = t.GetMethod("Get" + sceneName + "Obstacle");
		if (method != null) {
			return (Dictionary<string, bool>)method.Invoke(null, null);
		}
		return null;
	}

	/// <summary>
	/// Gets the angle.
	/// </summary>
	/// <returns>The angle.</returns>
	/// <param name="x1">The first x value.</param>
	/// <param name="y1">The first y value.</param>
	/// <param name="x2">The second x value.</param>
	/// <param name="y2">The second y value.</param>
	public static float GetAngle(float x1, float y1, float x2, float y2) {
		float angle = Mathf.Atan2(x2 - x1, y2 - y1) / Mathf.PI * 180;
		return angle >= 0 ? angle : angle + 360;
	}
	/// <summary>
	/// Gets the radian.
	/// </summary>
	/// <returns>The radian.</returns>
	/// <param name="angle">Angle.</param>
	public static float GetRadian(float angle) {
		return Mathf.PI / 180 * angle;
	}
	/// <summary>
	/// Gets the line aim by points.
	/// </summary>
	/// <returns>The line aim by points.</returns>
	/// <param name="x1">The first x value.</param>
	/// <param name="y1">The first y value.</param>
	/// <param name="x2">The second x value.</param>
	/// <param name="y2">The second y value.</param>
	/// <param name="lineHeight">Line height.</param>
	public static Vector2 GetLineAimByPoints(float x1, float y1, float x2, float y2, float lineHeight) {
		return GetLineAimByAngle(GetAngle(x1, y1, x2, y2), lineHeight);
	}
	/// <summary>
	/// Gets the line aim by angle.
	/// </summary>
	/// <returns>The line aim by angle.</returns>
	/// <param name="angle">Angle.</param>
	/// <param name="lineHeight">Line height.</param>
	public static Vector2 GetLineAimByAngle(float angle, float lineHeight) {
		return GetLineAimByRadian(GetRadian(angle), lineHeight);
	}
	/// <summary>
	/// Gets the line aim by radian.
	/// </summary>
	/// <returns>The line aim by radian.</returns>
	/// <param name="radian">Radian.</param>
	/// <param name="lineHeight">Line height.</param>
	public static Vector2 GetLineAimByRadian(float radian, float lineHeight) {
		return new Vector2(lineHeight * Mathf.Sin(radian), lineHeight * Mathf.Cos(radian));
	}
	
	/// <summary>
	/// Gets the circle point.
	/// </summary>
	/// <returns>The circle point.</returns>
	/// <param name="p">P.</param>
	/// <param name="r">The red component.</param>
	/// <param name="angle">Angle.</param>
	public static Vector2 GetCirclePoint(Vector2 p, float r, float angle) {
		return new Vector2(p.x + r * Mathf.Cos(angle * Mathf.PI / 180), p.y + r * Mathf.Sin(angle * Mathf.PI / 180));
	}

	/// <summary>
	/// 返回Resource路径下某个预设的克隆
	/// </summary>
	/// <param name="path">Resource路径</param>
	/// <returns>GameObject对象</returns>
	public static GameObject GetPrefabClone(string path) {
		return MonoBehaviour.Instantiate(Statics.GetPrefab(path)) as GameObject;
	}

	/// <summary>
	/// Gets the prefab clone.
	/// </summary>
	/// <returns>The prefab clone.</returns>
	/// <param name="clone">Clone.</param>
	public static GameObject GetPrefabClone(UnityEngine.Object clone) {
		return MonoBehaviour.Instantiate(clone) as GameObject;
	}

	/// <summary>
	/// Gets the prefab.
	/// </summary>
	/// <returns>The prefab.</returns>
	/// <param name="path">Path.</param>
	public static UnityEngine.Object GetPrefab(string path) {
		return Resources.Load(path, typeof(GameObject));
	}
	
	/// <summary>
	/// Gets the distance points.
	/// </summary>
	/// <returns>The distance points.</returns>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="distance">Distance.</param>
	public static List<Vector3> GetDistancePoints(Vector3 from, Vector3 to, float distance) {
		List<Vector3> points = new List<Vector3>();
		float num = Mathf.Ceil((Vector3.Distance(from, to) / distance) + 0.5f);
		float indexNum = 0;
		Vector3 cut = new Vector3((to.x - from.x) / num, from.y, (to.z - from.z) / num);
		while(indexNum++ < num - 1) {
			points.Add(new Vector3(from.x + cut.x * indexNum, cut.y, from.z + cut.z * indexNum));
		}
		points.Add(to);
		return points;
	}

	/// <summary>
	/// Gets the bullet layer.
	/// </summary>
	/// <returns>The bullet layer.</returns>
	public static int GetBulletLayer() {
		return 11;
	}

	/// <summary>
	/// Gets the halo layer.
	/// </summary>
	/// <returns>The halo layer.</returns>
	public static int GetHaloLayer() {
		return 12;
	}

	/// <summary>
	/// Gets the effect layer.
	/// </summary>
	/// <returns>The effect layer.</returns>
	public static int GetEffectLayer() {
		return 13;
	}

	/// <summary>
	/// Gets the flare layer.
	/// </summary>
	/// <returns>The flare layer.</returns>
	public static int GetFlareLayer() {
		return 14;
	}

	/// <summary>
	/// Determines if is pointer over U.
	/// </summary>
	/// <returns><c>true</c> if is pointer over U; otherwise, <c>false</c>.</returns>
	public static bool IsPointerOverUI() {
		return IPointerOverUI.Instance.IsPointerOverUIObject();
	}
}
