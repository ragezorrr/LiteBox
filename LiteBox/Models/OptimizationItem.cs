using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System;

namespace LiteBoxOptimizer.Models
{
    public class OptimizationItem : INotifyPropertyChanged
    {
        private bool _isApplied;

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public OptimizationType Type { get; set; }
        public string Script { get; set; } = string.Empty;
        public bool RequiresAdmin { get; set; } = true;

        public bool IsApplied
        {
            get => _isApplied;
            set
            {
                if (_isApplied != value)
                {
                    _isApplied = value;
                    OnPropertyChanged();
                    // Вызываем событие для обработки изменения
                    IsAppliedChanged?.Invoke(this, value);
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<bool>? IsAppliedChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum OptimizationType
    {
        Registry,
        PowerShell,
        Service,
        FileSystem
    }
}
