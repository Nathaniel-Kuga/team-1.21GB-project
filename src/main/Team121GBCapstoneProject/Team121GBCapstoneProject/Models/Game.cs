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

    [StringLength(526)]
    public string Description { get; set; }

    public int? YearPublished { get; set; }

    [Column("ESRBRatingID")]
    public int? EsrbratingId { get; set; }

    public double? AverageRating { get; set; }

    [StringLength(526)]
    public string CoverPicture { get; set; }

    [InverseProperty("Game")]
    public virtual ICollection<PersonGameList> PersonGameLists { get; } = new List<PersonGameList>();
}
