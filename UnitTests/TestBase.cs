using System.Reflection;

namespace UnitTests
{
    [TestFixture]
    public abstract class TestBase
    {
        private List<IDisposable> disposables = new List<IDisposable>();

        [TearDown]
        public void Cleanup()
        {
            foreach (IDisposable disposable in disposables)
                disposable.Dispose();
            disposables.Clear();
        }

        /// <summary>
        /// Returns the path where the executable is running.
        /// </summary>
        private string GetAppPath()
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (path == null)
                throw new IOException("Unable to determine application path");
            return path;
        }

        /// <summary>
        /// Returns the path where the test data is located.
        /// </summary>
        protected string GetTestDataPath()
        {
            string apppath = GetAppPath();
            return Path.Combine(apppath, "..\\..\\..\\TestData");
        }

        /// <summary>
        /// Opens a FileStream for the specified test data file.
        /// </summary>
        protected FileStream StreamTestData(string filename)
        {
            string pathfilename = Path.Combine(GetTestDataPath(), filename);
            FileStream fs = File.OpenRead(pathfilename);
            disposables.Add(fs);
            return fs;
        }

        protected string ReadAllTestData(string filename)
        {
            string pathfilename = Path.Combine(GetTestDataPath(), filename);
            return File.ReadAllText(pathfilename);
        }
    }
}
