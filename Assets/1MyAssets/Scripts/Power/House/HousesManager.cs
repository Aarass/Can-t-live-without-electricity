using Assets.Scripts.GeneralPurpose;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HousesManager : SingletonMonoBehaviour<HousesManager>
{
    SparseMatrix<House> Disabled = new(Map.Width, Map.Height);
    SparseMatrix<House> Enabled = new(Map.Width, Map.Height);

    HouseAccumulator houseAccumulator = new();
    public void EnableTwo()
    {
        if(!PowerPlantsManager.GetInstance().AllEnabled())
        {
            PowerColor color = PowerPlantsManager.GetInstance().LastAddedColor();
            houseAccumulator.GetHouse(color, out House house1, out House house2);
            EnableHouse(house1);
            EnableHouse(house2);
        }
        else
        {
            EnableHouse(Disabled.ValueAt(UnityEngine.Random.Range(0, Disabled.Count)));
            EnableHouse(Disabled.ValueAt(UnityEngine.Random.Range(0, Disabled.Count)));
        }
    }
    private void EnableHouse(House house)
    {
        if (Disabled.Count == 0) return;

        GameObject housePrefab = AssetsServer.Instance.HousePrefab(house.color);
        Vector3 position = house.transform.position;
        Instantiate(housePrefab, position, Quaternion.identity);

        Enabled[house.cell] = house;
        Disabled.Remove(house.cell);

        house.EnableGeometry();
    }
    public bool AllEnabled()
    {
        return Disabled.Count == 0;
    }
    public bool AllConnected()
    {
        foreach(House house in Enabled)
            if (!house.IsPowered)
                return false;
        return true;
    }
    public void RegisterNewHouse(House house)
    {
        Disabled[house.cell] = house;
        houseAccumulator.RegisterNewHouse(house);
    }
}
