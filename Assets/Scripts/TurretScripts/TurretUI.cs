using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretUI : MonoBehaviour {

	public GameObject ui;
	private Cube target;
	public Text upgradeCost;
	public Button upgradeButton;
	public Text sellAmount;

	public void SetTarget(Cube target)
	{
		this.target = target;

		transform.position = target.GetBuildPosition();
		

		if(!target.isUpgraded)
		{
			upgradeCost.text = "Upgrade\n$" + target.turretBlueprint.upgradeCost;
			upgradeButton.interactable = true;
		}
			
		else
		{
			upgradeCost.text = "MAX";
			upgradeButton.interactable = false;
		}

		sellAmount.text = "Sell\n$" + target.turretBlueprint.GetSellAmount();	

		ui.SetActive(true);
	}

	public void Hide()
	{
		ui.SetActive(false);
	}

	public void Upgrade()
	{
		target.UpgradeTurret();
		BuildManager.instance.DeselectCube();
	}

	public void Sell()
	{
		target.SellTurret();
		BuildManager.instance.DeselectCube();
	}
}
