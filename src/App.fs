﻿// For more information see https://aka.ms/fsharp-console-apps

open Feliz
open Elmish
open Elmish.React
open Root

let init () = StateDefault

let rec update (msg: Msg) (state: State) : State =

  match msg.Counter with
  | Some counter ->
    update { msg with Counter = None } {
      state with
          Counter = Counter.update counter state.Counter
    }
  | None ->
    match msg.Input with
    | Some m ->
      update { msg with Input = None } {
        state with
            Input = Input.update m state.Input
      }
    | None -> state

// let update (msg: Msg) (state: State) : State =
//     match msg.Counter, msg.Input with
//     | Some c, Some i ->
//         { state with
//             Counter = Counter.update c state.Counter
//             Input = Input.update i state.Input }
//     | Some c, None ->
//         { state with
//             Counter = Counter.update c state.Counter }
//     | None, Some i ->
//         { state with
//             Input = Input.update i state.Input }
//     | _ -> state

let render (state: State) (dispatch: Msg -> unit) : ReactElement =
  Html.div [
    Html.h1 "Elmish Vite Template"
    Counter.render state.Counter dispatch
    Input.render state.Input dispatch
  ]

Program.mkSimple init update render
|> Program.withReactSynchronous "elmish"
|> Program.run
