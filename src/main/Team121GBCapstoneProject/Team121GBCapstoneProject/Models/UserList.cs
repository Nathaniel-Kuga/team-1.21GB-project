using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Team121GBCapstoneProject.Models;

[Table("UserList")]
public partial class UserList
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(64)]
    public string Title { get; set; }

    public int PersonId { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("UserLists")]
    public virtual Person Person { get; set; }

    [InverseProperty("CompletedList")]
    public virtual ICollection<Person> PersonCompletedLists { get; } = new List<Person>();

    [InverseProperty("CurrentlyPlayingList")]
    public virtual ICollection<Person> PersonCurrentlyPlayingLists { get; } = new List<Person>();

    [InverseProperty("WantToPlayList")]
    public virtual ICollection<Person> PersonWantToPlayLists { get; } = new List<Person>();

    [ForeignKey("UserListId")]
    [InverseProperty("UserLists")]
    public virtual ICollection<Game> Games { get; } = new List<Game>();
}
