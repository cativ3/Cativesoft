using System;

namespace Cativesoft.Entities.Abstractions
{
    public abstract class BaseEntity<T> : IEntity
    {
        public T Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
