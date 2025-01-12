using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace FluentValidationExample.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        [HttpPost("Hello")]
        public IActionResult Hello(HelloModel helloModel)
        {
            //validador
            var validator = new HelloModelValidator() 
                                .Validate(helloModel);

            //si es valido
            if (validator.IsValid is true)
                return Content($"Hello {helloModel.Name}");


            //si no es valido
            var errorList = new Dictionary<string, List<string>>();
            validator.Errors.GroupBy(error => error.PropertyName).ToList()
                            .ForEach(group =>
                            {
                                errorList.Add(group.Key,    
                                              group.Select(x=> x.ErrorMessage) 
                                                    .ToList() 
                                );
                            }); 

            return BadRequest(errorList);

        }
    } 

    public class HelloModel
    {
        public string? Name { get; set; }

        public string? Email { get; set; }
    }


    public class HelloModelValidator : AbstractValidator<HelloModel>
    {
        public HelloModelValidator()
        {
            // Reglas de validación

            //Name
            RuleFor(x => x.Name).NotNull().WithMessage("Name is required");

            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is empty")
                                .When(x => x.Name is not null);


            //Email
            RuleFor(x => x.Email).NotNull().WithMessage("Email is required");


            RuleFor( x=> x.Email).NotEmpty().WithMessage("Email is empty")
                                 .EmailAddress().WithMessage("Email is not valid")
                                 .When(x => x.Email is not null);
        }

    }
   
}
