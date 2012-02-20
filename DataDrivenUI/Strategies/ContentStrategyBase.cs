namespace DataDrivenUI.Strategies
{
    public abstract class ContentStrategyBase
    {
        public abstract bool Applies(TemplateContext context);
        public abstract void Apply(TemplateContext context);
    }
}
