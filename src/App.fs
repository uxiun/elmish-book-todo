// For more information see https://aka.ms/fsharp-console-apps

open Feliz
open Elmish
open Elmish.React

Program.mkSimple WithFilterTab.init WithFilterTab.update WithFilterTab.render
|> Program.withReactSynchronous "elmish"
|> Program.run
