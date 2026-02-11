using System;

using JMD_Arbeitszeitmanager.Models;

namespace JMD_Arbeitszeitmanager.Contracts.Services
{
    public interface IThemeSelectorService
    {
        bool SetTheme(AppTheme? theme = null);

        AppTheme GetCurrentTheme();
    }
}
