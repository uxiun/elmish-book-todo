module Part2

open Feliz
open Elmish
open Elmish.React

open Helper
open Util

type Todo = {
  Id: int
  Description: string
  Completed: bool
}

type State = { TodoList: Todo list; NewTodo: string }

type Msg =
  | SetNewTodo of string
  | AddNewTodo
  | ToggleCompleted of int
  | DeleteTodo of int

let init () : State = { TodoList = []; NewTodo = "" }

let update msg state =
  match msg with
  | SetNewTodo s -> { state with NewTodo = s }
  | AddNewTodo when state.NewTodo = "" -> state
  | AddNewTodo ->
    let nextId =
      match state.TodoList with
      | [] -> 0
      | todos -> (todos ./ List.maxBy (fun todo -> todo.Id)).Id + 1

    let nextTodo: Todo = {
      Id = nextId
      Description = state.NewTodo
      Completed = false
    }

    {
      state with
          NewTodo = ""
          TodoList = nextTodo :: state.TodoList
    }

  | ToggleCompleted id -> {
      state with
          TodoList =
            state.TodoList
            |> List.map (fun todo ->
              if todo.Id = id then
                {
                  todo with
                      Completed = not todo.Completed
                }
              else
                todo
            )
    }

  | DeleteTodo id -> {
      state with
          TodoList = state.TodoList |> List.filter (fun todo -> todo.Id <> id)
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

let renderTodo (todo: Todo) (dispatch: Msg -> unit) : ReactElement =
  div [ "box" ] [
    div [
      "columns"
      "is-mobile"
      "is-vcentered"
    ] [
      div [ "column" ] [
        Html.p [
          prop.className "subtitle"
          prop.text todo.Description
        ]
      ]
      div [
        "column"
        "is-narrow"
      ] [
        div [ "buttons" ] [
          Html.button [
            prop.classes [
              "button"
              if todo.Completed then
                "is-success"
            ]
            prop.onClick (fun _ -> dispatch (ToggleCompleted todo.Id))
            prop.children [
              Html.i [
                prop.classes [
                  "fa"
                  "fa-check"
                ]
              ]
            ]
          ]

          Html.button [
            prop.classes [
              "button"
              "is-danger"
            ]
            prop.onClick (fun _ -> dispatch (DeleteTodo todo.Id))
            prop.children [
              Html.i [
                prop.classes [
                  "fa"
                  "fa-times"
                ]
              ]
            ]
          ]
        ]
      ]
    ]

  ]

let todoList (state: State) (dispatch: Msg -> unit) : ReactElement =
  state.TodoList ./ List.map (dispatch >. renderTodo >> Html.li) ./ Html.ul

let render (state: State) (dispatch: Msg -> unit) : ReactElement =
  Html.div [
    prop.style [ style.padding 20 ]
    prop.children [
      appTitle
      input state dispatch
      todoList state dispatch
    ]
  ]
