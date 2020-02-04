using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace HangFire_Core
{
    public class ProcessFile
    {
        private IHostingEnvironment _env { get; set; }
        public ProcessFile(IHostingEnvironment env)
        {
            _env = env;
        }
        public void ReadWriteFile()
        {
            string filePath = Path.Combine(_env.WebRootPath, "test.txt");
            string content = File.ReadAllText(filePath);
            int number = Int32.Parse(content);
            number = number + 1;
            File.WriteAllText(filePath, number.ToString());
        }
    }
}
