using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HouseAccumulator
{
    House[] redHouses;
    House[] yellowHouses;
    House[] blueHouses;
    House[] purpleHouses;
    House[] greenHouses;

    int rhc;
    int yhc;
    int bhc;
    int phc;
    int ghc;

    public HouseAccumulator()
    {
        redHouses = new House[2];
        yellowHouses = new House[2];
        blueHouses = new House[2];
        purpleHouses = new House[2];
        greenHouses = new House[2];

        rhc = 0;
        yhc = 0;
        bhc = 0;
        phc = 0;
        ghc = 0;
    }
    public void RegisterNewHouse(House house)
    {
        switch (house.color)
        {
            case PowerColor.Red:
                if(rhc < 2)
                    redHouses[rhc++] = house;
                break;
            case PowerColor.Yellow:
                if (yhc < 2)
                    yellowHouses[yhc++] = house;
                break;
            case PowerColor.Blue:
                if (bhc < 2)
                    blueHouses[bhc++] = house;
                break;
            case PowerColor.Purple:
                if (phc < 2)
                    purpleHouses[phc++] = house;
                break;
            case PowerColor.Green:
                if (ghc < 2)
                    greenHouses[ghc++] = house;
                break;
        }
    }
    public void GetHouse(PowerColor color, out House house1, out House house2)
    {
        switch (color)
        {
            case PowerColor.Red:
                house1 = redHouses[0];
                house2 = redHouses[1];
                break;
            case PowerColor.Yellow:
                house1 = yellowHouses[0];
                house2 = yellowHouses[1];
                break;
            case PowerColor.Blue:
                house1 = blueHouses[0];
                house2 = blueHouses[1];
                break;
            case PowerColor.Purple:
                house1 = purpleHouses[0];
                house2 = purpleHouses[1];
                break;
            case PowerColor.Green:
                house1 = greenHouses[0];
                house2 = greenHouses[1];
                break;
            default:
                house1 = null;
                house2 = null;
                break;
        }
    }
}