using System;

namespace XmlConfig
{
    public interface ISettings
    {
        Version SettingsVersion { get; set; }
        void Load();
        void Save();
    }


}
