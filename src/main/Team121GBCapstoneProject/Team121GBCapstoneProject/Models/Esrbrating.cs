using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("ESRBRating")]
public partial class Esrbrating
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("ESRBRatingName")]
    [StringLength(4)]
    public string EsrbratingName { get; set; }

    [Column("IGDBRatingValue")]
    public int? IgdbratingValue { get; set; }

    [InverseProperty("Esrbrating")]
    public virtual ICollection<Game> Games { get; } = new List<Game>();
}
