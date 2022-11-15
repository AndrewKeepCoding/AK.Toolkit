namespace AK.Toolkit.Uwp.Localization
{
    public class LocalizerResourcesStringsFolder
    {
        public LocalizerResourcesStringsFolder(string stringsFolderPath, string resourcesFileName = "Resources.resw")
        {
            StringsFolderPath = stringsFolderPath;
            ResourcesFileName = resourcesFileName;
        }

        public string StringsFolderPath { get; }

        public string ResourcesFileName { get; }
    }
}