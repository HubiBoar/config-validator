namespace ConfigValidator.Presentation;

public static class ValidationResultDrawerFactory
{
    public static IValidationResultDrawer Create()
    {
        return new ResultsDrawer();
    }
}