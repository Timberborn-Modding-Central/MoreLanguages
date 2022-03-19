using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using LINQtoCSV;
using Timberborn.Common;
using Timberborn.Localization;
using UnityEngine;

namespace MoreLanguages;

public static class LocalizationService
{
    private static readonly string _langDirectory = "lang";

    private static readonly string _namePoolFile = "_names";

    private static readonly string _langExtension = ".txt";

    public static string LangPath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty, _langDirectory);
    
    public static bool TryGetFilePath(string localizationKey, out string localizationFilePath)
    {
        localizationFilePath = Path.Combine(LangPath, localizationKey + _langExtension);
        return File.Exists(localizationFilePath);
    }
    
    public static Dictionary<string, string> GetLocalization(string localization)
    {
        Dictionary<string, string> dictionary = new ();
        
        if(TryGetFilePath(localization, out string localizationFilePath))
            dictionary.AddRange(TryToReadRecords(localization, localizationFilePath).ToDictionary(record => record.Id, record => TextColors.ColorizeText(record.Text)));
        
        if(TryGetFilePath(localization + _namePoolFile, out string localizationNameFilePath))
            dictionary.AddRange(TryToReadRecords(localization + _namePoolFile, localizationNameFilePath).ToDictionary(record => record.Id, record => TextColors.ColorizeText(record.Text)));
        
        return dictionary;
    }

    /// <summary>
    /// Original code from timberborn with a file location instead of stream.
    /// Timberborn.Localization.LocalizationRepository.TryToReadRecords
    /// </summary>
    /// <param name="localization"></param>
    /// <param name="filePath"></param>
    /// <returns></returns>
    /// <exception cref="InvalidDataException"></exception>
    public static ImmutableArray<LocalizationRecord> TryToReadRecords(
        string localization,
        string filePath)
    {
        try
        {
            return new CsvContext().Read<LocalizationRecord>(filePath).ToImmutableArray();
        }
        catch (Exception ex)
        {
            string message = "Unable to parse file for " + localization + ".";
            if (ex is AggregatedException aggregatedException)
                message = message + " First error: " + aggregatedException.m_InnerExceptionsList[0].Message;
            if (localization == LocalizationCodes.Default)
                throw new InvalidDataException(message, ex);
            Debug.LogError(message);
            return ImmutableArray<LocalizationRecord>.Empty;
        }
    }
}