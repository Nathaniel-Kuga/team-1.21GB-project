using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("Game")]
public partial class Game
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(64)]
    public string Title { get; set; }

    public string Description { get; set; }

    public int? YearPublished { get; set; }

    [Column("ESRBRatingID")]
    public int? EsrbratingId { get; set; }

    public double? AverageRating { get; set; }

    public string CoverPicture { get; set; }

    [Column("IGDBUrl")]
    public string Igdburl { get; set; }

    [Column("IGDBGameID")]
    public int? IgdbgameId { get; set; }

    [ForeignKey("EsrbratingId")]
    [InverseProperty("Games")]
    public virtual Esrbrating Esrbrating { get; set; }

    [InverseProperty("Game")]
    public virtual ICollection<GameGenre> GameGenres { get; } = new List<GameGenre>();

    [InverseProperty("Game")]
    public virtual ICollection<GamePlatform> GamePlatforms { get; } = new List<GamePlatform>();

    [InverseProperty("Game")]
    public virtual ICollection<PersonGame> PersonGames { get; } = new List<PersonGame>();
}
