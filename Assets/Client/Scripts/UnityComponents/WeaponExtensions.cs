using LeopotamGroup.Globals;

namespace LD52
{
    static class WeaponExtensions
    {
        public static WeaponData GetData(this IWeapon weapon)
        {
            if (weapon == default)
            {
                return default;
            }
            
            var staticData = Service<StaticData>.Get();
            var item = staticData.AllItems.GetItemById(weapon.DataID);
            return item as WeaponData;
        }
    }
}