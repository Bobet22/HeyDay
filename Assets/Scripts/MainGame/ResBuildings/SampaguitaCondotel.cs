using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampaguitaCondotel : ResBuilding
{
    private void Start()
    {
        this.buildingEnumName = ResBuildings.SAMPAGUITACONDOTEL;
        this.buildingNameStr = "Sampaguita Condotel";
        this.monthlyRent = 10500f;
        this.monthlyElecCharge = 1500f;
        this.monthlyWaterCharge = 500f;
        this.dailyAdtnlHappiness = 17f; 
        this.adtnlEnergyForSleep = 2f;

        this.actionButtons = new List<Buttons>(){Buttons.SLEEP, Buttons.EAT};
        BuildingManager.Instance.onBuildingBtnClicked += CheckBtnClicked;
    }


    private void OnDestroy()
    {
        BuildingManager.Instance.onBuildingBtnClicked -= CheckBtnClicked;
    } 


    public override void Sleep()
    {
        SleepManager.Instance.ShowSleepOverlay(this.adtnlEnergyForSleep);
    }


    public override void Eat()
    {

    }


    public override void CheckBtnClicked(Buttons clickedBtn)
    {
        if (Player.Instance.CurrentPlayerPlace == this)
            switch (clickedBtn)
            {
                case Buttons.SLEEP:
                    this.Sleep();
                    break;
                case Buttons.EAT:
                    Debug.Log("i'll be eating");
                    break;
            }
    }
}
