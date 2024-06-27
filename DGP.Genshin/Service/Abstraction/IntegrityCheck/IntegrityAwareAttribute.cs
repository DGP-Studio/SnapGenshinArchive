namespace DGP.Genshin.Service.Abstraction.IntegrityCheck
{
    /// <summary>
    /// 指示此属性需要受到完整性检查
    /// 完整性检查主要负责集合项目的Source检查
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class IntegrityAwareAttribute : Attribute
    {
    }
}