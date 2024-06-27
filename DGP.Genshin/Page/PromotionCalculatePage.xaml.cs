using DGP.Genshin.Control.Infrastructure.Concurrent;
using DGP.Genshin.ViewModel;
using Snap.Core.DependencyInjection;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace DGP.Genshin.Page
{
    /// <summary>
    /// 养成计算页面
    /// </summary>
    [View(InjectAs.Transient)]
    internal partial class PromotionCalculatePage : AsyncPage
    {
        /// <summary>
        /// 构造一个新的养成计算页面
        /// </summary>
        /// <param name="vm">视图模型</param>
        public PromotionCalculatePage(PromotionCalculateViewModel vm)
            : base(vm)
        {
            InitializeComponent();
        }

        private void NumberBoxValueChanged(ModernWpf.Controls.NumberBox sender, ModernWpf.Controls.NumberBoxValueChangedEventArgs args)
        {
            List<Button> buttons = new();
            FindChildren(sender, buttons);
            if (buttons.Count > 0)
            {
                ((Grid)buttons[0].Parent).Children.Remove(buttons[0]);
            }
        }

        private void NumberBoxGotFocus(object sender, RoutedEventArgs e)
        {
            List<Button> buttons = new();
            FindChildren((ModernWpf.Controls.NumberBox)sender, buttons);
            if (buttons.Count > 0)
            {
                ((Grid)buttons[0].Parent).Children.Remove(buttons[0]);
            }
        }

        private void FindChildren<T>(DependencyObject parent, List<T> results)
            where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(parent, i);
                if ((current.GetType()).Equals(typeof(T)))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }

                FindChildren<T>(current, results);
            }
        }

        private void PageUnloaded(object sender, RoutedEventArgs e)
        {
            ((PromotionCalculateViewModel)DataContext).CloseUICommand.Execute(null);
        }
    }
}