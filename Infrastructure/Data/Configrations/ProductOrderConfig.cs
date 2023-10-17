using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configrations
{
    internal class ProductOrderConfig : IEntityTypeConfiguration<ProductOrder>
    {
        public void Configure(EntityTypeBuilder<ProductOrder> builder)
        {
            builder.OwnsOne(order => order.ShipedToAddress,o=>o.WithOwner());

            builder.Property(O => O.OrderStatus)
                   .HasConversion
               (
                Status => Status.ToString(),
                Value =>(OrderStatus) Enum.Parse(typeof(OrderStatus),Value)
               ) ;
            builder.HasMany(O => O.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
