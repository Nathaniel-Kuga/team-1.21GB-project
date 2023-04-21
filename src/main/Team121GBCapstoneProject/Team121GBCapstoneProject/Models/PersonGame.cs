using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("PersonGame")]
public partial class PersonGame
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("PersonListID")]
    public int PersonListId { get; set; }

    [Column("GameID")]
    public int GameId { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("PersonGames")]
    public virtual Game Game { get; set; }

    [ForeignKey("PersonListId")]
    [InverseProperty("PersonGames")]
    public virtual PersonList PersonList { get; set; }
}
