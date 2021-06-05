using System.ComponentModel.DataAnnotations;

namespace Postera.WebApp.Models
{
    public class OrderStatusRequestModel
    {
        [Required] 
        public string Id { get; set; }
    }
}