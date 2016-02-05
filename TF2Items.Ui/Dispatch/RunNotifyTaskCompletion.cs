using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace TF2Items.Ui.Dispatch
{
    public sealed class RunNotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        private readonly Func<Task<TResult>> _func;

        public RunNotifyTaskCompletion(Func<Task<TResult>> func)
        {
            _func = func;
        }

        private void StartTask()
        {
            if (Task != null)
                return;

            Task = _func();
            if (!Task.IsCompleted)
            {
                var _ = WatchTaskAsync(Task);
            }
        }

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }
            var propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(this, new PropertyChangedEventArgs("Status"));
            propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
            propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));
            if (task.IsCanceled)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
            }
            else if (task.IsFaulted)
            {
                propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                propertyChanged(this,
                    new PropertyChangedEventArgs("InnerException"));
                propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
            }
            else
            {
                propertyChanged(this,
                    new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("Result"));
            }
        }
        public Task<TResult> Task { get; private set; }
        public TResult Result
        {
            get
            {
                StartTask();
                return (Task.Status == TaskStatus.RanToCompletion) ?
                    Task.Result : default(TResult);
            }
        }
        public TaskStatus Status
        {
            get
            {
                StartTask(); return Task.Status;
            }
        }
        public bool IsCompleted
        {
            get
            {
                StartTask(); return Task.IsCompleted;
            }
        }
        public bool IsNotCompleted
        {
            get
            {
                StartTask(); return !Task.IsCompleted;
            }
        }
        public bool IsSuccessfullyCompleted
        {
            get
            {
                StartTask();
                return Task.Status ==
                       TaskStatus.RanToCompletion;
            }
        }
        public bool IsCanceled
        {
            get
            {
                StartTask(); return Task.IsCanceled;
            }
        }
        public bool IsFaulted
        {
            get
            {
                StartTask(); return Task.IsFaulted;
            }
        }
        public AggregateException Exception
        {
            get
            {
                StartTask(); return Task.Exception;
            }
        }
        public Exception InnerException
        {
            get
            {
                return (Exception == null) ?
                    null : Exception.InnerException;
            }
        }
        public string ErrorMessage
        {
            get
            {
                return (InnerException == null) ?
                    null : InnerException.Message;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}