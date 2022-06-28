using System.ComponentModel;

namespace NoCopyAutoPreview.Config
{
    public interface IPluginConfig: INotifyPropertyChanged
    {
        bool Enabled { get; set; }
    }
}