using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UITests.Helpers;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using Uno.Material.Samples.Extensions;

namespace Uno.Material.Samples.UITest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void Smoke()
        {
			app.Repl();
			
        }

		[Test]
		public void Background_Color()
		{
			app.WaitForElement("NavView");
			app.ScrollDownTo("bgnError_Brush", "NavView");

			var colorValue = app.Query(x => x.Marked("bgnError_Color").Invoke("getCurrentTextColor"))[0];

		}
    }
}
