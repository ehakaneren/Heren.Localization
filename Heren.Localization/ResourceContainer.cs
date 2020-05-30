using System.Collections.Generic;
using System.Globalization;

namespace Heren.Localization
{
    internal class ResourceContainer
    {
        public string ResourceName { get; set; }
        public string ResourcePath { get; set; }
        public CultureInfo Culture { get; set; }
        public Dictionary<string, string> Resources { get; set; }
    }
}