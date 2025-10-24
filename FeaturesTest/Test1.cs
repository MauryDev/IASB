namespace FeaturesTest
{
    [TestClass]
    public sealed class Test1
    {
        public static bool IsPathInsideBaseDirectory(string basePath,string inputPath)
        {
            string canonicalInput = Path.GetFullPath(basePath + inputPath);

            return canonicalInput.StartsWith(
                basePath,
                StringComparison.OrdinalIgnoreCase
            ) && canonicalInput.Length != basePath.Length;
        }
        [TestMethod]
        public void GetParent()
        {
            var t = Path.GetDirectoryName("ad");
            Assert.AreEqual("ad",t);
        }
        [TestMethod]
        public void TestMethod1()
        {
            Assert.IsTrue(IsPathInsideBaseDirectory(Path.GetFullPath("Music") + "\\", "a"));
        }
    }
}
