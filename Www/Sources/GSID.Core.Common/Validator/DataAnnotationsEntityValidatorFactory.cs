
namespace GSID.Core.Common.Validator
{
    /// <summary>
    /// Data Annotations based entity validator factory
    /// </summary>
    public class DataAnnotationsEntityValidatorFactory
        : IEntityValidatorFactory
    {
        /// <summary>
        /// Create a entity validator
        /// </summary>
        /// <returns></returns>
        public IEntityValidator Create()
        {
            return new DataAnnotationsEntityValidator();
        }
    }
}
