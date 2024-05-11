// For more information see https://aka.ms/fsharp-console-apps

open Feliz
open Elmish
open Elmish.React

Program.mkSimple Part3.init Part3.update Part3.render
|> Program.withReactSynchronous "elmish"
|> Program.run
