namespace mvc_dev_2017.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Album")]
    public partial class Album
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Album()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        [Display(Name = "Artist")]
        public int AlbumId { get; set; }

        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        public int ArtistId { get; set; }

        [Required]
        [StringLength(160)]
        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        [Column(TypeName = "numeric")]
        [Range(0.01, 100.00, ErrorMessage = "Price must be between 0.01 and 100.00")]
        public decimal Price { get; set; }

        [StringLength(1024)]
        [Display(Name = "Album Art URL")]
        public string AlbumArtUrl { get; set; }

        public virtual Artist Artist { get; set; }

        public virtual Genre Genre { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
