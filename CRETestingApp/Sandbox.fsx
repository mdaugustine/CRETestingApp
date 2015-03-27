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
url "https://www.credashboards.com/dev/html5/_new_components/dashboard.php?id=4063"

//Global functions
let u = "https://www.credashboards.com/"
let rand = System.Random()
let timeStamp() =
    let date() = System.DateTime.Now.ToShortDateString().Split('/')
    let time() = System.DateTime.Now.ToLongTimeString().Split(' ')
    date().[0] + "/" + date().[1] + " " + time().[0]
let private wait timeout f =
    let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(timeout))
    wait.Until(fun _ -> (
                            try
                                (f ()) = true
                            with
                            | :? CanopyException as ce -> raise(ce)
                            | _ -> false
                        )
              ) |> ignore
    ()
let (-->) elementA elementB =
    wait elementTimeout (fun _ ->
        (new Actions(browser)).DragAndDrop(elementA, elementB).Perform()
        true)
let drag elementA elementB = elementA --> elementB

//Tests
"Login" &&& fun _ ->
    "#username" << "mdaugustine@fischercompany.com"
    "#password" << "password"
    click "Login"

context "Navigation Test"
before (fun _ ->
    on u
)
lastly (fun _ ->
    reload()
)
"Go to second Module" &&& fun _ ->
    click (nth 1 ".module_grouping")
"Go left" &&& fun _ ->
    click ".fa-angle-left"
"Go to first module" &&& fun _ ->
    click (first ".module_grouping")
"Go to second tab" &&& fun _ ->
    click "#active_tab_name"
    on u
    click (nth 1 ".nav_tab")
"go to first tab" &&& fun _ ->
    click "#active_tab_name"
    on u
    click (nth 0 ".nav_tab")

context "Config Views"
before (fun _ ->
    on u
)
"Open view more" &&& fun _ ->
    click "#btn-view-more"
"Hide All Views" &&& fun _ ->
    click "#btn-view-options"
    on u
    let unCheckAll selector =
        elements selector
        |> List.iter (fun element -> uncheck element)
    unCheckAll ".menu_button_options_checkboxes"
    click ".btn-primary"
"Open view more" &&& fun _ ->
    click "#btn-view-more"
"Show all views" &&& fun _ ->
    click "#btn-view-options"
    on u
    let checkAll selector =
        elements selector
        |> List.iter (fun element -> check element)
    checkAll ".menu_button_options_checkboxes"
    click ".btn-primary"
"Iterate through views" &&& fun _ ->
    let clickAll selector = 
        elements selector
        |> List.iter (fun element ->
        on u
        click element)
    clickAll ".btn-menu"
    on u
    click ".btn-danger"

context "Traditional Dashboards"
before (fun _ ->
    on u
)
once (fun _ ->
    click "#btn-view-multi"
)
"Create new view" &&& fun _ ->
    click "#config_btn"
    on u
    click ".btn-success"
    on u
    "#multi_title" << "Test " + timeStamp()
    click "#mv-option-2"
    let layouts = (element ".new-mv" |> elementsWithin "button")
    click (layouts.Item(rand.Next(0,14)))
    click ".btn-primary"
"Create Sections" &&& fun _ ->
    let sections() = (element "#multi_container" |> elementsWithin ".multi-renderer")
    describe (Convert.ToString(sections().Length))
    for i in 0 .. (sections().Length - 1) do
        on u
        describe (Convert.ToString(i))
        click (sections().Item(i) |> elementWithin ".glyphicon-cog")
        click (sections().Item(i) |> elementWithin ".form-control")
        on u
        let dropdown = (sections().Item(i) |> elementWithin ".form-control" |> elementsWithin "option")
        for j in 1 .. (rand.Next(1, dropdown.Length)) do
            press down
        press enter