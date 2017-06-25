using System.Web;
using System.Web.Mvc;

namespace EvidencijaSoftvera_IlijaDivljan
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
