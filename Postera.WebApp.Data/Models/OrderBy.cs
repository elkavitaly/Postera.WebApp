using Postera.WebApp.Data.Enums;

namespace Postera.WebApp.Data.Models
{
    public class OrderBy
    {
        public string Field { get; set; }

        public SortDirection SortDirection { get; set; }
    }
}