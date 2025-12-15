using System.ComponentModel.DataAnnotations;

namespace spoty_clon_backend.Utils
{
    public class ModelAttributes
    {
        /// <summary>
        ///     Atributo de campo de modelo con los posibles tipos de filtrado que tiene
        /// </summary>
      

        /// <summary>
        ///     Atributo de campo de modelo que determina que es un campo por el cual se puede ordenar su entidad
        /// </summary>
        public class SortableAttribute : Attribute
        {
            /// <summary>
            ///     Constructor por defecto
            /// </summary>
            public SortableAttribute()
            {
            }
        }

        /// <summary>
        ///     Atributo de campo de modelo que determina que es un campo por el cual se puede ordenar su entidad
        /// </summary>
        public class ImportAttribute : Attribute
        {
            /// <summary>
            ///     Constructor por defecto
            /// </summary>
            public ImportAttribute()
            {

            }
        }

        public class ValidEnumAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null)
                {
                    return ValidationResult.Success;
                }

                var type = value.GetType();

                if (!(type.IsEnum && Enum.IsDefined(type, value)))
                {
                    return new ValidationResult(ErrorMessage ?? $"{value} is not a valid value for type {type.Name}");
                }

                return ValidationResult.Success;
            }
        }
    }
}
