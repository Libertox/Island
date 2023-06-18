
using Island.InteractObject;

namespace Island.UI
{
    public class FurnaceUI : CookingUI
    {
        public override void Start()
        {
            Furnace.OnInteract += CookingItem_OnInteract;
            base.Start();
        }

        private void OnDestroy() => Furnace.OnInteract -= CookingItem_OnInteract;

    }
}