using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("GameGenre")]
public partial class GameGenre
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("GameID")]
    public int? GameId { get; set; }

    [Column("GenreID")]
    public int? GenreId { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("GameGenres")]
    public virtual Game Game { get; set; }

    [ForeignKey("GenreId")]
    [InverseProperty("GameGenres")]
    public virtual Genre Genre { get; set; }
}
