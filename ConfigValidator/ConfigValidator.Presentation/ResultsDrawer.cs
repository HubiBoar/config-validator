using System.Text;
using ConfigValidator.Contracts;
using ModulR.Validation;
using ModulR.Validation.Abstraction;
using MoreLinq;
using OneOf;
using OneOf.Types;
using Result = ConfigValidator.Presentation.IValidationResultDrawer.Result;

namespace ConfigValidator.Presentation;

internal class ResultsDrawer : IValidationResultDrawer
{
    public Result DrawResults(IReadOnlyCollection<ValidationResult> validationResults)
    {
        var resultBuilder = new StringBuilder();

        AddHeader(validationResults, resultBuilder);

        AddSummary(validationResults, resultBuilder);

        AddErrors(validationResults, resultBuilder);

        return new Result(resultBuilder.ToString(), GetStatus(validationResults));
    }

    private static void AddHeader(
        IReadOnlyCollection<ValidationResult> validationResults,
        StringBuilder resultBuilder)
    {
        resultBuilder.AppendLine("Keys Validation Result: ");
        resultBuilder.AppendLine($"Results Count: {validationResults.Count}");
    }
    
    private static void AddSummary(
        IReadOnlyCollection<ValidationResult> validationResults,
        StringBuilder resultBuilder)
    {
        var allKeysSummary = GetKeysSummary(validationResults);

        resultBuilder.AppendLine($"\tSummary Count: {allKeysSummary.Count}");
        resultBuilder.AppendLine(CombineLines("\t- ", allKeysSummary));
    }

    private static void AddErrors(
        IReadOnlyCollection<ValidationResult> validationResults,
        StringBuilder resultBuilder)
    {
        var errorLines = GetErrorLines("\t  ", validationResults);

        resultBuilder.AppendLine($"\tErrors Count: {errorLines.Count}");
        resultBuilder.AppendLine(CombineLines("\t- ", errorLines));
    }

    private static IReadOnlyCollection<string> GetKeysSummary(
        IReadOnlyCollection<ValidationResult> validationResults)
    {
        return validationResults
            .Select(r => r.Result.Match<string>(
                valid => valid.Result.Match<string>(
                    success => Successful(valid.Mapping),
                    valueNotFound => FailedNotFound(valid.Mapping),
                    validationErrors => FailedValidationErrors(valid.Mapping),
                    methodNotFound => FailedMethodNotFound(valid.Mapping)),
                InValidMapping))
            .ToArray();
    }

    private static IReadOnlyCollection<string> GetErrorLines(
        string errorPrefix,
        IReadOnlyCollection<ValidationResult> validationResults)
    {
        return validationResults
            .Select(r => r.Result.Match<OneOf<string, IReadOnlyCollection<string>, Success>>(
                valid => valid.Result.Match<OneOf<string, IReadOnlyCollection<string>, Success>>(
                    success => success,
                    valueNotFound => FailedNotFound(valid.Mapping),
                    validationErrors => FailedValidationErrors(errorPrefix, valid.Mapping, validationErrors),
                    methodNotFound => FailedMethodNotFound(valid.Mapping)),
                inValid => InValidMapping(errorPrefix, inValid)))
            .Where(x => x.IsT0)
            .Select(x => x.AsT0)
            .ToArray();
    }

    private static string MappingPrefix(Mapping mapping)
    {
        return $"[{mapping.KeyName.Name}: {mapping.MethodName.Name}]";
    }

    private static string Successful(Valid<Mapping> mapping)
    {
        return $"{MappingPrefix(mapping.ValidValue)} Validation Successful";
    }
    
    private static string FailedNotFound(Valid<Mapping> mapping)
    {
        return $"{MappingPrefix(mapping.ValidValue)} Validation Failed: ValueNotFound";
    }
    
    private static string FailedValidationErrors(Valid<Mapping> mapping)
    {
        return $"{MappingPrefix(mapping.ValidValue)} Validation Failed: ValidationErrors";
    }

    private static string FailedMethodNotFound(Valid<Mapping> mapping)
    {
        return $"{MappingPrefix(mapping.ValidValue)} Validation Failed: MethodNotFound";
    }
    
    private static string InValidMapping(InValid<Mapping> mapping)
    {
        return $"{MappingPrefix(mapping.InValidValue)} Validation Failed: Mapping is Invalid";
    }

    private static string FailedValidationErrors(
        string errorPrefix,
        Valid<Mapping> mapping,
        ValidationErrors validationErrors)
    {
        var resultBuilder = new StringBuilder();

        resultBuilder.AppendLine($"{FailedValidationErrors(mapping)}: {validationErrors.ErrorMessages.Count}");

        foreach (var error in validationErrors.ErrorMessages)
        {
            resultBuilder.AppendLine($"{errorPrefix}- {error}");
        }

        return resultBuilder.ToString();
    }
    
    private static string InValidMapping(
        string errorPrefix,
        InValid<Mapping> mapping)
    {
        var resultBuilder = new StringBuilder();

        var validationErrors = mapping.ValidationErrors;
        resultBuilder.AppendLine($"{InValidMapping(mapping)}: {validationErrors.ErrorMessages.Count}");

        foreach (var error in validationErrors.ErrorMessages)
        {
            resultBuilder.AppendLine($"{errorPrefix}- {error}");
        }

        return resultBuilder.ToString();
    }

    private static OneOf<Success, Error> GetStatus(
        IReadOnlyCollection<ValidationResult> validationResults)
    {
        return validationResults.All(x => x.Result.IsT0) ? new Success() : new Error();
    }

    private static string CombineLines(
        string prefix,
        IReadOnlyCollection<string> lines)
    {
        var stringBuilder = new StringBuilder();

        lines.ForEach(line =>
        {
            if (string.IsNullOrEmpty(line) == false)
            {
                stringBuilder.AppendLine($"{prefix}{line}");
            }
        });
        return stringBuilder.ToString();
    }
}