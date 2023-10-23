using AngleSharp.Dom;
using Bunit;
using DemoTesting.Client.Pages.Counter;
using FluentAssertions;
using Xunit;

namespace ClientTest.Pages.CounterTest;

public class CounterTest: TestContext
{
    /*
     * as a user
     * when I click increment count
     * it should increment the current count
     */
    
    /*
     * tests should be at the interfaces of our applications. In the case of a web application, that is at the level of user interaction
     * Obviously if logic behind the code becomes complex, then we may want to have their own testable interfaces, but also
     * consider moving that logic into the backend, as we are following vertical slice architecture and our controllers can
     * serve data in a model closely adhering to what the frontend needs
     */
    
    [Fact]
    public void ClickOnIncrement_IncrementCurrentCount()
    {
        IElement countDisplay;
        var cut = RenderComponent<Counter>();
        
        countDisplay = cut.Find("[role='status']");
        countDisplay.InnerHtml.Should().Contain("Current count: 0");
        
        var button = cut.Find("button");
        button.InnerHtml.Should().Contain("Click me");
        
        button.Click();
        
        countDisplay = cut.Find("[role='status']");
        countDisplay.InnerHtml.Should().Contain("Current count: 1");
        
    }
}