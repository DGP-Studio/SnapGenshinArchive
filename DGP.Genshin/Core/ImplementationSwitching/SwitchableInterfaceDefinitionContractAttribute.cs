namespace DGP.Genshin.Core.ImplementationSwitching
{
    /// <summary>
    /// 合约特性 指示此接口用于定义可切换的实现类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface)]
    public class SwitchableInterfaceDefinitionContractAttribute : Attribute
    {
    }
}