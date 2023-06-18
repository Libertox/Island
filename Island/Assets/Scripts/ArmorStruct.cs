using Island.Item;

namespace Island
{

    [System.Serializable]
    public struct Armor
    {
        public ItemInstance helmet;
        public ItemInstance body;
        public ItemInstance shoes;
        public ItemInstance hand;
        public BackpackInstance backpack;

        public void ReduceArmorStrength()
        {
            DecreaseArmorStrength(ref helmet);
            DecreaseArmorStrength(ref body);
            DecreaseArmorStrength(ref shoes);
            DecreaseArmorStrength(ref hand);
        }

        private void DecreaseArmorStrength(ref ItemInstance item)
        {
            if (item != null)
            {
                item.strength--;
                if (item.strength <= 0)
                {
                    item.itemSO = null;
                    item = null;
                }
            }
        }
    }
}