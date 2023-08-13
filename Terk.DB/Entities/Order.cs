namespace Terk.DB.Entities;

[Table("order")]
public partial class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("customer_id")]
    public int CustomerId { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("total_cost", TypeName = "money")]
    public decimal TotalCost { get; set; }

    [ForeignKey("CustomerId")]
    [InverseProperty("Orders")]
    public virtual User Customer { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderPosition> OrderPositions { get; set; } = new List<OrderPosition>();
}
