using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("Platform")]
public partial class Platform
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(64)]
    public string Name { get; set; }

    [InverseProperty("Platform")]
    public virtual ICollection<GamePlatform> GamePlatforms { get; } = new List<GamePlatform>();
}
