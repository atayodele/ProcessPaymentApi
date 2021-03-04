using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Models
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }

    public abstract class BaseEntity<T> : IEntity<T>
    {
        public virtual T Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;
        public DateTimeOffset? ModifiedOn { get; set; }
    }
    public abstract class BaseEntity : BaseEntity<Guid>
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
        }
    }
}
