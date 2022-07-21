using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerData : MonoBehaviour
{
    [Networked] public NetworkString<_32> Name { get; set; }

	[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
	public void RPC_SetName(NetworkString<_32> name)
	{
		if (name == "") Name = "Sin Nombre";
		else
		Name = name;
	}
}
