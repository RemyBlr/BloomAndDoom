using UnityEngine;

public static class SaveSystem
{

    // Checks if weapon is unlocked
    public static bool IsWeaponUnlocked(string className, string weaponName)
    {
        return PlayerPrefs.GetInt($"{className}_{weaponName}_Unlocked", 0) == 1;
    }

    // Unlcok weapons and save to PlayerPrefs
    public static void UnlockWeapon(string className, string weaponName)
    {
        PlayerPrefs.SetInt($"{className}_{weaponName}_Unlocked", 1);
        PlayerPrefs.Save();
    }
}
