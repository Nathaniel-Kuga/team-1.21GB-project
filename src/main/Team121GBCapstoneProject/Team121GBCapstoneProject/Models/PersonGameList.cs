using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("PersonGameList")]
public partial class PersonGameList
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("PersonID")]
    public int? PersonId { get; set; }

    [Column("GameID")]
    public int? GameId { get; set; }

    [Column("ListKindID")]
    public int ListKindId { get; set; }

    [Column("ListNameID")]
    public int ListNameId { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("PersonGameLists")]
    public virtual Game Game { get; set; }

    [ForeignKey("ListKindId")]
    [InverseProperty("PersonGameLists")]
    public virtual GamePlayListType ListKind { get; set; }

    [ForeignKey("ListNameId")]
    [InverseProperty("PersonGameLists")]
    public virtual ListName ListName { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PersonGameLists")]
    public virtual Person Person { get; set; }
}
