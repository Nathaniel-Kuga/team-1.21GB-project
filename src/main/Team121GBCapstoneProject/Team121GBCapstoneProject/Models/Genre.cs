using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("Genre")]
public partial class Genre
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(64)]
    public string Name { get; set; }

    [InverseProperty("Genre")]
    public virtual ICollection<GameGenre> GameGenres { get; } = new List<GameGenre>();
}
