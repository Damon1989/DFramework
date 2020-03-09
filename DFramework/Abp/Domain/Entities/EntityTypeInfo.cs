using System;

namespace Abp.Domain.Entities
{
    public class EntityTypeInfo
    {
        public EntityTypeInfo(Type entityType, Type declaringType)
        {
            EntityType = entityType;
            DeclaringType = declaringType;
        }

        /// <summary>
        ///     Type of the entity.
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        ///     DbContext type that has DbSet property.
        /// </summary>
        public Type DeclaringType { get; }
    }
}