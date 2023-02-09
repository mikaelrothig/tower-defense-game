using UnityEngine;

public class Cube : MonoBehaviour {

	public Color enoughMoneyColor;
	public Color notEnoughMoneyColor;
	private Renderer rend;
	private Color startColor;

	public Vector3 positionOffset;

	[HideInInspector]
	public GameObject turret;
	[HideInInspector]
	public TurretBlueprint turretBlueprint;
	[HideInInspector]
	public bool isUpgraded = false;

	BuildManager buildManager;

	void Start()
	{
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;

		buildManager = BuildManager.instance;
	}

	public Vector3 GetBuildPosition()
	{
		return transform.position + positionOffset;
	}

	void OnMouseDown()
	{
		if(turret != null)
		{
			buildManager.SelectCube(this);
			return;
		}

		if(!buildManager.canBuild)
			return;
		
		BuildTurret(buildManager.GetTurretToBuild());
	}

	void BuildTurret(TurretBlueprint blueprint)
	{
		if(PlayerStats.money < blueprint.cost)
		{
			return;
		}

		PlayerStats.money -= blueprint.cost;
		GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
		turret = _turret;
		isUpgraded = false;

		turretBlueprint = blueprint;

		GameObject buildEff = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(buildEff, 3f);
	}

	public void UpgradeTurret()
	{
		if(PlayerStats.money < turretBlueprint.upgradeCost)
		{
			return;
		}

		PlayerStats.money -= turretBlueprint.upgradeCost;
		Destroy(turret);

		GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
		turret = _turret;

		GameObject buildEff = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(buildEff, 3f);

		isUpgraded = true;
	}

	public void SellTurret()
	{
		PlayerStats.money += turretBlueprint.GetSellAmount();
		Destroy(turret);

		GameObject sellEff = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
		Destroy(sellEff, 3f);
		
		turretBlueprint = null;
	}

	void OnMouseEnter()
	{
		if(!buildManager.canBuild)
			return;

		if(buildManager.hasMoney)
			rend.material.color = enoughMoneyColor;
		else
			rend.material.color = notEnoughMoneyColor;
	}

	void OnMouseExit()
	{
		rend.material.color = startColor;
	}
}
