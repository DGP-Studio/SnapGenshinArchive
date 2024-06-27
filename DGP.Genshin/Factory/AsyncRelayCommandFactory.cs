using CommunityToolkit.Mvvm.Input;
using DGP.Genshin.Factory.Abstraction;
using Microsoft.AppCenter.Crashes;
using Snap.Core.DependencyInjection;
using Snap.Core.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace DGP.Genshin.Factory
{
    /// <inheritdoc cref="IAsyncRelayCommandFactory"/>
    [Factory(typeof(IAsyncRelayCommandFactory), InjectAs.Transient)]
    internal class AsyncRelayCommandFactory : IAsyncRelayCommandFactory
    {
        /// <inheritdoc/>
        public AsyncRelayCommand<T> Create<T>(Func<T?, Task> execute)
        {
            return Register(new AsyncRelayCommand<T>(execute));
        }

        /// <inheritdoc/>
        public AsyncRelayCommand<T> Create<T>(Func<T?, CancellationToken, Task> cancelableExecute)
        {
            return Register(new AsyncRelayCommand<T>(cancelableExecute));
        }

        /// <inheritdoc/>
        public AsyncRelayCommand<T> Create<T>(Func<T?, Task> execute, Predicate<T?> canExecute)
        {
            return Register(new AsyncRelayCommand<T>(execute, canExecute));
        }

        /// <inheritdoc/>
        public AsyncRelayCommand<T> Create<T>(Func<T?, CancellationToken, Task> cancelableExecute, Predicate<T?> canExecute)
        {
            return Register(new AsyncRelayCommand<T>(cancelableExecute, canExecute));
        }

        /// <inheritdoc/>
        public AsyncRelayCommand Create(Func<Task> execute)
        {
            return Register(new AsyncRelayCommand(execute));
        }

        /// <inheritdoc/>
        public AsyncRelayCommand Create(Func<CancellationToken, Task> cancelableExecute)
        {
            return Register(new AsyncRelayCommand(cancelableExecute));
        }

        /// <inheritdoc/>
        public AsyncRelayCommand Create(Func<Task> execute, Func<bool> canExecute)
        {
            return Register(new AsyncRelayCommand(execute, canExecute));
        }

        /// <inheritdoc/>
        public AsyncRelayCommand Create(Func<CancellationToken, Task> cancelableExecute, Func<bool> canExecute)
        {
            return Register(new AsyncRelayCommand(cancelableExecute, canExecute));
        }

        private AsyncRelayCommand Register(AsyncRelayCommand command)
        {
            ReportException(command);
            return command;
        }

        private AsyncRelayCommand<T> Register<T>(AsyncRelayCommand<T> command)
        {
            ReportException(command);
            return command;
        }

        private void ReportException(IAsyncRelayCommand command)
        {
            command.PropertyChanged += (sender, args) =>
            {
                if (sender is IAsyncRelayCommand asyncRelayCommand)
                {
                    if (args.PropertyName == nameof(AsyncRelayCommand.ExecutionTask))
                    {
                        if (asyncRelayCommand.ExecutionTask?.Exception is AggregateException exception)
                        {
                            Crashes.TrackError(exception);
                            this.Log(exception);
                        }
                    }
                }
            };
        }
    }
}