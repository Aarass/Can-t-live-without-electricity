
using Assets.Scripts.GeneralPurpose;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlantsManager : SingletonMonoBehaviour<PowerPlantsManager>
{
    [SerializeField]
    List<PowerPlant> powerPlants = new();
    int count = 0;


    public void RegisterNewPowerPlant(PowerPlant powerPlant)
    {
        powerPlants.Add(powerPlant);
    }
    public void EnableOne()
    {
        if (count >= powerPlants.Count) return;

        PowerPlant powerPlant = powerPlants[count];
        Cell cell = GridManager.Instance.WorldToCell(powerPlant.transform.position);
        PowerSource.Collection[cell] = powerPlant;
        cell.x -= 1;
        PowerSource.Collection[cell] = powerPlant;
        cell.z -= 1;
        PowerSource.Collection[cell] = powerPlant;
        cell.x += 1;
        PowerSource.Collection[cell] = powerPlant;

        powerPlant.gameObject.SetActive(true);
        count++;

        Network.Instance.ObserveNewPowerPlant(powerPlant);
    }
    public bool AllEnabled()
    {
        return count == powerPlants.Count;
    }
    public PowerColor RandomColor()
    {
        int index = UnityEngine.Random.Range(0, powerPlants.Count);
        return powerPlants[index].color;
    }
    public PowerColor RandomEnabledColor()
    {
        int index = UnityEngine.Random.Range(0, count);
        return powerPlants[index].color;
    }
    public PowerColor LastAddedColor()
    {
        return powerPlants[count-1].color;
    }
}
