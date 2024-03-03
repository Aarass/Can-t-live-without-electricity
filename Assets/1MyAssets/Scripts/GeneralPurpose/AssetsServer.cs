using Assets.Scripts.GeneralPurpose;
using UnityEngine;

public class AssetsServer : MonoBehaviour
{
    public static AssetsServer Instance;
    private void Awake() => Instance = this;

    [SerializeField] public GameObject shortWire;
    [SerializeField] public GameObject longWire;
    [SerializeField] public Material pipeMaterial;
    [SerializeField] public Mesh ring;

    [SerializeField] public GameObject rocks;
    [SerializeField] public GameObject hole;

    [SerializeField] public GameObject redHouse;
    [SerializeField] public GameObject yellowHouse;
    [SerializeField] public GameObject blueHouse;
    [SerializeField] public GameObject purpleHouse;
    [SerializeField] public GameObject greenHouse;

    [SerializeField] public GameObject signPrefab;
    public GameObject HousePrefab(PowerColor clr)
    {
        switch (clr)
        {
            case PowerColor.Gray:
                return null;
            case PowerColor.Red:
                return redHouse;
            case PowerColor.Yellow:
                return yellowHouse;
            case PowerColor.Blue:
                return blueHouse;
            case PowerColor.Purple:
                return purpleHouse;
            case PowerColor.Green:
                return greenHouse;
        }
        return null;
    }
}