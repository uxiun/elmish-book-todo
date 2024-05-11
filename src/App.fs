// For more information see https://aka.ms/fsharp-console-apps

open Feliz
open Elmish
open Elmish.React

Program.mkSimple Part1.init Part1.update Part1.render
|> Program.withReactSynchronous "elmish"
|> Program.run
