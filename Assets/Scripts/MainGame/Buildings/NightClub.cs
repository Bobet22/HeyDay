using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightClub : Building
{
    private void Start()
    {
        this.buildingName = Buildings.NIGHTCLUB;
        this.buildingOpeningTime = 20f;
        this.buildingClosingTime = 5f;

        this.actionButtons = new List<Buttons>(){Buttons.PARTY};
        BuildingManager.Instance.onBuildingBtnClicked += CheckBtnClicked;
    }


    private void OnDestroy()
    {
        BuildingManager.Instance.onBuildingBtnClicked -= CheckBtnClicked;
    }


    public override void CheckBtnClicked(Buttons clickedBtn)
    {
        if (BuildingManager.Instance.CurrentSelectedBuilding.buildingName == this.buildingName)
            switch (clickedBtn)
            {
                case Buttons.PARTY:
                    Debug.Log("party party");
                    break;
            }
    }


    public override void CheckButtons()
    {
    }
}
