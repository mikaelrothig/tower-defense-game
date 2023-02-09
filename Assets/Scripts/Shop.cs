using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	public TurretBlueprint standardTurretPrefab;
	public TurretBlueprint MissileTurretPrefab;

	public Text moneyCounter;

	BuildManager buildManager;

	void Start()
	{
		buildManager = BuildManager.instance;
	}

	void Update()
	{
		moneyCounter.text = "$" + PlayerStats.money;
	}

	public void SelectStandardTurret()
	{
		buildManager.SelectTurretToBuild(standardTurretPrefab);
	}

	public void SelectMissileTurret()
	{
		buildManager.SelectTurretToBuild(MissileTurretPrefab);
	}
}
