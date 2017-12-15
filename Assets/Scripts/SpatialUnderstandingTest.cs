using UnityEngine;
using HoloToolkit.Unity;
using System;

public class SpatialUnderstandingTest : MonoBehaviour {

	public TextMesh textMesh = null;
	public float kMinAreaForComplete = 50.0f;
	public float kMinHorizAreaForComplete = 25.0f;
	public float kMinWallAreaForComplete = 10.0f;

	private void LateUpdate()
	{
		if (DoesScanMeetMinBarForCompletion)
		{
			SpatialUnderstanding.Instance.RequestFinishScan();
		}

		Vector3 rayPos = Camera.main.transform.position;
		Vector3 rayVec = Camera.main.transform.forward * 10.0f;

		IntPtr raycastResultPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticRaycastResultPtr();
		SpatialUnderstandingDll.Imports.PlayspaceRaycast(
			rayPos.x,
			rayPos.y,
			rayPos.z,
			rayVec.x,
			rayVec.y,
			rayVec.z,
			raycastResultPtr);

		SpatialUnderstandingDll.Imports.RaycastResult rayCastResult = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticRaycastResult();
		textMesh.text = rayCastResult.SurfaceType.ToString();
	}
	private bool DoesScanMeetMinBarForCompletion
	{
		get
		{
			if ((SpatialUnderstanding.Instance.ScanState != SpatialUnderstanding.ScanStates.Scanning) ||
				(!SpatialUnderstanding.Instance.AllowSpatialUnderstanding))
			{
				return false;
			}

			IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
			if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) == 0)
			{
				return false;
			}

			SpatialUnderstandingDll.Imports.PlayspaceStats stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();
			if ((stats.TotalSurfaceArea > kMinAreaForComplete) ||
				(stats.HorizSurfaceArea > kMinHorizAreaForComplete) ||
				(stats.WallSurfaceArea > kMinWallAreaForComplete))
			{
				return true;
			}
			return false;
		}
	}
}