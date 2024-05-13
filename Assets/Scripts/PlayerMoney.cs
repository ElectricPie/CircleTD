using System;
using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField] private float m_startingMoney = 20;
    
    // Using a float to allow for multiplication of money increases
    public float Amount { get; private set; }

    private void Awake()
    {
        Amount = m_startingMoney;
    }

    /// <summary>
    /// Adds the amount to the players money
    /// </summary>
    /// <param name="amountToAdd">How much money to add to the player</param>
    public void AddMoney(float amountToAdd)
    {
        Amount += amountToAdd;
    }

    /// <summary>
    /// Remove the amount from the player if they have the amount 
    /// </summary>
    /// <param name="amountToRemove">The amount of money to remove from the player</param>
    /// <returns>True if the money is removed otherwise false</returns>
    public bool RemoveMoney(float amountToRemove)
    {
        if (Amount < amountToRemove)
        {
            return false;
        }

        Amount -= amountToRemove;
        return true;
    }
}
