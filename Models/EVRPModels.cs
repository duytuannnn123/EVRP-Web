using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EVRP_Web.Models
{
    [Table("nodes")]
    public class Node
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string? NodeId { get; set; }

        public string? NodeName { get; set; }

        [Required]
        public double X { get; set; }

        [Required]
        public double Y { get; set; }

        public double? Demand { get; set; }

        public string? NodeType { get; set; } 

        public int? StartTime { get; set; }

        public int? EndTime { get; set; }

        public int? ServiceTime { get; set; } = 10;
    }

    public class Route
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public int DriverId { get; set; }
        public string DriverName { get; set; } = string.Empty;
        public double TotalDistance { get; set; }
        public double TotalTravelTime { get; set; }
        public double RemainingBattery { get; set; }
    }

    [Table("Drivers")]
    public class Driver
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(50)]
        public string VehicleLicense { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    [Table("Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [Required]
        public double Latitude { get; set; } 

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Demand { get; set; }

        [StringLength(50)]
        public string TimeWindow { get; set; } = "08:00 - 17:00";

        // Trường đồng bộ trạng thái lưu cứng xuống MySQL
        public bool IsDelivered { get; set; } = false;

        [StringLength(20)]
        public string? ActualDeliveryTime { get; set; }
    }
}