using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

public static class PlaceGameObject 
{
    private static float m_objScale = 0.008f;
	private static float m_distanceToHand = 0.4f;	// distance between head and hand
	 

	// place an cube object infront of main camera
	public static GameObject PlaceCubeAtInfront() 
	{
		GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
		_PlaceObjAtInfrontOfCamera (obj);
		return obj;
	}

	// place an object infront of main camera
	public static GameObject PlaceObjectAtInfront(GameObject obj) 
	{
		_PlaceObjAtInfrontOfCamera (obj);
		// place object at little lower than eye pos
		obj.transform.position = new Vector3 (obj.transform.position.x, obj.transform.position.y - 0.05f, obj.transform.position.z);
		// look at camera
		obj.transform.LookAt (Camera.main.transform.position);
		return obj;
	}

	// place an cube object at the place cursor hits
	public static GameObject PlaceObjectAtGazePos()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _PlaceObjAtGazePos(obj);
		return obj;
    }

	// place an object at the place cursor hits
	public static GameObject PlaceObjectAtGazePos(GameObject obj)
	{
		_PlaceObjAtGazePos(obj);
		return obj;
	}

	// place an text at the place cursor hits
	public static GameObject PlaceObjectAtGazePos(string str)
    {
        GameObject obj = new GameObject("textmesh");
        obj.AddComponent<TextMesh>().text = str;
        _PlaceObjAtGazePos(obj);
		return obj;
    }


	private static GameObject _PlaceObjAtGazePos (GameObject obj)
    {
        Vector3 hitpos = GazeManager.Instance.HitPosition;
		return _SetObjectTransform (obj, hitpos);
    }

	private static GameObject _PlaceObjAtInfrontOfCamera (GameObject obj)
	{
		Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * m_distanceToHand;
		return _SetObjectTransform (obj, pos);
	}

	private static GameObject _SetObjectTransform (GameObject obj, Vector3 pos) {
		obj.transform.localScale = new Vector3(m_objScale, m_objScale, m_objScale);
		obj.transform.position = pos;
		obj.transform.rotation = Camera.main.transform.rotation;
		return obj;
	}
}
