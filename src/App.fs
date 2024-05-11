// For more information see https://aka.ms/fsharp-console-apps

open Feliz
open Elmish
open Elmish.React

Program.mkSimple Part2.init Part2.update Part2.render
|> Program.withReactSynchronous "elmish"
|> Program.run
