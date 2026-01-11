using System.Text;

namespace R6tracker.Util.Logger
{
    public class Logger<TService> : ILogger<TService> where TService : class
    {
        private const string DefaultLogPath = @"C:\Users\pc\R6tracker\Logs";
        private string DefaultFileName = "log-{0}.log";

        public Logger()
        {
            CreateNonExistentDirectory();
            CreateFile();
        }

        public void LogCritical(LogModel model)
        {
            Write(model);
        }

        public void LogDebug<TObject>(LogModel model, TObject? @object) where TObject : class
        {
            Write(model);
            Console.WriteLine($"debug: {@object}");
        }

        public void LogError(LogModel model)
        {
            Write(model);
        }

        public void LogInfo(LogModel model)
        {
            Write(model);
        }

        public void LogWarning(LogModel model)
        {
            Write(model);
        }

        private void Write(LogModel model)
        {
            string path = Path.Combine(DefaultLogPath, DefaultFileName);
            using StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(model.ToString());
        }

        private void CreateNonExistentDirectory()
        {
            if (!Directory.Exists(DefaultLogPath))
            {
                Directory.CreateDirectory(DefaultLogPath);
            }
        }

        private void CreateFile()
        {
            string date = DateOnly
                .FromDateTime(DateTime.UtcNow)
                .ToString("dd-MM-yyyy");

            DefaultFileName = string.Format(DefaultFileName, date);

            string path = Path.Combine(DefaultLogPath, DefaultFileName);

            if (!File.Exists(path))
            {
                using (File.Create(path)) { }
            }
        }
    }
}
