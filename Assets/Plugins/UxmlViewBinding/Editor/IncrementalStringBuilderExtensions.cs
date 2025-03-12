using System.Xml;

namespace UxmlViewBinding.Editor
{
    public static class IncrementalStringBuilderExtensions
    {
        private const string EngineTypePrefix = "engine:";
        
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
                if (name is null || name.StartsWith('_')) continue;

                string type = element.Name;
                if (type.StartsWith(EngineTypePrefix))
                {
                    type = type.Substring(EngineTypePrefix.Length);
                }
                
                switch (type)
                {
                    case "Template": continue;
                    case "Instance":
                    {
                        string template = element.Attributes?["template"].Value;
                        type = string.Concat(template, "ViewBinding");
                        builder = isConstructorLine 
                            ? builder.AddInstanceConstructorLine(name, type) 
                            : builder.AddField(name, type);
                        continue;
                    }
                }
                
                builder = isConstructorLine ? builder.AddConstructorLine(name, type) : builder.AddField(name, type);
            }
            return builder;
        }

        private static IncrementalBuilder AddConstructorLine(
            this IncrementalBuilder builder, string name, string type) =>
                builder - "            " + builder.ToPascalCase(name) + " = root.Q<" + type + ">(\"" 
                        + name + "\") ?? throw new NullReferenceException(\"\\\"" + name + "\\\" not found!\");";
        
        private static IncrementalBuilder AddInstanceConstructorLine(
            this IncrementalBuilder builder, string name, string type) =>
                builder - "            " + builder.ToPascalCase(name) + " = new " + type + "(root.Q<VisualElement>(\"" 
                        + name + "\") ?? throw new NullReferenceException(\"\\\"" + name + "\\\" not found!\"));";
        
        private static IncrementalBuilder AddField(
            this IncrementalBuilder builder, string name, string type) =>
                builder - "        [NotNull] public readonly " + type + " " + builder.ToPascalCase(name) + ";";
    }
}
