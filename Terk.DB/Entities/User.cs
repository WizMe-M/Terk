using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Terk.DB.Entities;

[Table("user")]
[Index("Login", Name = "uq_user_login", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(12)]
    public string Name { get; set; } = null!;

    [Column("login")]
    [StringLength(20)]
    [Unicode(false)]
    public string Login { get; set; } = null!;

    [InverseProperty("Customer")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
