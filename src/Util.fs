module Util

let (>.) x f = (fun y -> f y x)

let (./) x f = f x
let (/.) x y = x <| y

let (>>.) f g = fun a s -> f a s ./ g

module List =

  // let all (f: 'a -> bool) (list: 'a list) =
  //   list ./ List.fold (fun s a -> s && f a) true

  // let any (f: 'a -> bool) (list: 'a list) =
  //   match list with
  //   | [] -> true
  //   | _ -> list ./ List.map f ./ List.tryFind id ./ Option.defaultValue false

  let flatten list = List.collect id list

  // let flatMap : ('a -> list<'s>) -> list<'a> -> list<'s> = List.map >>. flatten

  let flatMap f list = List.map f list ./ flatten

  let filterMap (f: 'a -> 's option) (list: 'a list) : 's list =
    list
    ./ List.map (fun a ->
      match f a with
      | Some s -> [ s ]
      | None -> []
    )
    ./ flatten
