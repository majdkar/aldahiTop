using System.Collections.Generic;

namespace FirstCall.Domain.Contracts
{
    public interface IEntityWithExtendedAttributes<TExtendedAttribute>
    {
        public ICollection<TExtendedAttribute> ExtendedAttributes { get; set; }
    }
}