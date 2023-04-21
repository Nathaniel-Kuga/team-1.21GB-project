using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Team121GBNUnitTest
{
    public class ModelValidator
    {
        public List<ValidationResult> Validations { get; private set; }
        public bool Valid { get; private set; } = false;

        public ModelValidator(object model)
        {
            Validations = new List<ValidationResult>();
            ValidationContext vctx = new ValidationContext(model);
            Valid = Validator.TryValidateObject(model, vctx, Validations, true);
        }

        public bool ContainsFailuerFor(string member)
        {
            return Validations.Any(vr => vr.MemberNames.Contains(member));
        }

        public string GetAllErrors() =>
            Validations.Aggregate("", (accumulator, validaiton) => accumulator + $",{validaiton.ErrorMessage}");
    }
}
