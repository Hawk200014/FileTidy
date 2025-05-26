using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTidyDatabase;

public class SettingsController
{
    private readonly FileTidyDbContext _context;
    public SettingsController(FileTidyDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<String> GetSettingAsync(string key)
    {
        var setting = await _context.BasicSettings
            .Where(s => s.Key.Equals(key))
            .FirstOrDefaultAsync();
        return setting?.Value ?? string.Empty;
    }

    public async Task SetSettingAsync(string key, string value)
    {
        var setting = await _context.BasicSettings
            .Where(s => s.Key.Equals(key))
            .FirstOrDefaultAsync();
        if (setting == null)
        {
            setting = new Models.BasicSetting { Key = key, Value = value };
            _context.BasicSettings.Add(setting);
        }
        else
        {
            setting.Value = value;
            _context.BasicSettings.Update(setting);
        }
        await _context.SaveChangesAsync();
    }
}