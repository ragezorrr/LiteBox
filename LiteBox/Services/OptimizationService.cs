using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Win32;
using LiteBoxOptimizer.Models;

namespace LiteBoxOptimizer.Services
{
    public class OptimizationService
    {
        public async Task<bool> ApplyOptimizationAsync(OptimizationItem item)
        {
            try
            {
                switch (item.Type)
                {
                    case OptimizationType.Registry:
                        return await ApplyRegistryTweakAsync(item.Script);
                    
                    case OptimizationType.PowerShell:
                        return await ExecutePowerShellScriptAsync(item.Script);
                    
                    default:
                        return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error applying optimization: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ApplyRegistryTweakAsync(string script)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var lines = script.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("reg add"))
                        {
                            ExecuteRegistryCommand(line);
                        }
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        private async Task<bool> ExecutePowerShellScriptAsync(string script)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-Command \"{script}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        Verb = "runas"
                    };

                    using var process = Process.Start(startInfo);
                    process?.WaitForExit();
                    return process?.ExitCode == 0;
                }
                catch
                {
                    return false;
                }
            });
        }

        private void ExecuteRegistryCommand(string command)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {command}",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    Verb = "runas"
                };

                using var process = Process.Start(startInfo);
                process?.WaitForExit();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Registry command failed: {ex.Message}");
            }
        }

        public List<OptimizationCategory> GetOptimizationCategories()
        {
            return new List<OptimizationCategory>
            {
                new OptimizationCategory
                {
                    Name = "Tweaks",
                    Description = "Системные настройки и модификации интерфейса",
                    Icon = "⚙️",
                    Items = new ObservableCollection<OptimizationItem>
                    {
                        new OptimizationItem
                        {
                            Name = "Убрать вкладку Галерея из проводника",
                            Description = "Удаляет вкладку 'Галерея' из проводника Windows 11",
                            Type = OptimizationType.Registry,
                            Script = @"reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Desktop\NameSpace_41040327\{e88865ea-0e1c-4e20-9aa6-edcd0212c87c}"" /v ""System.IsPinnedToNameSpaceTree"" /t REG_DWORD /d 0 /f",
                            RequiresAdmin = true
                        },
                        new OptimizationItem
                        {
                            Name = "Показать расширения файлов",
                            Description = "Включает отображение расширений файлов в проводнике",
                            Type = OptimizationType.Registry,
                            Script = @"reg add ""HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"" /v ""HideFileExt"" /t REG_DWORD /d 0 /f",
                            RequiresAdmin = false
                        },
                        new OptimizationItem
                        {
                            Name = "Отключить звуки Windows",
                            Description = "Отключает системные звуки Windows для более тихой работы",
                            Type = OptimizationType.Registry,
                            Script = @"reg add ""HKEY_CURRENT_USER\AppEvents\Schemes"" /ve /d "".None"" /f",
                            RequiresAdmin = false
                        },
                        new OptimizationItem
                        {
                            Name = "Отключить анимации Windows",
                            Description = "Отключает анимации интерфейса для повышения скорости работы",
                            Type = OptimizationType.Registry,
                            Script = @"reg add ""HKEY_CURRENT_USER\Control Panel\Desktop"" /v ""UserPreferencesMask"" /t REG_BINARY /d ""9012038010000000"" /f",
                            RequiresAdmin = false
                        }
                    }
                },
                new OptimizationCategory
                {
                    Name = "Производительность",
                    Description = "Оптимизация производительности системы",
                    Icon = "🚀",
                    Items = new ObservableCollection<OptimizationItem>
                    {
                        new OptimizationItem
                        {
                            Name = "Очистить временные файлы",
                            Description = "Удаляет временные файлы Windows для освобождения места",
                            Type = OptimizationType.PowerShell,
                            Script = @"cleanmgr /sagerun:1",
                            RequiresAdmin = false
                        },
                        new OptimizationItem
                        {
                            Name = "Отключить гибернацию",
                            Description = "Отключает гибернацию и удаляет файл hiberfil.sys",
                            Type = OptimizationType.PowerShell,
                            Script = @"powercfg -h off",
                            RequiresAdmin = true
                        },
                        new OptimizationItem
                        {
                            Name = "Очистить кеш DNS",
                            Description = "Очищает кеш DNS для улучшения сетевого соединения",
                            Type = OptimizationType.PowerShell,
                            Script = @"ipconfig /flushdns",
                            RequiresAdmin = true
                        }
                    }
                },
                new OptimizationCategory
                {
                    Name = "Приватность",
                    Description = "Настройки конфиденциальности и телеметрии",
                    Icon = "🔒",
                    Items = new ObservableCollection<OptimizationItem>
                    {
                        new OptimizationItem
                        {
                            Name = "Отключить телеметрию Windows",
                            Description = "Полностью отключает сбор телеметрических данных Microsoft",
                            Type = OptimizationType.Registry,
                            Script = @"reg add ""HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection"" /v ""AllowTelemetry"" /t REG_DWORD /d 0 /f",
                            RequiresAdmin = true
                        },
                        new OptimizationItem
                        {
                            Name = "Отключить рекламу в Windows",
                            Description = "Отключает рекламные предложения в меню Пуск и настройках",
                            Type = OptimizationType.Registry,
                            Script = @"reg add ""HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"" /v ""SystemPaneSuggestionsEnabled"" /t REG_DWORD /d 0 /f",
                            RequiresAdmin = false
                        }
                    }
                },
                new OptimizationCategory
                {
                    Name = "Интерфейс",
                    Description = "Настройки внешнего вида и поведения интерфейса",
                    Icon = "🎨",
                    Items = new ObservableCollection<OptimizationItem>
                    {
                        new OptimizationItem
                        {
                            Name = "Включить темную тему",
                            Description = "Включает темную тему для Windows и приложений",
                            Type = OptimizationType.Registry,
                            Script = @"reg add ""HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize"" /v ""AppsUseLightTheme"" /t REG_DWORD /d 0 /f",
                            RequiresAdmin = false
                        },
                        new OptimizationItem
                        {
                            Name = "Отключить прозрачность",
                            Description = "Отключает эффекты прозрачности для повышения производительности",
                            Type = OptimizationType.Registry,
                            Script = @"reg add ""HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize"" /v ""EnableTransparency"" /t REG_DWORD /d 0 /f",
                            RequiresAdmin = false
                        }
                    }
                }
            };
        }
    }
}
