using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("PersonList")]
public partial class PersonList
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("PersonID")]
    public int PersonId { get; set; }

    [Column("ListKindID")]
    public int? ListKindId { get; set; }

    [StringLength(50)]
    public string ListKind { get; set; }

    [ForeignKey("ListKindId")]
    [InverseProperty("PersonLists")]
    public virtual ListKind ListKindNavigation { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PersonLists")]
    public virtual Person Person { get; set; }

    [InverseProperty("PersonList")]
    public virtual ICollection<PersonGame> PersonGames { get; } = new List<PersonGame>();
}
