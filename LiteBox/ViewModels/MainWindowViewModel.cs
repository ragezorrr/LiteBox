using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LiteBoxOptimizer.Commands;
using LiteBoxOptimizer.Models;
using LiteBoxOptimizer.Services;

namespace LiteBoxOptimizer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly OptimizationService _optimizationService;
    private OptimizationCategory? _selectedCategory;
    private OptimizationItem? _selectedItem;

    public MainWindowViewModel()
    {
        _optimizationService = new OptimizationService();
        Categories = new ObservableCollection<OptimizationCategory>(_optimizationService.GetOptimizationCategories());
        
        // Подписываемся на события изменения IsApplied для всех элементов
        foreach (var category in Categories)
        {
            foreach (var item in category.Items)
            {
                item.IsAppliedChanged += OnOptimizationToggled;
            }
        }
        
        // Создаем команды
        ApplyOptimizationCommand = new AsyncRelayCommand<OptimizationItem>(ApplyOptimization);
        SelectCategoryCommand = new RelayCommand<OptimizationCategory>(SelectCategory);
        SelectItemCommand = new RelayCommand<OptimizationItem>(SelectItem);
        
        // Выбираем первую категорию по умолчанию
        if (Categories.Count > 0)
        {
            SelectedCategory = Categories[0];
        }
    }

    public ObservableCollection<OptimizationCategory> Categories { get; }

    public ICommand ApplyOptimizationCommand { get; }
    public ICommand SelectCategoryCommand { get; }
    public ICommand SelectItemCommand { get; }

    public OptimizationCategory? SelectedCategory
    {
        get => _selectedCategory;
        set => SetField(ref _selectedCategory, value);
    }

    public OptimizationItem? SelectedItem
    {
        get => _selectedItem;
        set => SetField(ref _selectedItem, value);
    }

    private async void OnOptimizationToggled(object? sender, bool isApplied)
    {
        if (sender is OptimizationItem item)
        {
            try
            {
                if (isApplied)
                {
                    // Применяем оптимизацию
                    var result = await _optimizationService.ApplyOptimizationAsync(item);
                    if (!result)
                    {
                        // Если применение не удалось, возвращаем состояние обратно
                        item.IsApplied = false;
                    }
                }
                // Если isApplied == false, то просто отключаем оптимизацию
            }
            catch (Exception)
            {
                // В случае ошибки возвращаем переключатель в исходное состояние
                item.IsApplied = false;
            }
        }
    }

    private void SelectCategory(OptimizationCategory category)
    {
        SelectedCategory = category;
        SelectedItem = null;
    }

    private void SelectItem(OptimizationItem item)
    {
        SelectedItem = item;
    }

    private async Task ToggleOptimization(OptimizationItem item)
    {
        if (item == null) return;

        try
        {
            // Если оптимизация уже применена, просто переключаем состояние
            if (item.IsApplied)
            {
                item.IsApplied = false;
                return;
            }

            // Применяем оптимизацию
            var result = await _optimizationService.ApplyOptimizationAsync(item);
            
            // Обновляем состояние
            item.IsApplied = result;
        }
        catch (Exception)
        {
            // В случае ошибки возвращаем переключатель в исходное состояние
            item.IsApplied = false;
        }
    }

    private async Task ApplyOptimization(OptimizationItem item)
    {
        if (item == null) return;

        try
        {
            // Если оптимизация уже применена, просто переключаем состояние
            if (item.IsApplied)
            {
                item.IsApplied = false;
                return;
            }

            // Применяем оптимизацию
            var result = await _optimizationService.ApplyOptimizationAsync(item);
            
            // Обновляем состояние
            item.IsApplied = result;
        }
        catch (Exception)
        {
            // В случае ошибки возвращаем переключатель в исходное состояние
            item.IsApplied = false;
        }
    }
}
