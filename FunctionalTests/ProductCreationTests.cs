using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace FunctionalTests
{
    public class ProductCreationTests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly string _baseUrl = "http://localhost:8080";

        public ProductCreationTests()
        {
            Console.WriteLine("=========================================");
            Console.WriteLine("🚀 INICIALIZANDO PRUEBAS FUNCIONALES");
            Console.WriteLine("=========================================");
            Console.WriteLine($"📁 Directorio actual: {Directory.GetCurrentDirectory()}");

            new DriverManager().SetUpDriver(new ChromeConfig());

            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-notifications");
            options.AddArgument("--window-size=1280,720");

            _driver = new ChromeDriver(options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _driver.Manage().Window.Maximize();
        }

        [Fact]
        public void Test_01_CreateProduct_WithValidData()
        {
            LogTestStart("01 - Crear Producto con Datos Válidos");

            NavigateToCreateProduct();
            ScreenshotHelper.CaptureScreenshot(_driver, "01_Navegacion_Formulario");

            FillProductForm("TEST123", "Producto de prueba válido", "25.99", "https://example.com/image.jpg");
            ScreenshotHelper.CaptureScreenshot(_driver, "02_Formulario_Lleno");

            SubmitForm();
            ScreenshotHelper.CaptureScreenshot(_driver, "03_Resultado_Exitoso");

            Assert.Contains("/product/", _driver.Url);
            LogTestSuccess("✅ Producto creado exitosamente");
        }

        [Fact]
        public void Test_02_CreateProduct_WithEmptyProductId()
        {
            LogTestStart("02 - Crear Producto con ProductId Vacío");

            NavigateToCreateProduct();
            FillProductForm("", "Producto sin ID", "15.50", "https://test.com/img.png");
            ScreenshotHelper.CaptureScreenshot(_driver, "04_ProductId_Vacio");

            SubmitForm();
            ScreenshotHelper.CaptureScreenshot(_driver, "05_Resultado_Sin_ProductId");

            Assert.Contains("/product/", _driver.Url);
            LogTestSuccess("✅ Producto creado sin ProductId - Campo opcional");
        }

        [Fact]
        public void Test_03_CreateProduct_WithNegativePrice()
        {
            LogTestStart("03 - Crear Producto con Precio Negativo");

            NavigateToCreateProduct();
            FillProductForm("NEG001", "Producto con precio negativo", "-10.00", "https://test.com/img.jpg");
            ScreenshotHelper.CaptureScreenshot(_driver, "06_Precio_Negativo");

            SubmitForm();
            ScreenshotHelper.CaptureScreenshot(_driver, "07_Resultado_Precio_Negativo");

            Assert.Contains("/product/", _driver.Url);
            LogTestSuccess("✅ Producto con precio negativo creado");
        }

        [Fact]
        public void Test_04_CreateProduct_WithInvalidPriceFormat()
        {
            LogTestStart("04 - Crear Producto con Precio Inválido");

            NavigateToCreateProduct();
            FillProductForm("INV001", "Producto con precio inválido", "abc", "https://test.com/img.jpg");
            ScreenshotHelper.CaptureScreenshot(_driver, "08_Precio_Invalido");

            SubmitForm();
            ScreenshotHelper.CaptureScreenshot(_driver, "09_Manejo_Error_Precio");

            Assert.Equal($"{_baseUrl}/product", _driver.Url);
            LogTestSuccess("✅ Manejo de error correcto - Redirección a lista de productos");
        }

        [Fact]
        public void Test_05_CreateProduct_WithMinimumFields()
        {
            LogTestStart("05 - Crear Producto con Campos Mínimos");

            NavigateToCreateProduct();
            FillProductForm("MIN001", "Producto mínimo", "1.00", "");
            ScreenshotHelper.CaptureScreenshot(_driver, "10_Campos_Minimos");

            SubmitForm();
            ScreenshotHelper.CaptureScreenshot(_driver, "11_Resultado_Campos_Minimos");

            Assert.Contains("/product/", _driver.Url);
            LogTestSuccess("✅ Producto creado con campos mínimos");
        }

        [Fact]
        public void Test_06_CreateProduct_WithLongDescription()
        {
            LogTestStart("06 - Crear Producto con Descripción Larga");

            NavigateToCreateProduct();
            var longDescription = "Producto con descripción extensa para probar el comportamiento del sistema con textos largos que podrían afectar la interfaz de usuario";
            FillProductForm("LONG001", longDescription, "99.99", "https://test.com/long.jpg");
            ScreenshotHelper.CaptureScreenshot(_driver, "12_Descripcion_Larga");

            SubmitForm();
            ScreenshotHelper.CaptureScreenshot(_driver, "13_Resultado_Descripcion_Larga");

            Assert.Contains("/product/", _driver.Url);
            LogTestSuccess("✅ Producto con descripción larga creado");
        }

        [Fact]
        public void Test_07_CreateProduct_WithZeroPrice()
        {
            LogTestStart("07 - Crear Producto con Precio Cero");

            NavigateToCreateProduct();
            FillProductForm("ZERO001", "Producto con precio cero", "0.00", "https://test.com/zero.jpg");
            ScreenshotHelper.CaptureScreenshot(_driver, "14_Precio_Cero");

            SubmitForm();
            ScreenshotHelper.CaptureScreenshot(_driver, "15_Resultado_Precio_Cero");

            Assert.Contains("/product/", _driver.Url);
            LogTestSuccess("✅ Producto con precio cero creado");
        }

        [Fact]
        public void Test_08_CreateProduct_WithLargePrice()
        {
            LogTestStart("08 - Crear Producto con Precio Grande");

            NavigateToCreateProduct();
            FillProductForm("LARGE001", "Producto con precio grande", "999999.99", "https://test.com/large.jpg");
            ScreenshotHelper.CaptureScreenshot(_driver, "16_Precio_Grande");

            SubmitForm();
            ScreenshotHelper.CaptureScreenshot(_driver, "17_Resultado_Precio_Grande");

            Assert.Contains("/product/", _driver.Url);
            LogTestSuccess("✅ Producto con precio grande creado");
        }

        [Fact]
        public void Test_09_CreateProduct_WithSpecialCharacters()
        {
            LogTestStart("09 - Crear Producto con Caracteres Especiales");

            NavigateToCreateProduct();
            FillProductForm("SPEC@123", "Producto con @caracteres#especiales$", "50.00", "https://test.com/special.jpg");
            ScreenshotHelper.CaptureScreenshot(_driver, "18_Caracteres_Especiales");

            SubmitForm();
            ScreenshotHelper.CaptureScreenshot(_driver, "19_Resultado_Caracteres_Especiales");

            Assert.Contains("/product/", _driver.Url);
            LogTestSuccess("✅ Producto con caracteres especiales creado");
        }

        private void NavigateToCreateProduct()
        {
            Console.WriteLine("   🌐 Navegando a la aplicación...");
            _driver.Navigate().GoToUrl(_baseUrl);
            Thread.Sleep(1500);

            Console.WriteLine("   📝 Haciendo clic en 'Create Product'...");
            var createProductLink = _driver.FindElement(By.XPath("//a[contains(text(), 'Create Product')]"));
            createProductLink.Click();
            Thread.Sleep(1500);
        }

        private void FillProductForm(string productId, string description, string price, string imageUrl)
        {
            Console.WriteLine($"   📋 Llenando formulario - ProductId: {productId}, Precio: {price}");

            var productIdField = _driver.FindElement(By.Name("productId"));
            var descriptionField = _driver.FindElement(By.Name("description"));
            var priceField = _driver.FindElement(By.Name("price"));
            var imageUrlField = _driver.FindElement(By.Name("imageUrl"));

            productIdField.Clear();
            productIdField.SendKeys(productId);

            descriptionField.Clear();
            descriptionField.SendKeys(description);

            priceField.Clear();
            priceField.SendKeys(price);

            imageUrlField.Clear();
            imageUrlField.SendKeys(imageUrl);
        }

        private void SubmitForm()
        {
            Console.WriteLine("   🚀 Enviando formulario...");
            var submitButton = _driver.FindElement(By.CssSelector("button[type='submit']"));
            submitButton.Click();
            Thread.Sleep(2000);
        }

        private void LogTestStart(string testName)
        {
            Console.WriteLine($"\n🎬 {testName}");
            Console.WriteLine($"   ⏰ {DateTime.Now:HH:mm:ss}");
        }

        private void LogTestSuccess(string message)
        {
            Console.WriteLine($"   {message}");
            Console.WriteLine($"   📍 URL Final: {_driver.Url}");
        }

        public void Dispose()
        {
            Console.WriteLine($"\n=========================================");
            Console.WriteLine("🏁 TODAS LAS PRUEBAS COMPLETADAS");
            Console.WriteLine("=========================================");
            Console.WriteLine($"⏰ Hora de finalización: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            // Mostrar resumen de screenshots
            Console.WriteLine($"\n{ScreenshotHelper.GetScreenshotsSummary()}");

            // Abrir carpeta de screenshots automáticamente
            ScreenshotHelper.OpenScreenshotsFolder();

            Console.WriteLine("\n🎉 ¡REPORTE GENERADO EXITOSAMENTE!");
            Console.WriteLine("📍 Los screenshots están disponibles en la carpeta que se abrió");

            _driver.Quit();
            _driver.Dispose();
        }
    }
}