module Part1

open Feliz
open Elmish
open Elmish.React

type State = {
  TodoList: string list
  NewTodo: string
}

type Msg =
  | SetNewTodo of string
  | AddNewTodo

let init () : State = { TodoList = []; NewTodo = "" }

let update msg state =
  match msg with
  | SetNewTodo s -> { state with NewTodo = s }
  | AddNewTodo when state.NewTodo = "" -> state
  | AddNewTodo -> {
      state with
          NewTodo = ""
          TodoList = state.NewTodo :: state.TodoList
    }

let appTitle =
  Html.p [
    prop.className "title"
    prop.text "Elmish To-Do List"
  ]

let input (state: State) (dispatch: Msg -> unit) : ReactElement =
  Html.div [
    prop.classes [
      "field"
      "has-addons"
    ]
    prop.children [
      Html.div [
        prop.classes [
          "control"
          "is-expanded"
        ]
        prop.children [
          Html.input [
            prop.classes [
              "input"
              "is-medium"
            ]
            prop.valueOrDefault state.NewTodo
            prop.onChange (SetNewTodo >> dispatch)
          ]
        ]
      ]

      Html.div [
        prop.className "control"
        prop.children [
          Html.button [
            prop.classes [
              "button"
              "is-primary"
            ]
            prop.onClick (fun _ -> dispatch AddNewTodo)
            prop.children [
              Html.i [
                prop.classes [
                  "fa"
                  "fa-plus"
                ]
              ]
            ]
          ]
        ]
      ]
    ]
  ]

let todoList (state: State) (dispatch: Msg -> unit) : ReactElement =
  Html.ul [
    prop.children [
      for todo in state.TodoList ->
        Html.li [
          prop.classes [
            "box"
            "subtitle"
          ]
          prop.text todo
        ]
    ]
  ]

let render (state: State) (dispatch: Msg -> unit) : ReactElement =
  Html.div [
    prop.style [ style.padding 20 ]
    prop.children [
      appTitle
      input state dispatch
      todoList state dispatch
    ]
  ]
