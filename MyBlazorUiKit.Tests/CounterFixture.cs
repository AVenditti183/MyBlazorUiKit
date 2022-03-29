using System;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MyBlazorUiKit.Services;
using NUnit.Framework;
using TestContext = Bunit.TestContext;

namespace MyBlazorUiKit.Tests
{
    public class CounterFixture
    {
        private TestContext ctx = new TestContext();
        private Mock<IServiceCounter> service;

        [SetUp]
        public void SetUp()
        {
            service = new Mock<IServiceCounter>();

            service.Setup(o => o.Increment(It.IsAny<int>()))
                .Returns((int v) => v + 1);
            service.Setup(o => o.Decrement(It.IsAny<int>()))
                .Returns((int v) => v - 1);

            ctx = new TestContext();
            ctx.Services.AddSingleton<IServiceCounter>(service.Object);
        }

        [Test]
        public void Init_ViewCountValueZero()
        {
            var counter = ctx.RenderComponent<Counter>();

            counter.MarkupMatches(@"<h1>Counter</h1>
               <p role=""status"">Current count: 0</p>
               <button class=""btn btn-primary"">Click me to increment</button>
               <button class=""btn btn-secondary"">Click me to decrement</button>
");
        }

        [Test]
        public void ClicksButtonIncrement_ShowIncrementCounter()
        {
            var counter = ctx.RenderComponent<Counter>();
            counter.Render();
            counter.SaveSnapshot();

            var button = counter.Find(".btn-primary");
            button.Click();

            var diff = counter.GetChangesSinceSnapshot();

            diff.ShouldHaveSingleTextChange("Current count: 1", null);
        }

        [TestCase(2)]
        [TestCase(100)]
        public void ClicksButtonIncrement_ShowIncrementCounter(int clicks)
        {
            var counter = ctx.RenderComponent<Counter>();
            counter.Render();
            for (int i = 1; i <= clicks; i++)
            {
                counter.SaveSnapshot();

                var button = counter.Find(".btn-primary");
                button.Click();

                var diff = counter.GetChangesSinceSnapshot();
                diff.ShouldHaveSingleTextChange($"Current count: {i}", null);
            }
        }

        [Test]
        public void ClicksButtonDecrement_ShowDecrementCounter()
        {
            var counter = ctx.RenderComponent<Counter>();
            counter.Render();
            counter.SaveSnapshot();

            var button = counter.Find(".btn-secondary");
            button.Click();

            var diff = counter.GetChangesSinceSnapshot();

            diff.ShouldHaveSingleTextChange("Current count: -1", null);
        }

        [Test]
        public void ClicksButtonIncrementAndThenDecrement_ShowCorrectCounterValue()
        {
            var counter = ctx.RenderComponent<Counter>();
            counter.Render();
            counter.SaveSnapshot();

            var buttonInc = counter.Find(".btn-primary");
            var buttonDec = counter.Find(".btn-secondary");
            buttonInc.Click();
            buttonInc.Click();

            var diff = counter.GetChangesSinceSnapshot();

            diff.ShouldHaveSingleTextChange("Current count: 2", null);
            counter.SaveSnapshot();

            buttonDec.Click();
            diff = counter.GetChangesSinceSnapshot();
            diff.ShouldHaveSingleTextChange("Current count: 1", null);
        }

        [Test]
        public void ParameterDefaultCount_ViewInitCountValueParameter()
        {
            var defaultCount = ComponentParameterFactory.Parameter("DefaultCount", 10);
            var counter = ctx.RenderComponent<Counter>(defaultCount);

            counter.MarkupMatches(@"<h1>Counter</h1>
               <p role=""status"">Current count: 10</p>
               <button class=""btn btn-primary"">Click me to increment</button>
               <button class=""btn btn-secondary"">Click me to decrement</button>
");
        }

        [Test]
        public void EventCallBack_ReturnCurrentValue()
        {
            var currentValue = -1;

            var counter = ctx.RenderComponent<Counter>();

            var callback = ComponentParameterFactory.EventCallback("OnNewCountValue", (int value) => currentValue = value);
            counter.SetParametersAndRender(callback);

            var button = counter.Find(".btn-primary");
            button.Click();

            Assert.AreEqual(1, currentValue);
        }

        [Test]
        public void ParameterDefaultCountAndEventCallBack_ReturnCurrentValue()
        {
            var currentValue = -1;

            var defaultCount = ComponentParameterFactory.Parameter("DefaultCount", 10);
            var callback = ComponentParameterFactory.EventCallback("OnNewCountValue", (int value) => currentValue = value);
            var counter = ctx.RenderComponent<Counter>(defaultCount, callback);

            var button = counter.Find(".btn-primary");
            button.Click();

            Assert.AreEqual(11, currentValue);
            counter.MarkupMatches(@"<h1>Counter</h1>
               <p role=""status"">Current count: 11</p>
               <button class=""btn btn-primary"">Click me to increment</button>
               <button class=""btn btn-secondary"">Click me to decrement</button>
");
        }
    }
}