using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerAPI
{
    public class StringToArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            //Target model must of type Enumerable
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            string valueFromBody = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();

            if (string.IsNullOrEmpty(valueFromBody))
            {
                return Task.CompletedTask;
            }
            var splitData = valueFromBody.Split(new char[] { ',' }).Select(x => Guid.Parse(x));

            bindingContext.Result = ModelBindingResult.Success(splitData);
            return Task.CompletedTask;
        }
    }
}
