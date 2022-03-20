using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Timberborn.Localization;

namespace MoreLanguages;

[HarmonyPatch]
[BepInPlugin("com.timbercentral.morelanguages", "MoreLanguages", "1.1.0")]
public class MoreLanguagesPlugin : BaseUnityPlugin
{
    private static ConfigEntry<bool> _missingKeyLogging;
    
    private void Awake()
    {
        _missingKeyLogging = Config.Bind("Logging", "MissingKeyLogging", false, "Log message when Loc doesn't have the given key in directory (devs only)");
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
    }
    
    /// <summary>
    /// Adds the new languages to the language selection box
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Timberborn.Localization.LocalizationService), "AvailableLanguages", MethodType.Getter)]
    private static IEnumerable<(string, string)> AddNewTranslations(IEnumerable<(string, string)> values)
    {
        foreach ((string, string) valueTuple in values) 
            yield return valueTuple;

        if (!Directory.Exists(LocalizationService.LangPath)) 
            yield break;
        
        foreach (string filePath in Directory.GetFiles(LocalizationService.LangPath).Where(path => !path.Contains("_names")))
            yield return (LocalizationService.TryToReadRecords(Path.GetFileNameWithoutExtension(filePath), filePath).SingleOrDefault(record => record.Id.Equals("Settings.Language.Name"))?.Text ?? Path.GetFileNameWithoutExtension(filePath), Path.GetFileNameWithoutExtension(filePath));
    }
    
    /// <summary>
    /// Ignores the original code when it's a custom language preventing errors
    /// </summary>
    /// <param name="localizationKey"></param>
    /// <param name="__result"></param>
    /// <returns></returns>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(LocalizationRepository), "GetLocalization", typeof(string))]
    private static bool IgnoreOriginalWithNewLanguages(string localizationKey, ref Dictionary<string, string> __result)
    {
        if (GetFieldValuesFromStatic(typeof(LocalizationCodes)).ContainsValue(localizationKey))
            return true;
        
        __result = LocalizationService.GetLocalization(localizationKey);
        return false;
    }

    /// <summary>
    /// Ignores the original code when it's a custom language preventing errors
    /// </summary>
    /// <param name="languageName"></param>
    /// <param name="__instance"></param>
    /// <param name="____localizationRepository"></param>
    /// <param name="____loc"></param>
    /// <returns></returns>
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Timberborn.Localization.LocalizationService), "Load", typeof(string))]
    [SuppressMessage("Member Access", "Publicizer001:Accessing a member that was not originally public")]
    private static bool IgnoreOriginalWithNewLanguagesLoad(string languageName, Timberborn.Localization.LocalizationService __instance, LocalizationRepository ____localizationRepository, ILoc ____loc)
    {
        if (GetFieldValuesFromStatic(typeof(LocalizationCodes)).ContainsValue(languageName))
            return true;
        
        __instance.CurrentLanguage = languageName;
        ____loc.Initialize(____localizationRepository.GetLocalization(languageName));
        return false;
    }
    
    /// <summary>
    /// Patch to ignore the debugging warning when localization is missing. Keeps console cleaner with experimental items.
    /// </summary>
    [HarmonyPatch(typeof(Loc), "T", typeof(string))]
    public static class PatchLocKeyLogging
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (_missingKeyLogging.Value)
                return instructions;

            var foundMassUsageMethod = false;
            int startIndex = -1;
            int endIndex = -1;

            List<CodeInstruction> codes = new (instructions);
            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode != OpCodes.Ret)
                    continue;
                
                if (foundMassUsageMethod)
                {
                    endIndex = i; // include current 'ret'
                    break;
                }
                
                startIndex = i + 1; // exclude current 'ret'

                for (int j = startIndex; j < codes.Count; j++)
                {
                    if (codes[j].opcode == OpCodes.Ret)
                        break;
                    
                    var strOperand = codes[j].operand as string;
                    
                    if (strOperand != "The given key ") 
                        continue;
                    
                    foundMassUsageMethod = true;
                    break;
                }
            }

            if (startIndex > -1 && endIndex > -1) 
                codes.RemoveRange(startIndex + 1, endIndex - startIndex - 1);
            return codes.AsEnumerable();
        }
    }

    /// <summary>
    /// Get all properties key, values from given static class
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static Dictionary<string, string> GetFieldValuesFromStatic(IReflect type)
    {
        return type
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(f => f.FieldType == typeof(string))
            .ToDictionary(f => f.Name,
                f => (string) f.GetValue(null));
    }
}