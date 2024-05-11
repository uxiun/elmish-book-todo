module Model

type State = {
  TodoList: string list
  NewTodo: string
}

let StateDefault: State = { TodoList = []; NewTodo = "" }

type Msg =
  | SetNewTodo of string
  | AddNewTodo
