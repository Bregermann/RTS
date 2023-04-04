using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    // Initial resource values
    public int dung = 100;

    public int bananas = 50;
    public int elderberries = 25;

    // Functions to add/subtract resources
    public void AddDung(int amount)
    {
        dung += amount;
    }

    public void SubtractDung(int amount)
    {
        dung -= amount;
    }

    public void AddBananas(int amount)
    {
        bananas += amount;
    }

    public void SubtractBananas(int amount)
    {
        bananas -= amount;
    }

    public void AddElderberries(int amount)
    {
        elderberries += amount;
    }

    public void SubtractElderberries(int amount)
    {
        elderberries -= amount;
    }

    // Check if player has enough resources
    public bool HasEnoughDung(int amount)
    {
        return dung >= amount;
    }

    public bool HasEnoughBananas(int amount)
    {
        return bananas >= amount;
    }

    public bool HasEnoughElderberries(int amount)
    {
        return elderberries >= amount;
    }
}