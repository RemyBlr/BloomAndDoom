using UnityEngine;

public static class SaveSystem
{

    // Checks if weapon is unlocked
    public static bool IsWeaponUnlocked(string className, string weaponName)
    {
        // 0 is locked and 1 is unlocked
        return PlayerPrefs.GetInt($"weapon_{className}_{weaponName}", 0) == 1;
    }

    // Unlcok weapons and save to PlayerPrefs
    public static void UnlockWeapon(string className, string weaponName)
    {
        string key = $"weapon_{className}_{weaponName}";
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
    }
}
