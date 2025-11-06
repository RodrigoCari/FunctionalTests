using OpenQA.Selenium;
using System.Reflection;

namespace FunctionalTests
{
    public static class ScreenshotHelper
    {
        private static int _screenshotCount = 0;
        private static readonly string _projectRoot = Directory.GetCurrentDirectory();
        private static readonly string _screenshotsFolder = Path.Combine(_projectRoot, "TestResults", "Screenshots");

        public static void CaptureScreenshot(IWebDriver driver, string testName)
        {
            try
            {
                // Crear directorio si no existe
                if (!Directory.Exists(_screenshotsFolder))
                {
                    Directory.CreateDirectory(_screenshotsFolder);
                    Console.WriteLine($"📁 Carpeta creada: {_screenshotsFolder}");
                }

                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                _screenshotCount++;

                // Limpiar nombre del test para el archivo
                var cleanTestName = testName.Replace(" ", "_").Replace("/", "-");
                var fileName = $"{_screenshotCount:00}_{cleanTestName}_{DateTime.Now:HHmmss}.png";
                var filePath = Path.Combine(_screenshotsFolder, fileName);

                // CORRECCIÓN: Usar SaveAsFile sin el formato
                screenshot.SaveAsFile(filePath);

                var fullPath = Path.GetFullPath(filePath);
                Console.WriteLine($"📸 Screenshot guardado: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al capturar screenshot: {ex.Message}");
            }
        }

        public static void OpenScreenshotsFolder()
        {
            try
            {
                if (Directory.Exists(_screenshotsFolder))
                {
                    System.Diagnostics.Process.Start("explorer.exe", _screenshotsFolder);
                    Console.WriteLine($"📂 Abriendo carpeta de screenshots: {_screenshotsFolder}");
                }
                else
                {
                    Console.WriteLine("❌ Carpeta de screenshots no encontrada");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al abrir carpeta: {ex.Message}");
            }
        }

        public static string GetScreenshotsSummary()
        {
            if (!Directory.Exists(_screenshotsFolder))
                return "No se capturaron screenshots";

            var screenshots = Directory.GetFiles(_screenshotsFolder, "*.png")
                .OrderBy(f => f)
                .ToArray();

            var summary = $"📸 RESUMEN DE SCREENSHOTS ({screenshots.Length} capturas):\n";
            summary += $"📍 Ubicación: {Path.GetFullPath(_screenshotsFolder)}\n\n";

            foreach (var screenshot in screenshots)
            {
                var fileName = Path.GetFileName(screenshot);
                var fileInfo = new FileInfo(screenshot);
                summary += $"• {fileName} ({fileInfo.Length / 1024} KB)\n";
            }

            return summary;
        }
    }
}