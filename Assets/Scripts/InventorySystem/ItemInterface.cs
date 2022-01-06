/* Defines Functions for Items
 Usage
 - Inherit from IItem
 - Implement Overrides
*/

namespace InventorySystem
{
    public interface IItem
    {
        public void PrimaryUse();

        public void SecondaryUse();
    }
}
