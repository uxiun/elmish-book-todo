module Part3

open Feliz
open Elmish
open Elmish.React
open System

open Helper
open Util

type Todo = {
  Id: Guid
  Description: string
  Completed: bool
}

type TodoId = Guid

type TodoEditing = {
  Id: TodoId
  Description: string
}

type State = {
  TodoList: Todo list
  NewTodo: string
  TodoEditing: TodoEditing option
}

type Msg =
  | SetNewTodo of string
  | AddNewTodo
  | ToggleCompleted of TodoId
  | DeleteTodo of TodoId
  | CancelEdit
  | ApplyEdit
  | StartEdit of TodoId
  | SetEditedDescription of string

let edit (todo: Todo) : TodoEditing = {
  Id = todo.Id
  Description = todo.Description
}

let init () : State = {
  TodoList = []
  NewTodo = ""
  TodoEditing = None
}

let update msg state =
  match msg with
  | SetNewTodo s -> { state with NewTodo = s }
  | AddNewTodo when state.NewTodo = "" -> state
  | AddNewTodo ->
    // let nextId =
    //   match state.TodoList with
    //   | [] -> 0
    //   | todos -> (todos ./ List.maxBy (fun todo -> todo.Id)).Id + 1

    let nextTodo: Todo = {
      Id = Guid.NewGuid()
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
              if id = todo.Id then
                { todo with Completed = not todo.Completed }
              else
                todo
            )
    }

  | DeleteTodo id -> { state with TodoList = state.TodoList |> List.filter (fun todo -> todo.Id = id) }
  | StartEdit id ->
    let todo =
      state.TodoList ./ List.tryFind (fun todo -> todo.Id = id) ./ Option.map edit

    { state with TodoEditing = todo }

  | CancelEdit -> { state with TodoEditing = None }
  | ApplyEdit ->
    match state.TodoEditing with
    | None -> state
    | Some todoEditing when todoEditing.Description = "" -> state
    | Some todoEditing ->
      let list =
        state.TodoList
        ./ List.map (fun todo ->
          if todo.Id = todoEditing.Id then
            { todo with Description = todoEditing.Description }
          else
            todo
        )

      {
        state with
            TodoEditing = None
            TodoList = list
      }

  | SetEditedDescription desc -> {
      state with
          TodoEditing = state.TodoEditing ./ Option.map (fun t -> { t with Description = desc })
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
          "is-medium"
        ]
        prop.children [
          Html.input [
            prop.classes [ "input" ]
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
              "is-medium"
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
              "is-primary"
            ]
            prop.onClick (fun _ -> dispatch (StartEdit todo.Id))
            prop.children [
              Html.i [
                prop.classes [
                  "fa"
                  "fa-edit"
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

let renderEditForm (editing: TodoEditing) (dispatch: Msg -> unit) =
  div [ "box" ] [
    div [
      "field"
      "is-grouped"
    ] [
      div [
        "control"
        "is-expanded"
      ] [
        Html.input [
          prop.classes [
            "input"
            "is-medium"
          ]
          prop.valueOrDefault editing.Description
          prop.onTextChange (SetEditedDescription >> dispatch)
        ]
      ]

      div [
        "control"
        "buttons"
      ] [
        Html.button [
          prop.classes [
            "button"
            "is-primary"
          ]
          prop.onClick (fun _ -> dispatch ApplyEdit)
          prop.children [
            Html.i [
              prop.classes [
                "fa"
                "fa-save"
              ]
            ]
          ]
        ]

        Html.button [
          prop.classes [
            "button"
            "is-warning"
          ]
          prop.onClick (fun _ -> dispatch CancelEdit)
          prop.children [
            Html.i [
              prop.classes [
                "fa"
                "fa-arrow-right"
              ]
            ]
          ]
        ]
      ]
    ]
  ]

let todoList (state: State) (dispatch: Msg -> unit) : ReactElement =
  state.TodoList
  ./ List.map (fun todo ->
    (match state.TodoEditing with
     | Some e when e.Id = todo.Id -> renderEditForm e
     | _ -> renderTodo todo)
      dispatch
    ./ Html.li
  )
  ./ Html.ul

let render (state: State) (dispatch: Msg -> unit) : ReactElement =
  Html.div [
    prop.style [ style.padding 20 ]
    prop.children [
      appTitle
      input state dispatch
      todoList state dispatch
    ]
  ]
