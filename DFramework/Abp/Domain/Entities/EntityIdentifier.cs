using System;

namespace Abp.Domain.Entities
{
    [Serializable]
    public class EntityIdentifier
    {
        /// <summary>
        /// Entity Type
        /// </summary>
        public Type Type { get; }
        /// <summary>
        /// Entity's Id
        /// </summary>
        public object Id { get; }

        /// <summary>
        /// Add for serialization purposes.
        /// </summary>
        public EntityIdentifier()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityIdentifier"/> class.
        /// </summary>
        /// <param name="type">Entity type.</param>
        /// <param name="id">Id of the entity.</param>
        public EntityIdentifier(Type type,object id)
        {
            if (type==null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            Type = type;
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }
    }
}