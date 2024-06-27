using Snap.Data.Primitive;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace DGP.Genshin.Control.Infrastructure.SmoothScrollViewer
{
    /// <summary>
    /// 平滑滚动视图
    /// https://github.com/HandyOrg/HandyControl/blob/master/src/Shared/HandyControl_Shared/Controls/Other/ScrollViewer.cs
    /// </summary>
    [SuppressMessage("", "SA1201")]
    [SuppressMessage("", "SA1202")]
    [SuppressMessage("", "SA1309")]
    [SuppressMessage("", "SA1413")]
    [SuppressMessage("", "SA1600")]
    public class SmoothScrollViewer : ScrollViewer
    {
        private double _totalVerticalOffset;
        private double _totalHorizontalOffset;
        private bool _isRunning;

        /// <summary>
        ///     滚动方向
        /// </summary>
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            nameof(Orientation), typeof(Orientation), typeof(SmoothScrollViewer), new PropertyMetadata(Orientation.Vertical));

        /// <summary>
        ///     滚动方向
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)GetValue(OrientationProperty);

            set => SetValue(OrientationProperty, value);
        }

        /// <summary>
        ///     是否响应鼠标滚轮操作
        /// </summary>
        public static readonly DependencyProperty CanMouseWheelProperty = DependencyProperty.Register(
            nameof(CanMouseWheel), typeof(bool), typeof(SmoothScrollViewer), new PropertyMetadata(BoxedValue.TrueBox));

        /// <summary>
        ///     是否响应鼠标滚轮操作
        /// </summary>
        public bool CanMouseWheel
        {
            get => (bool)GetValue(CanMouseWheelProperty);

            set => SetValue(CanMouseWheelProperty, BoxedValue.Boolean(value));
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            // scroll optimized
            if (ViewportHeight + VerticalOffset >= ExtentHeight && e.Delta <= 0)
            {
                return;
            }

            if (VerticalOffset == 0 && e.Delta >= 0)
            {
                return;
            }

            if (!CanMouseWheel)
            {
                return;
            }

            if (!IsInertiaEnabled)
            {
                if (Orientation == Orientation.Vertical)
                {
                    base.OnMouseWheel(e);
                }
                else
                {
                    _totalHorizontalOffset = HorizontalOffset;
                    CurrentHorizontalOffset = HorizontalOffset;
                    _totalHorizontalOffset = Math.Min(Math.Max(0, _totalHorizontalOffset - e.Delta), ScrollableWidth);
                    CurrentHorizontalOffset = _totalHorizontalOffset;
                }

                return;
            }

            e.Handled = true;

            if (Orientation == Orientation.Vertical)
            {
                if (!_isRunning)
                {
                    _totalVerticalOffset = VerticalOffset;
                    CurrentVerticalOffset = VerticalOffset;
                }

                _totalVerticalOffset = Math.Min(Math.Max(0, _totalVerticalOffset - e.Delta), ScrollableHeight);
                ScrollToVerticalOffsetWithAnimation(_totalVerticalOffset);
            }
            else
            {
                if (!_isRunning)
                {
                    _totalHorizontalOffset = HorizontalOffset;
                    CurrentHorizontalOffset = HorizontalOffset;
                }

                _totalHorizontalOffset = Math.Min(Math.Max(0, _totalHorizontalOffset - e.Delta), ScrollableWidth);
                ScrollToHorizontalOffsetWithAnimation(_totalHorizontalOffset);
            }
        }

        internal void ScrollToTopInternal(double milliseconds = 500)
        {
            if (!_isRunning)
            {
                _totalVerticalOffset = VerticalOffset;
                CurrentVerticalOffset = VerticalOffset;
            }

            ScrollToVerticalOffsetWithAnimation(0, milliseconds);
        }

        public void ScrollToVerticalOffsetWithAnimation(double offset, double milliseconds = 500)
        {
            DoubleAnimation animation = AnimationHelper.CreateAnimation(offset, milliseconds);
            animation.EasingFunction = new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            };
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += (s, e1) =>
            {
                CurrentVerticalOffset = offset;
                _isRunning = false;
            };
            _isRunning = true;

            BeginAnimation(CurrentVerticalOffsetProperty, animation, HandoffBehavior.Compose);
        }

        public void ScrollToHorizontalOffsetWithAnimation(double offset, double milliseconds = 500)
        {
            DoubleAnimation animation = AnimationHelper.CreateAnimation(offset, milliseconds);
            animation.EasingFunction = new CubicEase
            {
                EasingMode = EasingMode.EaseOut
            };
            animation.FillBehavior = FillBehavior.Stop;
            animation.Completed += (s, e1) =>
            {
                CurrentHorizontalOffset = offset;
                _isRunning = false;
            };
            _isRunning = true;

            BeginAnimation(CurrentHorizontalOffsetProperty, animation, HandoffBehavior.Compose);
        }

        protected override HitTestResult? HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return IsPenetrating ? null : base.HitTestCore(hitTestParameters);
        }

        /// <summary>
        ///     是否支持惯性
        /// </summary>
        public static readonly DependencyProperty IsInertiaEnabledProperty = DependencyProperty.RegisterAttached(
            nameof(IsInertiaEnabled), typeof(bool), typeof(SmoothScrollViewer), new PropertyMetadata(BoxedValue.TrueBox));

        public static void SetIsInertiaEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsInertiaEnabledProperty, BoxedValue.Boolean(value));
        }

        public static bool GetIsInertiaEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(IsInertiaEnabledProperty);
        }

        /// <summary>
        ///     是否支持惯性
        /// </summary>
        public bool IsInertiaEnabled
        {
            get => (bool)GetValue(IsInertiaEnabledProperty);

            set => SetValue(IsInertiaEnabledProperty, BoxedValue.Boolean(value));
        }

        /// <summary>
        ///     控件是否可以穿透点击
        /// </summary>
        public static readonly DependencyProperty IsPenetratingProperty = DependencyProperty.RegisterAttached(
            nameof(IsPenetrating), typeof(bool), typeof(SmoothScrollViewer), new PropertyMetadata(BoxedValue.FalseBox));

        /// <summary>
        ///     控件是否可以穿透点击
        /// </summary>
        public bool IsPenetrating
        {
            get => (bool)GetValue(IsPenetratingProperty);

            set => SetValue(IsPenetratingProperty, BoxedValue.Boolean(value));
        }

        public static void SetIsPenetrating(DependencyObject element, bool value)
        {
            element.SetValue(IsPenetratingProperty, BoxedValue.Boolean(value));
        }

        public static bool GetIsPenetrating(DependencyObject element)
        {
            return (bool)element.GetValue(IsPenetratingProperty);
        }

        /// <summary>
        ///     当前垂直滚动偏移
        /// </summary>
        internal static readonly DependencyProperty CurrentVerticalOffsetProperty = DependencyProperty.Register(
            nameof(CurrentVerticalOffset), typeof(double), typeof(SmoothScrollViewer), new PropertyMetadata(BoxedValue.Double0Box, OnCurrentVerticalOffsetChanged));

        private static void OnCurrentVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SmoothScrollViewer ctl && e.NewValue is double v)
            {
                ctl.ScrollToVerticalOffset(v);
            }
        }

        /// <summary>
        ///     当前垂直滚动偏移
        /// </summary>
        internal double CurrentVerticalOffset
        {
            // ReSharper disable once UnusedMember.Local
            get => (double)GetValue(CurrentVerticalOffsetProperty);

            set => SetValue(CurrentVerticalOffsetProperty, value);
        }

        /// <summary>
        ///     当前水平滚动偏移
        /// </summary>
        internal static readonly DependencyProperty CurrentHorizontalOffsetProperty = DependencyProperty.Register(
            nameof(CurrentHorizontalOffset), typeof(double), typeof(SmoothScrollViewer), new PropertyMetadata(BoxedValue.Double0Box, OnCurrentHorizontalOffsetChanged));

        private static void OnCurrentHorizontalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SmoothScrollViewer ctl && e.NewValue is double v)
            {
                ctl.ScrollToHorizontalOffset(v);
            }
        }

        /// <summary>
        ///     当前水平滚动偏移
        /// </summary>
        internal double CurrentHorizontalOffset
        {
            get => (double)GetValue(CurrentHorizontalOffsetProperty);

            set => SetValue(CurrentHorizontalOffsetProperty, value);
        }
    }
}