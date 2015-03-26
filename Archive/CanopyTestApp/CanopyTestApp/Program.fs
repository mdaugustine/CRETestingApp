﻿//these are similar to C# using statements open canopy open runner open System open canopy.types open configuration open reporters open OpenQA.Selenium open OpenQA.Selenium.Support.UI open OpenQA.Selenium.Interactions  reporter <- new LiveHtmlReporter() :> IReporter  configuration.chromeDir <- "c:\Users\Mdaugustine"  let u = "https://www.credashboards.com/" let rand = System.Random() let date() = System.DateTime.Now.ToShortDateString().Split('/') //let time() = System.DateTime.Now.ToLongTimeString().Split(':') let time() = System.DateTime.Now.ToLongDateString().TrimEnd(' ') //let timeStamp() = date().[0] + "" + date().[1] + "" + time().[0] + "" + time().[1] + "" + time().[2].Split(' ').[0] let timeStamp() = date().[0] + "/" + date().[1] + " " + time()  let private wait timeout f =     let wait = new WebDriverWait(browser, TimeSpan.FromSeconds(timeout))     wait.Until(fun _ -> (                             try                                 (f ()) = true                             with                             | :? CanopyException as ce -> raise(ce)                             | _ -> false                         )               ) |> ignore     ()  let (-->) elementA elementB =     wait elementTimeout (fun _ ->         (new Actions(browser)).DragAndDrop(elementA, elementB).Perform()         true)  let drag elementA elementB = elementA --> elementB  //start an instance of the firefox browser //start firefox start chrome  pin FullScreen  "Open Page" &&& fun _ -> url "https://www.credashboards.com/dev/html5/_new_components/dashboard.php?id=4063"  context "Login Page Tests"  "Login" &&& fun _ ->      //input username     "#username" << "mdaugustine@fischercompany.com"      //input password     "#password" << "password"      //click login     click "Login"  //End of Test 1  //End of Login  //Top Nav Tests ************************************************************************************************** context "Navigation Tests"  before (fun _ ->     on u )  "Go to second module" &&& fun _ ->      click (nth 1 ".module_grouping")  "Go left" &&& fun _ ->     click ".fa-angle-left"  "Go to first module" &&& fun _ ->          click (first ".module_grouping")  "Go to second tab" &&& fun _ ->      click "#active_tab_name"     on u     click (nth 1 ".nav_tab")  "Go to first tab" &&& fun _ ->     click "#active_tab_name"     on u     click (nth 0 ".nav_tab")   //Configure View ******************************************************************************************** context "Config Views"  before (fun _ ->     on u )  //Next two functions open all views "Open view more" &&& fun _ ->          click "#btn-view-more"      "Hide All Views" &&& fun _ ->      click "#btn-view-options"     on u     let unCheckAll selector =         elements selector         |> List.iter (fun element -> uncheck element)     unCheckAll ".menu_button_options_checkboxes"     click ".btn-primary"  //Next two functions close all views "Open view more" &&& fun _ ->          click "#btn-view-more"  "Show all views" &&& fun _ ->      click "#btn-view-options"     on u     let checkAll selector =         elements selector         |> List.iter (fun element -> check element)     checkAll ".menu_button_options_checkboxes"     click ".btn-primary"  "Iterate through views" &&! fun _ ->      let clickAll selector =         elements selector         |> List.iter (fun element ->          on u         click element)     clickAll ".btn-menu"     on u     click ".btn-danger"   //Dashboard test ************************************************************************************ context "Dashboard"  before (fun _ ->     on u )  once (fun _ ->     //click "#btn-view-more"     click "#btn-view-multi" )  "Create new view" &&& fun _ ->      click "#config_btn"     on u     click ".btn-success"     on u     //let number = rand.Next(0, 1000)     //let titles = ["Test Dashboard", Convert.ToString(number)]     //let title = String.Concat titles     "#multi_title" << "Test " + timeStamp()     let layouts = (element ".new-mv" |> elementsWithin "button")     //describe (Convert.ToString(layouts.Length))     //for i in 0 .. 14 do         //click (layouts.Item(i))     //let multiview = rand.Next(7, 20)     //for i in 6..21 do         //multiview = i     //let sections = (element ".btn-group-lg" |> elementsWithin "button")     //click (nth (sections.Length) ".btn-group-lg" |> elementsWithin "button")     //click (nth multiview ".btn-default")     click (layouts.Item(rand.Next(0, 14)))     click ".btn-primary"  "Create sections" &&& fun _ ->      let sections = (element "#multi_container" |> elementsWithin ".multi-renderer")     //let cogs = (element "#multi_container" |> elementsWithin ".glyphicon-cog")     //describe (Convert.ToString(cogs.Length))     describe (Convert.ToString(sections.Length))     for i in 0 .. (sections.Length - 1) do         let sections = (element "#multi_container" |> elementsWithin ".multi-renderer")         on u         describe (Convert.ToString(i))         //click (cogs.Item(i))         click (sections.Item(i) |> elementWithin ".glyphicon-cog")         click (sections.Item(i) |> elementWithin ".form-control")         on u         let dropdown = (sections.Item(i) |> elementWithin ".form-control" |> elementsWithin "option")         //for j in 1 .. dropdown.Length do             //press up         for j in 1 .. (rand.Next(1, dropdown.Length)) do             press down         press enter         //let cog = (sections.Item(i) |> elementWithin ".glyphicon-cog")         //click (cog)  //        let dropdown = (sections.Item(i) |> elementWithin ".form-control") //        click (dropdown)  //        sleep 2    //      let options = (dropdown |> elementsWithin "option")       //  for j in 1 .. rand.Next(1, options.Length) do     //        press down         //press enter  "Rename view" &&& fun _ ->      click "#config_btn"     on u     let tabs = (element "#configure_tabs_container" |> elementsWithin ".glyphicon-pencil")     for i in 0 .. (tabs.Length - 1) do         let rename = (nth i ".tab_editor_renderer" |> elementWithin ".glyphicon-pencil")         click rename         let number = rand.Next(0, 1000)         //let titles = ["Rename Dashboard", Convert.ToString(number)]         //let title = String.Concat titles         "#tiRenameTab" << "Rename " + timeStamp()         click ".btn-primary"         on u      click ".btn-primary"  "Full screen" &&& fun _ ->      click (element ".multi-display-container" |> elementWithin ".glyphicon-fullscreen")     click (element ".multi-display-container" |> elementWithin ".glyphicon-resize-small")  "Navigate Dashboards" &&& fun _ ->      //let clickAll selector =         //elements selector         //|> List.iter (fun element ->          //on u         //click element)      //let xpath = "//ul[@id='multi_nav']/descendant::span[text()]"      //clickAll xpath      let tabs = (element "#multi_nav" |> elementsWithin "li")     //let tabsLength = tabs.Length     //describe (Convert.ToString(tabsLength))     for i in 1..(tabs.Length - 1) do         click ((element "#multi_nav" |> elementsWithin "li").Item(i))         on u  "Delete Dashboard " &&& fun _ ->     click "#config_btn"     on u     let delete = (last ".tab_editor_renderer" |> elementWithin ".glyphicon-trash")     click delete     acceptAlert()  "Rearrange Tabs" &&& fun _ ->     click "#config_btn"     on u     let tabs = (element "#configure_tabs_container" |> elementsWithin ".tab_editor_renderer")     let first = (tabs.Item(0) |> elementWithin ".glyphicon-move")     let destination = tabs.Item(tabs.Length - 1)     drag first destination     click ".btn-primary"  context "Basic List View Functions" once (fun _ ->     click "#btn-view-list" ) before (fun _ ->     on u ) lastly (fun _ ->     ".numRecordsToDisplay" << "50" ) "Sorting" &&& fun _ ->     let columns = (element ".slick-header-columns" |> elementsWithin ".slick-header-sortable")     for i in columns do         click i         on u "list nav" &&& fun _ ->     let lists = (element "#lv_tab_menu" |> elementsWithin ".lv_tab")     for i in lists do         click i         on u     click (lists.Item(0)) "show" &&& fun _ ->     let num = ["10"; "25"; "50"; "100"; "2000"]     for i in num do         ".numRecordsToDisplay" << i         on u     ".numRecordsToDisplay" << "10"     "pagination" &&& fun _ ->     let options = (element ".selectPageNum" |> elementsWithin "option")     for i in 1 .. options.Length do         ".selectPageNum" << i.ToString()         on u     for i in 1 .. options.Length - 1 do         click ".prevPage"         on u     for i in 1 .. options.Length - 1 do         click ".nextPage"         on u     ".selectPageNum" << "1" //run all tests run()  printfn "press [enter] to exit" System.Console.ReadLine() |> ignore  quit()