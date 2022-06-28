using System;
using System.IO;
using NoCopyAutoPreview.GUI;
using HarmonyLib;
using Sandbox.Graphics.GUI;
using NoCopyAutoPreview.Config;
using NoCopyAutoPreview.Logging;
using NoCopyAutoPreview.Patches;
using VRage.FileSystem;
using VRage.Plugins;

namespace NoCopyAutoPreview
{
    // ReSharper disable once UnusedType.Global
    public class Plugin : IPlugin, IDisposable
    {
        public const string Name = "NoCopyAutoPreview";
        public static Plugin Instance { get; private set; }

        public IPluginLogger Log => Logger;
        private static readonly IPluginLogger Logger = new PluginLogger(Name);

        public IPluginConfig Config => config?.Data;
        private PersistentConfig<PluginConfig> config;
        private static readonly string ConfigFileName = $"{Name}.cfg";

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public void Init(object gameInstance)
        {
            Instance = this;

            Log.Info("Loading");

            var configPath = Path.Combine(MyFileSystem.UserDataPath, ConfigFileName);
            config = PersistentConfig<PluginConfig>.Load(Log, configPath);

            if (!PatchHelpers.HarmonyPatchAll(Log, new Harmony(Name)))
            {
                return;
            }

            Log.Debug("Successfully loaded");
        }

        public void Dispose()
        {
            Instance = null;
        }

        public void Update()
        {
            
        }

        // ReSharper disable once UnusedMember.Global
        public void OpenConfigDialog()
        {
            MyGuiSandbox.AddScreen(new MyPluginConfigDialog());
        }
    }
}