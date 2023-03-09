using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("ListName")]
public partial class ListName
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(64)]
    public string NameOfList { get; set; }

    [InverseProperty("ListName")]
    public virtual ICollection<PersonGameList> PersonGameLists { get; } = new List<PersonGameList>();
}
