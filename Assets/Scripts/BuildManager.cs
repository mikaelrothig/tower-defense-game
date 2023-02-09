using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour {
	
	public static BuildManager instance;

	void Awake()
	{
		instance = this;
	}

	public GameObject standardTurretPrefab;
	public GameObject MissileTurretPrefab;
	private TurretBlueprint turretToBuild;
	private Cube selectedCube;

	public TurretUI turretUI;

	public GameObject buildEffect;
	public GameObject sellEffect;
	
	public bool canBuild { get {return turretToBuild != null; } }

	public bool hasMoney { get {return PlayerStats.money >= turretToBuild.cost; } }

	public void SelectCube(Cube cube)
	{
		if(selectedCube == cube)
		{
			DeselectCube();
			return;
		}

		selectedCube = cube;
		turretToBuild = null;

		turretUI.SetTarget(cube);
	}
	public void DeselectCube()
	{
		selectedCube = null;
		turretUI.Hide();
	}

	public void SelectTurretToBuild(TurretBlueprint turret)
	{
		turretToBuild = turret;
		DeselectCube();
	}

	public TurretBlueprint GetTurretToBuild()
	{
		return turretToBuild;
	}
}
