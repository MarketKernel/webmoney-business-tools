using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace LocalizationAssistant
{
    public sealed class Translator
    {
        private const string StorageDirectory = "Localization";

        private static readonly object Anchor = new object();

        private readonly string _languageDirectory;
        private readonly SortedDictionary<string, ValueDictionaryHolder> _categoryDictionary;

        private static volatile Translator _instance;

        public static Translator Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (Anchor)
                    {
                        if (null == _instance)
                            _instance = new Translator();
                    }
                }

                return _instance;
            }
        }

        public Translator(string storageDirectory = null)
        {

            if (null == storageDirectory)
                storageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, StorageDirectory);

            if (!Directory.Exists(storageDirectory))
                Directory.CreateDirectory(storageDirectory);

            var twoLetterIsoLanguageName = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

            _languageDirectory = Path.Combine(storageDirectory, twoLetterIsoLanguageName);

            if (!Directory.Exists(_languageDirectory))
                Directory.CreateDirectory(_languageDirectory);

            _categoryDictionary = new SortedDictionary<string, ValueDictionaryHolder>();
            var files = Directory.GetFiles(_languageDirectory, "*.json", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                var category = Path.GetFileNameWithoutExtension(file);

                category = category?.ToLower(CultureInfo.InvariantCulture) ??
                           throw new InvalidOperationException("null == category");

                var valueDictionary = ValueDictionaryHolder.LoadFormFile(file);
                _categoryDictionary.Add(category, valueDictionary);
            }
        }

        public void Apply()
        {
            lock (Anchor)
            {
                _instance = this;
            }
        }

        public string Translate(string category, string value)
        {
            if (null == category)
                return null;

            if (null == value)
                return null;

            category = category.ToLower();

            string translation;

            lock (Anchor)
            {
                ValueDictionaryHolder valueDictionaryHolder;

                if (!_categoryDictionary.TryGetValue(category, out valueDictionaryHolder))
                {
                    valueDictionaryHolder = new ValueDictionaryHolder();
                    _categoryDictionary.Add(category, valueDictionaryHolder);
                }

                var valueDictionary = valueDictionaryHolder.Dictionary;

                if (!valueDictionary.ContainsKey(value))
                {
                    valueDictionary.Add(value, string.Empty);
                    valueDictionaryHolder.Changed = true;
                }

                translation = valueDictionary[value];
            }

            if (string.IsNullOrEmpty(translation))
                return value;

            return translation;
        }

        public void Save()
        {
            lock (Anchor)
            {
                foreach (var keyValue in _categoryDictionary)
                {
                    if (!keyValue.Value.Changed)
                        continue;

                    var filePath = Path.Combine(_languageDirectory, keyValue.Key);
                    filePath = Path.ChangeExtension(filePath, ".json");

                    keyValue.Value.SaveToFile(filePath);
                }
            }
        }
    }
}