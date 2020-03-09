using JetBrains.Annotations;

namespace Abp.Domain.Entities
{
    /// <summary>
    /// Some useful extension methods for Entities.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        ///  Check if this Entity is null or marked as deleted.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [ContractAnnotation("null => true")]
        public static bool IsNullOrDelete(this ISoftDelete entity)
        {
            return entity == null || entity.IsDeleted;
        }
    }
}