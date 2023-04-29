using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetFailHandler : MonoBehaviour
{
	public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
	{
		if (shutdownReason == ShutdownReason.Ok) return;

		if (shutdownReason == ShutdownReason.HostMigration)
		{
			Debug.Log("Ignore shutdown from host migration");
			return;
		}
		
		if (shutdownReason == ShutdownReason.GameNotFound)
		{
			FindObjectOfType<NetworkDebugStart>().ShutdownAll();
		}
		
	}
}
