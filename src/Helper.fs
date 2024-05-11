module Helper

open Feliz
open Elmish
open Elmish.React

let div (classes: string list) (children: ReactElement list) : ReactElement =
  Html.div [
    prop.classes classes
    prop.children children
  ]
