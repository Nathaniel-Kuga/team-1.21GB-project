using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("ListKind")]
public partial class ListKind
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string Kind { get; set; }

    [InverseProperty("ListKindNavigation")]
    public virtual ICollection<PersonList> PersonLists { get; } = new List<PersonList>();
}
