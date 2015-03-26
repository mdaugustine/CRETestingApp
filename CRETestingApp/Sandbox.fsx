#r "../packages/Selenium.WebDriver.2.45.0/lib/net40/WebDriver.dll"
#r "../packages/Selenium.Support.2.45.0/lib/net40/WebDriver.Support.dll"
#r "../packages/Newtonsoft.Json.6.0.1/lib/net40/Newtonsoft.Json.dll"
#r "../packages/SizSelCsZzz.0.3.36.0/lib/SizSelCsZzz.dll"
#r "../packages/canopy.0.9.22/lib/canopy.dll"

open canopy
open runner
open System
open canopy.types
open configuration
open reporters
open OpenQA.Selenium
open OpenQA.Selenium.Support.UI
open OpenQA.Selenium.Interactions

reporter <- new LiveHtmlReporter() :> IReporter
configuration.chromeDir <- "c:\Users\Mdaugustine"
start chrome
pin FullScreen