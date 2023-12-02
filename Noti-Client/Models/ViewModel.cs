using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Noti_Client.Models
{
    public class ViewModel
    {
        [Key]
        public int ID { get; set; }
        public DateTime? Posted { get; set; }

        public string? Description { get; set; }

        public string? Information { get; set; }

        public int? GroupID { get; set; }
        [ForeignKey("GroupID")]
        public virtual Group? Group { get; set; }

        public string? UserName { get; set; }
        public List<SelectListItem>? GroupList { get; set; }
        public int? SelectedId { get; set; }
    }
}
