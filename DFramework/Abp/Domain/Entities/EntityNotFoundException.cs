using System;
using System.Runtime.Serialization;

namespace Abp.Domain.Entities
{
    /// <summary>
    ///     This exception is thrown if an entity excepted to be found but not found.
    /// </summary>
    [Serializable]
    public class EntityNotFoundException : AbpException
    {
        /// <summary>
        ///     Creates a new <see cref="EntityNotFoundException" /> object.
        /// </summary>
        public EntityNotFoundException()
        {
        }

        /// <summary>
        ///     Creates a new <see cref="EntityNotFoundException" /> object.
        /// </summary>
        /// <param name="serializationInfo"></param>
        /// <param name="context"></param>
        public EntityNotFoundException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        ///     Creates a new <see cref="EntityNotFoundException" /> objects
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="id"></param>
        public EntityNotFoundException(Type entityType, object id)
            : this(entityType, id, null)
        {
        }

        /// <summary>
        ///     Creates a new <see cref="EntityNotFoundException" /> object.
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="id"></param>
        /// <param name="innerException"></param>
        public EntityNotFoundException(Type entityType, object id, Exception innerException)
            : base($"There is no such an entity.Entity Type:{entityType.FullName},id:{id}", innerException)
        {
            EntityType = entityType;
            Id = id;
        }

        /// <summary>
        ///     Creates a new <see cref="EntityNotFoundException" /> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public EntityNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Creates a new <see cref="EntityNotFoundException" /> objects
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public EntityNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Type of the entity.
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        ///     Id of the Entity.
        /// </summary>
        public object Id { get; }
    }
}