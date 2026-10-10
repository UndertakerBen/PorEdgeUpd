using System.Globalization;

namespace PortableEdge
{
    internal static class UiLanguage
    {
        public static void UseEnglish()
        {
            // Keep number/date formatting regional; application UI is always English.
            var english = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = english;
            CultureInfo.CurrentUICulture = english;
        }
    }
}
