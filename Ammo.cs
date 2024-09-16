using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private static Ammo _instance;
    public static Ammo Instance {  get { return _instance; } }
    [SerializeField] AmmoSlots[] _ammoSlots;
    [System.Serializable]
    class AmmoSlots
    {
        public AmmoType AmmoType;
        public int AmmoAmount;
    }
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        _ammoSlots = new AmmoSlots[Enum.GetValues(typeof(AmmoType)).Length];
        for ( int i = 0; i < _ammoSlots.Length; i++)
        {
             _ammoSlots[i] = new AmmoSlots();
            _ammoSlots[i].AmmoType = (AmmoType)i;
        }

    }
    public int GetCurrentAmmo(AmmoType ammoType)
    {
        return GetAmmoSlots(ammoType).AmmoAmount;
    }
    public void ReduceAmmo(AmmoType ammoType, int ammoAmount)
    {
        GetAmmoSlots(ammoType).AmmoAmount-=ammoAmount;
    }
    public void IncreaseAmmo(AmmoType ammoType, int ammoAmount)
    {
        GetAmmoSlots(ammoType).AmmoAmount = GetAmmoSlots(ammoType).AmmoAmount + ammoAmount;
    }

    private AmmoSlots GetAmmoSlots(AmmoType ammoType)
    {
        foreach(AmmoSlots slots in _ammoSlots)
        {
            if (slots.AmmoType == ammoType)
            {
                return slots;
            }
        }
        return null;
    }
}