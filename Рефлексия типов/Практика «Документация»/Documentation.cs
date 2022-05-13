using System;
using System.Linq;
using System.Reflection;

namespace Documentation
{

    public class Specifier<T> : ISpecifier
    {
        public string GetApiDescription()
        {
            return typeof(T)
                .GetCustomAttributes(true)
                .OfType<ApiDescriptionAttribute>()
                .FirstOrDefault()?
                .Description;
        }
        public string[] GetApiMethodNames()
        {
            return typeof(T)
                .GetTypeInfo()
                .GetMethods()
                .Where(m => m.GetCustomAttribute<ApiDescriptionAttribute>() != null)
                .Where(m => m.GetCustomAttribute<ApiMethodAttribute>() != null)
                .Select(x => x.Name)
                .ToArray();
        }

        public string GetApiMethodDescription(string methodName)
        {
            var result1 = typeof(T).GetMethod(methodName);
            if (result1 == null) return null;

            var result2 = result1
                .GetCustomAttributes(true)
                .OfType<ApiDescriptionAttribute>()
                .FirstOrDefault()?
                .Description;

            return result2;
        }

        public string[] GetApiMethodParamNames(string methodName)
        {
            var result1 = typeof(T).GetMethod(methodName);
            if (result1 == null) return null;

            var result2 = result1
                .GetParameters()
                .Select(x => x.Name)
                .ToArray();

            return result2;
        }

        public string GetApiMethodParamDescription(string methodName, string paramName)
        {
            var result1 = typeof(T).GetMethod(methodName);
            if (result1 == null) return null;

            var result2 = result1
                .GetParameters();

            var result3 = result2.Where(x => x.Name == paramName)
                .FirstOrDefault();

            if (result3 == null) return null;

            var result4 = result3.GetCustomAttributes(true)
                .OfType<ApiDescriptionAttribute>()
                .FirstOrDefault()?
                .Description;

            return result4;
        }

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
        {
            var result = new ApiParamDescription
            {
                ParamDescription = new CommonDescription(paramName)
            };

            var desiredMethod = typeof(T).GetMethod(methodName);
            if (desiredMethod != null && desiredMethod.GetCustomAttributes(true).OfType<ApiMethodAttribute>().Count() > 0)
            {
                var desiredParameter = desiredMethod
                    .GetParameters()
                    .Where(param => param.Name == paramName);

                if (desiredParameter.Count() > 0)
                {
                    var descriptionAttribute = desiredParameter
                        .First()
                        .GetCustomAttributes(true)
                        .OfType<ApiDescriptionAttribute>()
                        .FirstOrDefault();

                    if (descriptionAttribute != null)
                        result.ParamDescription.Description = descriptionAttribute.Description;

                    var attributeValidation = desiredParameter
                        .First()
                        .GetCustomAttributes(true)
                        .OfType<ApiIntValidationAttribute>()
                        .FirstOrDefault();

                    if (attributeValidation != null)
                    {
                        result.MinValue = attributeValidation.MinValue;
                        result.MaxValue = attributeValidation.MaxValue;
                    }

                    var requiredAttribute = desiredParameter
                        .First()
                        .GetCustomAttributes(true)
                        .OfType<ApiRequiredAttribute>()
                        .FirstOrDefault();

                    if (requiredAttribute != null)
                        result.Required = requiredAttribute.Required;
                }
            }

            return result;
        }

        public ApiMethodDescription GetApiMethodFullDescription(string methodName)
        {
            var desiredMethod = typeof(T).GetMethod(methodName);
            if (desiredMethod == null) return null;
            var methodAttributes = desiredMethod.GetCustomAttributes(true).OfType<ApiMethodAttribute>();
            if (methodAttributes.Count() <= 0) return null;
            var result = new ApiMethodDescription
            {
                MethodDescription = new CommonDescription(methodName, GetApiMethodDescription(methodName)),
                ParamDescriptions = GetApiMethodParamNames(methodName).Select(param => GetApiMethodParamFullDescription(methodName, param)).ToArray()
            };

            var returnParameter = desiredMethod.ReturnParameter;
            bool returnParameterbool = false;
            var returnParamDiscription = new ApiParamDescription();
            returnParamDiscription.ParamDescription = new CommonDescription();
            var descriptionAttribute = returnParameter.GetCustomAttributes(true).OfType<ApiDescriptionAttribute>().FirstOrDefault();
            if (descriptionAttribute != null)
            {
                returnParamDiscription.ParamDescription.Description = descriptionAttribute.Description;
                returnParameterbool = true;
            }

            var apiAttribute = returnParameter
                .GetCustomAttributes(true)
                .OfType<ApiIntValidationAttribute>()
                .FirstOrDefault();

            if (apiAttribute != null)
            {
                returnParamDiscription.MinValue = apiAttribute.MinValue;
                returnParamDiscription.MaxValue = apiAttribute.MaxValue;
                returnParameterbool = true;
            }

            var requiredAttribute = returnParameter.GetCustomAttributes(true).OfType<ApiRequiredAttribute>().FirstOrDefault();
            if (requiredAttribute != null)
            {
                returnParamDiscription.Required = requiredAttribute.Required;
                returnParameterbool = true;
            }

            if (returnParameterbool)
                result.ReturnDescription = returnParamDiscription;

            return result;
        }
    }
}