using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;

namespace UxmlViewBinding.Editor
{
    public class UxmlBindingGenerator : AssetPostprocessor
    {
        private const string GeneratedPath = "Assets/Generated/UxmlViewBindings/";

        private static readonly IncrementalBuilder Builder = new(new StringBuilder());
        private static readonly XmlDocument XmlDoc = new();

        static UxmlBindingGenerator()
        {
            if (Directory.Exists(GeneratedPath)) return;
            Directory.CreateDirectory(GeneratedPath);
        }

        private static void OnPostprocessAllAssets(
            string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                if (!assetPath.StartsWith("Assets/")) continue;
                if (Path.GetExtension(assetPath) != ".uxml") continue;
                GenerateUxmlBinding(assetPath);
            }
        }

        private static void GenerateUxmlBinding(string uxmlPath)
        {
            string className = Builder.Clear() + Path.GetFileNameWithoutExtension(uxmlPath) + "ViewBinding";
            string outputPath = Path.Combine(GeneratedPath, Builder + ".cs");
            
            XmlDoc.Load(uxmlPath);

            XmlNodeList elements = XmlDoc.SelectNodes("//*[@name]");
            if (elements == null) return;
            
            string fileContent = CreateFileString(className, elements, Builder.Clear());
            File.WriteAllText(outputPath, fileContent);
            AssetDatabase.Refresh();
        }

        //language=C#
        private static string CreateFileString(string className, XmlNodeList elements, IncrementalBuilder builder) =>
            builder
                + "using System.Diagnostics.CodeAnalysis;"
                - "using UnityEngine.UIElements;"
                - "using System;"
                - "// ReSharper disable CheckNamespace"
                - "// ReSharper disable UnusedType.Global"
                - "// ReSharper disable NotAccessedField.Global"
                - "// ReSharper disable RedundantNullableFlowAttribute"
                - ""
                - "namespace UxmlViewBindings"
                - "{"
                - "    public readonly struct " + className
                - "    {"
                - "        [NotNull] public readonly VisualElement Root;"
                +          builder.AddFields(elements)
                - ""
                - "        public " + className + "(VisualElement root)"
                - "        {"
                - "            Root = root;"
                +              builder.AddConstructorLines(elements)
                - "        }"
                - "    }"
                - "}"
                - "";
    }
}
