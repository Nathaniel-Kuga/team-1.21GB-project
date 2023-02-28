using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("Person")]
public partial class Person
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("AuthorizationID")]
    public int? AuthorizationId { get; set; }

    [StringLength(64)]
    public string? FirstName { get; set; }

    [StringLength(64)]
    public string? LastName { get; set; }

    [StringLength(64)]
    public string? Username { get; set; }

    [StringLength(64)]
    public string? Email { get; set; }

    [StringLength(256)]
    public string? ProfilePicture { get; set; }

    [StringLength(526)]
    public string? ProfileBio { get; set; }

    [Column("RoleID")]
    public int? RoleId { get; set; }
}
