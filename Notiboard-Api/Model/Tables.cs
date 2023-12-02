using Microsoft.AspNetCore.Mvc.Rendering;
using Notiboard_Api.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Notiboard_Api.Model
{
    public class Board
    {
        [Key]
        public int ID { get; set; }
        public DateTime? Posted { get; set; }

        public string? Description { get; set; }

        public string? Information { get; set; } 

        public int? GroupID { get; set; }
        [ForeignKey("GroupID")]
        public virtual Group? Group { get; set; }

        // if  i get identiy working in time then ill save the user name here so that the user who posted this reocrd id can be showwed 
        // be sure to make user sign in work with user email and NOT USER NAME
        public string? UserName { get; set; }

    }

    public class Group
    {
        [Key]
        public int ID { get; set; }

        // Three Groups 
        public string? Name { get; set; }
    }
}
//Featues id like to add//
// Sort the list by the Group selected either by a filter or if thats too hard just add a button that shows record with that group tag
// like using an http get to get data withh say group tag amber or group tag network support
// a chat bubble style design maybe something kinda simple yet modern with light colors and simple ui


//Later note if i get this done quicker then i think (x for doubt) add something tha can allow user to add polls as well
