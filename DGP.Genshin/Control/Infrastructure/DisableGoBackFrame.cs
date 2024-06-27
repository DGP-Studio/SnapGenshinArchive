namespace DGP.Genshin.Control.Infrastructure
{
    /// <summary>
    /// 禁用了返回事件的 <see cref="ModernWpf.Controls.Frame"/>
    /// </summary>
    public class DisableGoBackFrame : ModernWpf.Controls.Frame
    {
        /// <summary>
        /// 构造一个新的 <see cref="DisableGoBackFrame"/> 实例
        /// </summary>
        public DisableGoBackFrame()
        {
            // remove all command bindings to prevent hotkey go back.
            CommandBindings.Clear();
        }
    }
}