using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("GamePlayListType")]
public partial class GamePlayListType
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(64)]
    public string ListKind { get; set; }

    [InverseProperty("ListKind")]
    public virtual ICollection<PersonGameList> PersonGameLists { get; } = new List<PersonGameList>();
}
