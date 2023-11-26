using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bank : Building
{
    private void Start()
    {
        this.buildingStringName = "Intelli Cash Bank";
        this.buildingEnumName = Buildings.BANK;
        this.buildingOpeningTime = 8f;
        this.buildingClosingTime = 16f;

        BuildingManager.Instance.onBuildingBtnClicked += CheckBtnClicked;
    }


    private void OnDestroy()
    {
        BuildingManager.Instance.onBuildingBtnClicked -= CheckBtnClicked;
    }


    public override void CheckBtnClicked(Buttons clickedBtn)
    {
        if (BuildingManager.Instance.CurrentSelectedBuilding.buildingEnumName == this.buildingEnumName)
            switch (clickedBtn)
            {
                case Buttons.OPENSAVINGSACCOUNT:
                    BankSystemManager.Instance.CreateSavingsAcc();
                    break;
                case Buttons.ACCESSSAVINGSACCOUNT:
                    BankSystemManager.Instance.OpenBankSystem();
                    break;
                case Buttons.APPLY:
                    JobManager.Instance.Apply(this);
                    break;
                case Buttons.WORK:
                    JobManager.Instance.Work();
                    break;
                case Buttons.QUIT:
                    JobManager.Instance.QuitWork();
                    break;
            }
    }


    public override void CheckButtons()
    {
        this.actionButtons = new List<Buttons>(){Buttons.OPENSAVINGSACCOUNT, Buttons.ACCESSSAVINGSACCOUNT, Buttons.APPLY};

        if (this.currentlyHired)
        {
            this.actionButtons.Add(Buttons.WORK);
            this.actionButtons.Add(Buttons.QUIT);   
        }
    }
}
