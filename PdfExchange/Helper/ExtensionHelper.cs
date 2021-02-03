using System.Collections;
using System.Linq;
using System.Windows.Controls;

namespace PdfExchange.Helper
{
    public static class ExtensionHelper
    {
        private static bool IsStringEmpty(this string item)
        {
            return string.IsNullOrWhiteSpace(item);
        }

        private static bool IsEnumerableEmpty(this IEnumerable item)
        {
            return FormControlHelper.ConvertToList(item).Any() == false;
        }

        public static bool IsObjectEmpty(this object item)
        {
            return item == null || ((item as ItemCollection)?.IsEnumerableEmpty() ?? item.ToString().IsStringEmpty());
        }
    }
}
