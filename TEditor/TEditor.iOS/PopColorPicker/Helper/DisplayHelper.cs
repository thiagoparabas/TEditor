using UIKit;


namespace PopColorPicker.iOS
{
    public static class DisplayHelper
    {
        public static bool UserInterfaceIdiomIsPhone => UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;

        public static bool Is4InchDisplay()
        {
            return UIScreen.MainScreen.Bounds.Size.Height >= 568f;
        }
    }
}

