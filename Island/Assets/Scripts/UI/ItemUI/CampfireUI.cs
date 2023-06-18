
using Island.InteractObject;

namespace Island.UI
{
    public class CampfireUI : CookingUI
    {
        public override void Start()
        {
            Campfire.OnInteract += CookingItem_OnInteract;
            base.Start();

        }
        private void OnDestroy() => Campfire.OnInteract -= CookingItem_OnInteract;

    }
}
