using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PowerPlantsManager powerPlantsManager;
    HousesManager housesManager;
    private void Start()
    {
        powerPlantsManager = PowerPlantsManager.GetInstance();
        housesManager = HousesManager.GetInstance();
    }
    public void NextTurn()
    {
        if(!powerPlantsManager.AllEnabled())
            powerPlantsManager.EnableOne();
        if(!housesManager.AllEnabled())
            housesManager.EnableTwo();
    }
    private void Update()
    {

    }
}
