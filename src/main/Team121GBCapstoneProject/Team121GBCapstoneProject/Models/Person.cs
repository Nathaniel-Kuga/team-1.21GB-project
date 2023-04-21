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
    [StringLength(450)]
    public string AuthorizationId { get; set; }

    [InverseProperty("Person")]
    public virtual ICollection<PersonList> PersonLists { get; } = new List<PersonList>();
}
