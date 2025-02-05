using System.Xml;

namespace UxmlViewBinding
{
    public static class IncrementalStringBuilderExtensions
    {
        private const int PrefixLength = 7;
        
        public static IncrementalBuilder AddFields(this IncrementalBuilder builder, XmlNodeList elements) =>
            builder.AddFieldsOrConstructorLines(elements, isConstructorLine: false);
        
        public static IncrementalBuilder AddConstructorLines(this IncrementalBuilder builder, XmlNodeList elements) =>
            builder.AddFieldsOrConstructorLines(elements, isConstructorLine: true);
        
        private static IncrementalBuilder AddFieldsOrConstructorLines(
            this IncrementalBuilder builder, XmlNodeList elements, bool isConstructorLine)
        {
            foreach (XmlNode element in elements)
            {
                string name = element.Attributes?["name"].Value;
                if (name is null) continue;

                string type = element.Name.Substring(PrefixLength);

                if (isConstructorLine)
                {
                    builder = builder - "            " + builder.ToPascalCase(name) + " = root.Q<" + type + ">(\"" 
                              + name + "\") ?? throw new NullReferenceException(\"\\\"" + name + "\\\" not found!\");";
                }
                else
                {
                    builder = builder - "        [NotNull] public readonly " 
                              + type + " " + builder.ToPascalCase(name) + ";";
                }
            }
            return builder;
        }
    }
}
