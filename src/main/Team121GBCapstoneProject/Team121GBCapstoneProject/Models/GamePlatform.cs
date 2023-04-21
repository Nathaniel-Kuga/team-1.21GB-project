using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("GamePlatform")]
public partial class GamePlatform
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("GameID")]
    public int? GameId { get; set; }

    [Column("PlatformID")]
    public int? PlatformId { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("GamePlatforms")]
    public virtual Game Game { get; set; }

    [ForeignKey("PlatformId")]
    [InverseProperty("GamePlatforms")]
    public virtual Platform Platform { get; set; }
}
