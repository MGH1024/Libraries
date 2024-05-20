using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MGH.Core.CrossCutting.Localizations.ModelBinders;

public class DateTimeModelBinderProvider:IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var modelType = context.Metadata.UnderlyingOrModelType;

        if (modelType == typeof(DateTime) || modelType == typeof(DateTime?))
        {
            return new UtcAwareDateTimeModelBinder();
        }
      

        return null;
    }
}